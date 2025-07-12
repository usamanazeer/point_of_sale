using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Services.InventoryManagement.ItemServices;
using Pos_WebApp.Services.InventoryManagement.UnitServices;
using Pos_WebApp.Services.InventoryManagement.CategoryServices;
using Pos_WebApp.Services.InventoryManagement.BrandServices;
using Pos_WebApp.Services.InventoryManagement.ColorServices;
using Pos_WebApp.Services.InventoryManagement.SizeServices;
using StatusCodesEnums = Models.Enums.StatusCodes;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using System.Linq;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), Route("[controller]")]
    public class ItemsController : BaseController
    {
        private readonly IItemService _itemService;
        private readonly IUnitService _unitService;
        private readonly IMainCategoryService _mainCategoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IBrandService _brandService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        public ItemsController(
            IItemService itemService,
            IUnitService unitService, 
            IMainCategoryService mainCategoryService, 
            ISubCategoryService subCategoryService,
            IBrandService brandService,
            IColorService colorService,
            ISizeService sizeService) :base("/Items")
        {
            _itemService = itemService;
            _unitService = unitService;
            _mainCategoryService = mainCategoryService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
            _colorService = colorService;
            _sizeService = sizeService;
        }

        [JsonResponseAction, RightAuthorization(RightName = "Items"), HttpGet(nameof(GetAll))]
        public async Task<JsonResult> GetAll(InvItemDto model) {
            var res = await _itemService.Get(TOKEN, model);
            var items = res.ViewList;
            var totalRecords = 0;
            if (items.Any())
                totalRecords = items[0].totalRecords;
            return Json(new { model.sEcho, iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords, aaData = items} );
        }

        [RightAuthorization]
        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            var model = new InvItemDto { Id = id, Status = status };
            try
            {
                 
                    if (getDeleted.HasValue) model.DisplayDeleted = getDeleted.Value;
                    var bulkUploadFilePath = await _itemService.GetItemsBulkUploadSamplePath(TOKEN);
                    ViewBag.BulkUploadSampleFilePath = bulkUploadFilePath;
            }
            catch (Exception)
            {
                //return Error(global::Models.Response.Error("An Error Occurred, while loading items."));
            }
            return View(model);
        }

        [RightAuthorization, HttpGet(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new InvItemDto { InvItemBarCode = new List<InvItemBarCodeDto> { new InvItemBarCodeDto { BarCode = "" } } };
                ViewBag.Units = (await _unitService.Get(TOKEN)).Units ?? new List<InvUnitDto>();
                ViewBag.Categories = (await _mainCategoryService.Get(TOKEN)).MainCategories ?? new List<InvCategoryDto>();
                ViewBag.Brands = (await _brandService.Get(TOKEN)).Brands ?? new List<InvBrandDto>();
                ViewBag.Colors = (await _colorService.Get(TOKEN)).Colors ?? new List<InvColorDto>();
                ViewBag.Sizes = (await _sizeService.Get(TOKEN)).Sizes ?? new List<InvSizeDto>();
                return View(model);
            }
            catch (Exception )
            {
                return Error(global::Models.Response.Error("An Error Occurred."));
            }
        }
        
        [JsonResponseAction, RightAuthorization, HttpPost, Route(nameof(Create), Name = "CreateItem")]
        public async Task<IActionResult> Create(InvItemDto model, IFormFile itemImage)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _itemService.Create(TOKEN, model, itemImage)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating item."));
            }
        }

        [RightAuthorization, HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _itemService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading item data."), backUrl: IndexUrl);
            }
        }

        [RightAuthorization, HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = (await _itemService.Details(TOKEN, id));
                if (model.Response.ResponseCode == StatusCodesEnums.OK.ToInt())
                {
                    ViewBag.Units = (await _unitService.Get(TOKEN)).Units ?? new List<InvUnitDto>();
                    ViewBag.Categories = (await _mainCategoryService.Get(TOKEN)).MainCategories ?? new List<InvCategoryDto>();
                    ViewBag.SubCategories = model.CategoryId.HasValue ? (await _subCategoryService.Get(TOKEN, new InvSubCategoryDto { CategoryId = model.CategoryId })).SubCategories : new List<InvSubCategoryDto>();
                    ViewBag.Brands = (await _brandService.Get(TOKEN)).Brands ?? new List<InvBrandDto>();
                    ViewBag.Colors = (await _colorService.Get(TOKEN)).Colors ?? new List<InvColorDto>();
                    ViewBag.Sizes = (await _sizeService.Get(TOKEN)).Sizes ?? new List<InvSizeDto>();
                }
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading item data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(InvItemDto model, IFormFile itemImage)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _itemService.Edit(TOKEN, model, itemImage));

                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating item data."));
            }
        }

        [JsonResponseAction, RightAuthorization, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _itemService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting item."));
            }
        }

        [JsonResponseAction, RightAuthorization(RightName = "Items"), HttpGet(nameof(GetAddItemRecipeRow))]
        public async Task<IActionResult> GetAddItemRecipeRow(int rowNo, int itemType)=> await Task.FromResult(ViewComponent("AddItem_SubItemRow", new Tuple<int, InvItemRecipeDto, int>(rowNo, null, itemType)));

        [JsonResponseAction, RightAuthorization(RightName = "Items"), HttpGet(nameof(GetAddItemModifierRow))]
        public async Task<IActionResult> GetAddItemModifierRow(int rowNo) => await Task.FromResult(ViewComponent("AddItem_ItemModifiersRow", new Tuple<int, InvItemModifierDto>(rowNo, null)));

        [JsonResponseAction, HttpGet(nameof(GetSelectList))]
        public async Task<JsonResult> GetSelectList()
        {
            try
            {
                return Json(await _itemService.GetSelectListResponse(TOKEN));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while getting items select-list."));
            }
        }

        [JsonResponseAction, RightAuthorization, HttpPost(nameof(BulkUpload))]
        public async Task<JsonResult> BulkUpload(IFormFile file)
        {
            try
            {
                return Json(await _itemService.BulkUpload(file, TOKEN));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while uploading items."));
            }
        }
    }
}