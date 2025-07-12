using Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Models.Enums;
using POS_API.Utilities;
using POS_API.Data.TVPs;
using System.Threading.Tasks;
using Models.DTO.Notifications;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Models.DTO.InventoryManagement;
using Microsoft.Extensions.Configuration;
using Models.DTO.InventoryManagement.ViewDTO;
using StatusCodesEnums = Models.Enums.StatusCodes;
using POS_API.Repositories.InventoryManagement.ItemRepos;
using POS_API.Services.InventoryManagement.PhysicalInventory;

namespace POS_API.Services.InventoryManagement.ItemServices
{
    public class ItemService : IItemService, IService
    {
        private readonly IConfiguration _configuration;

        private readonly IItemRepository _itemRepository;

        private readonly IStockNotificationManager _stockNotificationManager;

        //private const string Paths.ItemImagesFolder = "imgs/ItemImages";
        //private const string Paths.DEFAULT_IMAGE = "Default.png";
        public ItemService(IItemRepository itemRepository, IConfiguration configuration, IStockNotificationManager stockNotificationManager)
        {
            _itemRepository = itemRepository;
            _configuration = configuration;
            _stockNotificationManager = stockNotificationManager;
        }

        public async Task<Response> Create(InvItemDto model)
        {
            if (model.InvItemRecipeChild != null)
            {
                //removing extra entries from recipe items
                model.InvItemRecipeChild = model.InvItemRecipeChild
                .Where(predicate: x => x.Status != StatusTypes.Delete.ToInt() && x.ItemId > 0 && x.Quantity > 0).GroupBy(keySelector: x => x.ItemId)
                .Select(selector: itemRecipe =>
                   new InvItemRecipeDto
                   {
                       ItemId = itemRecipe.First().ItemId, BarCodeId = itemRecipe.First().BarCodeId, Quantity = itemRecipe.Sum(x => x.Quantity),
                       Status = StatusTypes.Active.ToInt(), CompanyId = model.CompanyId, CreatedBy = model.CreatedBy, CreatedOn = model.CreatedOn
                   }).ToList();
            }

            if (model.InvItemModifiers != null)
            {
                //removing extra entries from item modifiers.
                model.InvItemModifiers = model.InvItemModifiers.Where(predicate: x => x.Status != StatusTypes.Delete.ToInt() && x.ModifierId > 0 && x.Quantity > 0)
                 .GroupBy(keySelector: x => new { x.ModifierId, x.IsMandatory })
                 .Select(selector: itemModifier =>
                  new InvItemModifierDto
                  {
                      ModifierId = itemModifier.First().ModifierId, Quantity = itemModifier.Sum(x => x.Quantity), IsMandatory = itemModifier.First().IsMandatory,
                      Status = StatusTypes.Active.ToInt(), CompanyId = model.CompanyId, CreatedBy = model.CreatedBy,CreatedOn = model.CreatedOn
                  }).ToList();
            }

            var returnModel = await _itemRepository.Create(model: model);
            var response = returnModel.Response;
            response.Model = model;
            if (!model.Response.ErrorOccured)
                _ = SendLowInventoryNotification(model: model, userId: Convert.ToInt32(value: model.CreatedBy));
            return response;
        }

        public async Task<Response> SaveImage(InvItemDto model, IFormFile file)
        {
            var host = _configuration.GetSection(key: Paths.AppSettings_ApiHost).Value;
            string oldImagePath = null;
            var response = new Response { Model = false };

            model.Status = StatusTypes.Active.ToInt();
            model = await _itemRepository.GetDetails(model: model);
            //var isBarCodeExists = model.ItemBarCode == null ? false : await this.IsItemBarCodeExist(new InvItemBarCodeDTO() { BarCode = model.ItemBarCode, CompanyId = model.CompanyId });
            if (model == null)
                return Response.Message("Item Not Found", StatusCodesEnums.Not_Found ,false);

            if (model.Id != null)
            {
                model.ImageUrl ??= "";
                model.ImageUrl = model.ImageUrl.Replace(oldValue: host, newValue: "").Replace(oldValue: Paths.DEFAULT_IMAGE, newValue: "");
                if (model.ImageUrl != "")
                    oldImagePath = GetDirectoryPath(param: model.ImageUrl.Split(separator: new[] { '/' }));
                else
                    model.ImageUrl = null;
            }

            var filepath = "";
            if (file != null)
            {
                var fi = new FileInfo(fileName: file.FileName);
                var fileName = Guid.NewGuid() + fi.Extension;

                var directoryPath = GetDirectoryPath();
                filepath = Path.Combine(path1: directoryPath, path2: fileName);
                model.ImageUrl = Paths.ItemImagesFolder + "/" + fileName;
            }

            var imageSaved = false;
            if (file != null) imageSaved = await file.SaveFileAsync(path: filepath);
            if (imageSaved)
            {
                imageSaved = await _itemRepository.UpdateImagePath(model: model);
                if (imageSaved)
                {
                    model.ImageUrl = model.ImageUrl != null ? host + model.ImageUrl : host + Paths.DEFAULT_IMAGE;
                    response.SetMessage("Items Image saved.", model: model);
                }
                else
                {
                    response.SetError("Failed to save Item Image.");
                }
                //delete old image
                if (oldImagePath != null) File.Delete(path: oldImagePath);
            }
            else
            {
                response.SetError("Failed to save Item Image.");
            }
            return response;
        }


        public async Task<Response> Delete(InvItemDto model)
        {
            return await _itemRepository.Delete(model) ? Response.Message("Item Deleted Successfully.", model: true)
                : Response.Message("Item Not Found.", StatusCodesEnums.Not_Found, false);
        }

        public async Task<Response> Edit(InvItemDto model)
        {
            var host = _configuration.GetSection(key: Paths.AppSettings_ApiHost).Value;
            var response = new Response();

            if (model.InvItemRecipeChild != null)
            {
                //removing extra entries from recipe items
                model.InvItemRecipeChild = model.InvItemRecipeChild
                .Where(predicate: x => x.Status != null && x.Status != StatusTypes.Delete.ToInt() && x.ItemId > 0 && x.Quantity > 0)
                .GroupBy(keySelector: item => item.ItemId)
                .Select(selector: itemRecipe =>
                new InvItemRecipeDto
                {
                    ItemId = itemRecipe.First().ItemId, BarCodeId = itemRecipe.First().BarCodeId,
                    Status = itemRecipe.First().Status, Quantity = itemRecipe.Sum(selector: x => x.Quantity)
                }).ToList();
            }

            if (model.InvItemModifiers != null)
            {
                //removing extra entries from item modifiers.
                model.InvItemModifiers = model.InvItemModifiers
                .Where(predicate: x => x.Status != StatusTypes.Delete.ToInt() && x.ModifierId > 0 && x.Quantity > 0)
                .GroupBy(keySelector: x => new { x.ModifierId, x.IsMandatory }).Select(selector: itemModifier =>
                   new InvItemModifierDto
                   {
                       ModifierId = itemModifier.First().ModifierId, Quantity = itemModifier.Sum( x => x.Quantity),
                       IsMandatory = itemModifier.First().IsMandatory, Status = StatusTypes.Active.ToInt(),  CompanyId = model.CompanyId
                   }).ToList();
            }

            var responseModel = await _itemRepository.Edit(model: model);
            if (responseModel != null)
            {
                response = responseModel.Response;
                //LowInventoryNotification
                _ = SendLowInventoryNotification(model: model, userId: Convert.ToInt32(value: model.ModifiedBy));
                responseModel.ImageUrl = responseModel.ImageUrl != null ? host + responseModel.ImageUrl : host + Paths.DEFAULT_IMAGE;
                response.Model = responseModel;
            }
            else
            {
                response.SetError("Item Not Found.", StatusCodesEnums.Not_Found);
            }
            return response;
        }

        public async Task<Response> GetAll(InvItemDto model, bool forPos = false, bool withModifiers = false/*, bool fromCache = false*/)
        {
            var host = _configuration.GetSection(key: Paths.AppSettings_ApiHost).Value;
            List<InvItemViewDto> res;

            if (withModifiers == false)
                res = await _itemRepository.GetAll(model: model);
            else
                res = await _itemRepository.GetAllWithModifiers(model: model/*, fromCache*/);

            if (forPos)
            {
                res = res.Where(predicate: x =>
                        x.ItemType != ItemTypes.NormalItem.ToInt() || x.RemainingInventory > 0 || x.ManageStock == false || x.AllowBackOrder == true).ToList();
                res = res.Where(predicate: x =>
                        (x.CategoryDisplayOnPos == null || x.CategoryDisplayOnPos == true) && (x.SubCategoryDisplayOnPos == null || x.SubCategoryDisplayOnPos == true)).ToList();
            }

            foreach (var item in res)
                item.ImageUrl = item.ImageUrl != null ? host + item.ImageUrl : host + Paths.DEFAULT_IMAGE;

            return res.Any() ? Response.Message(null,model:res) : Response.Error("No Item Found", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetDetails(InvItemDto model)
        {
            var host = _configuration.GetSection(key: Paths.AppSettings_ApiHost).Value;
            
            var item = await _itemRepository.GetDetails(model: model);
            if (item != null)
            {
                item.ImageUrl = item.ImageUrl != null ? host + item.ImageUrl : host + Paths.DEFAULT_IMAGE;
                return Response.Message(null,model:item);
            }

            return Response.Message("Item Not Found", StatusCodesEnums.Not_Found);
        }

        public async Task<bool> IsExist(InvItemDto model) => await _itemRepository.IsExist(model: model);

        public async Task<Response> GetSelectList(InvItemDto model)
        {
            var itemsList = await _itemRepository.GetSelectList(model: model);
            return itemsList != null ? Response.Message(null, model:itemsList) : Response.Error("Item Not Found", StatusCodesEnums.Not_Found);
        }

        public string GetBulkImportSampleFilePath()
        {
            var filePath = _configuration.GetSection(key: Paths.AppSettings_ItemsBulkImportSampleFile).Value;
            var hostPath = _configuration.GetSection(key: Paths.AppSettings_ApiHost).Value;
            var fullPath = Path.Combine(path1: hostPath, path2: filePath);
            return fullPath;
        }

        public async Task<Response> BulkUpload(InvItemDto model, IFormFile file)
        {
            if (file != null)
            {
                var fileInfo = new FileInfo(fileName: file.FileName);
                var batchGuid = Guid.NewGuid();
                var fileName = batchGuid + fileInfo.Extension;

                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration.GetSection(Paths.AppSettings_RootFolder).Value , "temp_files");
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                directoryPath = Path.Combine(directoryPath,model.CompanyId.ToString());
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                //delete existing files
                var di = new DirectoryInfo(directoryPath);
                di.GetFiles().ToList().ForEach(x => x.Delete());
                
                var filepath = Path.Combine(path1: directoryPath, path2: fileName);
                if(await file.SaveFileAsync(path: filepath))
                {
                    var lines = (await File.ReadAllLinesAsync(filepath)).ToList();
                    File.Delete(filepath);
                    var dataHead = lines[0];
                    var dataHeadList = lines[0].Split(',').ToList();
                    dataHeadList.ForEach(x => { x = x.Trim();});
                    lines.RemoveAt(0);
                    var data = new List<BulkUploadItemsTvp>();
                    var invalidItemsList = new List<string>();
                    var trueValList = new List<string> { "1", "yes", "true" };
                    foreach (var x in lines)
                    {
                        var temp = x.Split(",");
                        var itemCode = temp[0].Trim() != ""? temp[0].Trim():null;
                        var itemName = temp[1].Trim();
                        var barCode = temp[2].Trim();
                        var measurement = temp[3].Trim();
                        var unitName = temp[4].Trim();
                        var purchaseRate = temp[5]!="" ? Convert.ToDouble( temp[5].Trim()) : 0;
                        var salesRate = temp[6] != "" ? Convert.ToDouble(temp[6].Trim()) : 0;
                        //var discountAmount = temp[7] != "" ? Convert.ToDouble(temp[7].Trim()) : 0;
                        //var isDiscountInPercentage = trueValList.Contains(temp[8].Trim().ToLower());
                        var categoryName = temp[7].Trim();
                        var subCategoryName =temp[8].Trim();
                        var minimumQuantity = temp[9] != "" ? Convert.ToDouble(temp[9].Trim()):0;
                        var brandName = temp[10].Trim();
                        var colorName = temp[11].Trim();
                        var sizeName = temp[12].Trim();
                        var isReturnable = trueValList.Contains(temp[13].Trim().ToLower());
                        var displayOnPos = trueValList.Contains(temp[14].Trim().ToLower());
                        var isRawItem = trueValList.Contains(temp[15].Trim().ToLower());
                        var allowBackOrder = trueValList.Contains(temp[16].Trim().ToLower());

                        if (isRawItem) displayOnPos = false;
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        if (itemName== "" || /*purchaseRate == 0||*/  categoryName == "")
                        {
                            var invalidDataString = $",{x}";
                            if (itemName == "") invalidDataString = $"{dataHeadList[1]} can not be null.{x}";

                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            if (categoryName == "")
                                invalidDataString = $"{dataHeadList[9]} is required. {invalidDataString}";
                            invalidItemsList.Add(invalidDataString);
                        }
                        else
                        {
                            data.Add(new BulkUploadItemsTvp
                                     {
                                         ItemCode = itemCode,
                                         ItemName = itemName,
                                         BarCode = barCode,
                                         Measurement = measurement,
                                         UnitName = unitName,
                                         PurchaseRate = purchaseRate,
                                         SalesRate = salesRate,
                                         //DiscountAmount = discountAmount,
                                         //IsDiscountInPercentage = isDiscountInPercentage,
                                         CategoryName = categoryName,
                                         SubCategoryName = subCategoryName,
                                         MinimumQuantity = minimumQuantity,
                                         BrandName = brandName,
                                         ColorName = colorName,
                                         SizeName = sizeName,
                                         IsReturnable = isReturnable,
                                         DisplayOnPos = displayOnPos,
                                         IsRawItem = isRawItem,
                                         AllowBackOrder = allowBackOrder
                                     });
                        }
                    }

                    IList<BulkUploadItemsResponse> responseList = new List<BulkUploadItemsResponse>();
                    if (data.Any()) responseList = await _itemRepository.ItemsBulkUpload(model, data);

                    //save invalid items to file.
                    if (responseList != null && responseList.Any())
                    {
                        foreach (var response in responseList)
                        {
                            var strRes = new StringBuilder();
                            strRes.Append($"{response.Message},");
                            strRes.Append($"{response.Action},");
                            strRes.Append($"{response.ItemCode},");
                            strRes.Append($"{response.ItemName},");
                            strRes.Append($"{response.BarCode},");
                            strRes.Append($"{response.Measurement},");
                            strRes.Append($"{response.PurchaseRate},");
                            strRes.Append($"{response.SalesRate},");
                            strRes.Append($"{response.CategoryName},");
                            strRes.Append($"{response.SubCategoryName},");
                            strRes.Append($"{response.MinimumQuantity},");
                            strRes.Append($"{response.BrandName},");
                            strRes.Append($"{response.ColorName},");
                            strRes.Append($"{response.SizeName},");
                            strRes.Append($"{(response.IsReturnable ? "1" : "0")},");
                            strRes.Append($"{strRes}{(response.DisplayOnPos ? "1" : "0")},");
                            strRes.Append($"{(response.IsRawItem ? "1" : "0")},");
                            strRes.Append($"{strRes}{(response.DisplayOnPos ? "1" : "0")},");
                            strRes.Append($"{strRes}{(response.AllowBackOrder ? "1" : "0")},");
                            invalidItemsList.Add(strRes.ToString());
                        }
                    }
                    var hostPath = _configuration.GetSection(key: Paths.AppSettings_ApiHost).Value;
                    var path = $"{hostPath}/temp_files/{model.CompanyId}/{fileName}";
                    if (invalidItemsList.Any())
                    {
                        invalidItemsList.Insert(0, $"Message,Action,{dataHead}");
                        await File.WriteAllLinesAsync(filepath, invalidItemsList);
                    }
                    if (!invalidItemsList.Any() && responseList != null && !responseList.Any())
                        return Response.Message("Items Uploaded Successfully.");
                    
                    return invalidItemsList.Count + responseList?.Count < data.Count ? Response.Error("Items Uploaded. With few errors.", model: path) : Response.Error("Failed to Upload all items.", model: path );
                }
                return Response.Error("An Error Occurred While Uploading Items.");
            }
            return Response.Error("File can not be null.");
        }

        private string GetDirectoryPath(string[] param = null)
        {
            var folders = Paths.ItemImagesFolder.Split(separator: "/");
            var directoryPath = Path.Combine(path1: Directory.GetCurrentDirectory(), path2: _configuration.GetSection(key: Paths.AppSettings_RootFolder).Value);
            if (param == null)
                foreach (var folder in folders)
                {
                    directoryPath = Path.Combine(path1: directoryPath, path2: folder);
                    if (!Directory.Exists(path: directoryPath)) Directory.CreateDirectory(path: directoryPath);
                }
            else
                directoryPath = param.Aggregate(seed: directoryPath, func: Path.Combine);
            return directoryPath;
        }

        private async Task SendLowInventoryNotification(InvItemDto model, int userId)
        {
            //START LowInventoryNotification
            if (model.Id != null)
            {
                var notificationDto = new NotiNotificationDto(title: "Low Inventory", message: $"'{model.FullName}' is near to end.", url: "", 
                                                              referenceKey: model.Id.Value, companyId: model.CompanyId, createdBy: userId);
                var notificationsList = new List<NotiNotificationDto> { notificationDto };
                await _stockNotificationManager.LowInventoryNotifications(notiList: notificationsList) .ConfigureAwait(continueOnCapturedContext: false);
            }
            //END LowInventoryNotification
        }
    }
}