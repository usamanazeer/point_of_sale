using Models;
using System;
using AutoMapper;
using System.Data;
using System.Linq;
using Models.Enums;
using POS_API.Data;
using POS_API.Data.TVPs;
using Models.DTO.Accounts;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models.DTO.InventoryManagement;
using POS_API.Repositories.MemoryCache;
using Microsoft.Extensions.Caching.Memory;
using Models.DTO.InventoryManagement.ViewDTO;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using POS_API.Repositories.AccountsManagement.AccountRepositories;
using POS_API.Repositories.InventoryManagement.ItemBarCodeRepos;

namespace POS_API.Repositories.InventoryManagement.ItemRepos
{
    public class ItemRepository : RepositoryBase, IItemRepository, IRepository
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IItemBarCodeRepository _barCodeRepository;
        //private readonly IMemoryCache _memoryCache;
        //private readonly IMemoryCacheUtil _memoryCacheUtil;

        private string GetItemsWithModifiers_CacheKey(int companyId) => $"itemsWithModifiers{companyId}";

        public ItemRepository(PosDB_Context dbContext,IMapper mapper, IItemBarCodeRepository barCodeRepository,
                              IAccountRepository accountRepository/*, IMemoryCache memoryCache, IMemoryCacheUtil memoryCacheUtil*/) 
            : base(dbContext: dbContext, mapper: mapper)
        {
            _barCodeRepository = barCodeRepository;
            _accountRepository = accountRepository;
            //_memoryCache = memoryCache;
            //_memoryCacheUtil = memoryCacheUtil;
        }


        public async Task<InvItemDto> Create(InvItemDto itemDto)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //8
                await using var createItemTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var next = await _dbContext.InvItem.CountAsync(predicate: x => x.CompanyId == itemDto.CompanyId) + 1;
                    itemDto.ItemCode = "ITM-" + next.ToString().PadLeft(totalWidth: 4, paddingChar: '0');
                    itemDto.AccountNo = itemDto.ItemCode;

                    var isItemExist = await IsExist(itemDto: itemDto);
                    var isBarCodeExists = itemDto.InvItemBarCode[index: 0].BarCode != null &&
                    await _barCodeRepository.IsExist(model: new InvItemBarCodeDto
                    {
                        BarCode = itemDto.InvItemBarCode[index: 0].BarCode.Trim(),
                        CompanyId = itemDto.CompanyId
                    });

                    var isItemAssAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                    {
                        Title = $"{itemDto.FullName} (Ass.)",
                        ParentId = Account.Inventory.ToInt(),
                        AccNo = $"{itemDto.AccountNo}-{AccountType.Asset.ToInt()}",
                        CompanyId = itemDto.CompanyId
                    });

                    var isItemExpAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                    {
                        Title = $"{itemDto.FullName} (Exp.)",
                        ParentId = Account.InventoryExpense.ToInt(),
                        AccNo = $"{itemDto.AccountNo}-{AccountType.Expenses.ToInt()}",
                        CompanyId = itemDto.CompanyId
                    });

                    if (isItemExist || isBarCodeExists || isItemAssAccountExist || isItemExpAccountExist)
                    {
                        itemDto.Response = new Response
                        {
                            ErrorCode = StatusCodes.Error_Occured.ToInt(),
                            ErrorMessage = isItemExist
                                               ? "Item Already Exists."
                                               : isBarCodeExists
                                                   ? $"Item with BarCode '{itemDto.InvItemBarCode[index: 0].BarCode}' Already Exists."
                                                   : "Account No. Already Exists."
                        };
                        return itemDto;
                    }

                    var data = _mapper.Map<InvItem>(source: itemDto);
                    if (data.InvItemBarCode != null)
                    {
                        var barcode =
                            data.InvItemBarCode.FirstOrDefault(predicate: x => x.BarCode != null && x.BarCode.Trim() != "");
                        if (barcode != null)
                        {
                            barcode.BarCode = barcode.BarCode.Trim();
                            barcode.ItemId = data.Id;
                            barcode.CompanyId = itemDto.CompanyId;
                            barcode.Status = StatusTypes.Active.ToInt();
                            barcode.CreatedBy = itemDto.CreatedBy;
                            barcode.CreatedOn = itemDto.CreatedOn;
                        }
                        else
                        {
                            data.InvItemBarCode.Remove(item: data.InvItemBarCode.FirstOrDefault());
                        }
                    }

                    data.ItemCode = itemDto.ItemCode;
                    data.Status = StatusTypes.Active.ToInt();
                    await _dbContext.InvItem.AddAsync(entity: data);
                    
                    //create asset account for this item.
                    var itemAssDtoAccount = await _accountRepository.Create(accAccountDto: new AccAccountDto
                    {
                        Title = $"{itemDto.FullName} (Ass.)",
                        AccountTypeId = AccountType.Asset.ToInt(),
                        ParentId = Account .Inventory .ToInt(),
                        AccNo = $"{itemDto.AccountNo}-{AccountType.Asset.ToInt()}",
                        SystemMade = true,
                        IsParent = false,
                        AllowForManualTransaction = false,
                        CompanyId = data.CompanyId,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = data.CreatedOn
                    }, isEditable: false);
                    //create revenue account for this item.
                    var itemExpDtoAccount = await _accountRepository.Create(accAccountDto: new AccAccountDto
                    {
                        Title = $"{itemDto.FullName} (Exp.)",
                        AccountTypeId = AccountType.Expenses.ToInt(),
                        ParentId = Account.InventoryExpense.ToInt(),
                        AccNo = $"{itemDto.AccountNo}-{AccountType.Expenses.ToInt()}",
                        SystemMade = true,
                        IsParent = false,
                        AllowForManualTransaction = false,
                        CompanyId = data.CompanyId,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = data.CreatedOn
                    },isEditable: false);

                    if (itemAssDtoAccount.Id != null) data.AssAccountId = itemAssDtoAccount.Id.Value;
                    if (itemExpDtoAccount.Id != null) data.ExpAccountId = itemExpDtoAccount.Id.Value;
                    await _dbContext.SaveChangesAsync();

                    //add negative inventory record with Quantity.
                    await _dbContext.InvNegativeInventory.AddAsync(new InvNegativeInventory { ItemId = data.Id, Quantity = 0 });
                    await _dbContext.SaveChangesAsync();
                    await  createItemTransaction.CommitAsync();
                    itemDto.Id = data.Id;
                    itemDto.Response = new Response
                    {
                        ResponseCode = StatusCodes.Created.ToInt(),
                        ResponseMessage = "Item Created Successfully."
                    };
                    //await _memoryCacheUtil.UpdateCache_Items(data.CompanyId);
                    return itemDto;
                }
                catch (Exception)
                {
                    await createItemTransaction.RollbackAsync();
                    throw;
                }
            });
        }


        public async Task<bool> Delete(InvItemDto model)
        {
            var item = await _dbContext.InvItem.Where(predicate: x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt()).FirstOrDefaultAsync();
            if (item is null) return false;
            item.ModifiedBy = model.ModifiedBy;
            item.ModifiedOn = model.ModifiedOn;
            item.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            
            //var cacheData = await GetAllWithModifiersFromCache(new InvItemDto { CompanyId = item.CompanyId });
            //if (cacheData != null)
            //{
            //    var updatedCacheData = cacheData.Where(x => x.Id != item.Id).ToList();
            //    await _memoryCacheUtil.UpdateCache_Items(item.CompanyId, updatedCacheData);
                
            //}
            
            return true;
        }


        public async Task<InvItemDto> Edit(InvItemDto model)
        {
            var data = await _dbContext.Set<InvItem>().Include(navigationPropertyPath: x => x.InvItemRecipeParent)
                                       .Include(navigationPropertyPath: x => x.InvItemModifiers)
                                       .Where(predicate: x => x.Id == model.Id)
                                       .FirstOrDefaultAsync();

            if (data == null) return null;
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //9
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    model.AccountNo = model.ItemCode;

                    var isExist = await IsExist(itemDto: model);
                    var isAssAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                    {
                        Title =
                                                                                                    $"{model.FullName} (Ass.)",
                        ParentId = Account
                                                                                                           .Inventory
                                                                                                           .ToInt(),
                        AccNo =
                                                                                                    $"{model.AccountNo}-{AccountType.Asset.ToInt()}",
                        CompanyId = model
                                                                                                    .CompanyId,
                        Id = data.AssAccountId
                    });
                    var isExpAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                                                                                            {
                                                                                                Title =
                                                                                                    $"{model.FullName} (Exp.)",
                                                                                                ParentId = Account
                                                                                                           .InventoryExpense
                                                                                                           .ToInt(),
                                                                                                AccNo =
                                                                                                    $"{model.AccountNo}-{AccountType.Expenses.ToInt()}",
                                                                                                CompanyId = model
                                                                                                    .CompanyId,
                                                                                                Id = data.ExpAccountId
                                                                                            });

                    if (isExist || isAssAccountExist || isExpAccountExist)
                    {
                        model.Response = Response.Error(isExist ? "Item Already Exists." : "Account No. Already Exists.");
                        return model;
                    }

                    data.Name = model.Name;
                    data.FullName = model.FullName;
                    data.UnitId = model.UnitId;
                    data.BrandId = model.BrandId;
                    data.SizeId = model.SizeId;
                    data.ColorId = model.ColorId;
                    data.SubCategoryId = model.SubCategoryId;
                    data.CategoryId = model.CategoryId;
                    data.ManageStock = model.ManageStock;
                    data.DisplayOnPos = model.DisplayOnPos;
                    data.ItemType = model.ItemType;
                    data.IsDeal = model.IsDeal;
                    data.IsRecipe = model.IsRecipe;
                    data.IsRawItem = model.IsRawItem;
                    data.IsReturnable = model.IsReturnable;
                    data.MinimumQuantity = model.MinimumQuantity;
                    data.PurchaseRate = model.PurchaseRate;
                    data.SalesRate = model.SalesRate;
                    data.DiscountAmount = model.DiscountAmount;
                    data.IsDiscountInPercent = model.IsDiscountInPercent;
                    //data.TaxId = model.TaxId;
                    //data.AllowBackOrder = model.AllowBackOrder;
                    data.Status = model.Status;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;

                    //edit recipe
                    foreach (var itemOld in data.InvItemRecipeParent)
                    {
                        itemOld.Status = StatusTypes.Delete.ToInt();
                        var newItem = model.InvItemRecipeChild.FirstOrDefault(predicate: x => x.ItemId == itemOld.ItemId);
                        if (newItem != null)
                        {
                            itemOld.BarCodeId = newItem.BarCodeId <= 0 ? null : newItem.BarCodeId;
                            itemOld.Quantity = newItem.Quantity;
                            itemOld.Status = StatusTypes.Active.ToInt();
                            itemOld.ModifiedBy = data.ModifiedBy;
                            itemOld.ModifiedOn = data.ModifiedOn;
                            model.InvItemRecipeChild.Remove(item: newItem);
                        }
                    }

                    var subItems = Map<List<InvItemRecipe>>(obj: model.InvItemRecipeChild);
                    foreach (var newItem in subItems)
                    {
                        newItem.Status = StatusTypes.Active.ToInt();
                        newItem.CompanyId = data.CompanyId;
                        newItem.CreatedBy = data.ModifiedBy;
                        newItem.CreatedOn = data.ModifiedOn;
                        data.InvItemRecipeParent.Add(item: newItem);
                    }

                    //edit item modifiers
                    foreach (var itemOld in data.InvItemModifiers)
                    {
                        itemOld.Status = StatusTypes.Delete.ToInt();
                        var newItem =
                            model.InvItemModifiers.FirstOrDefault(predicate: x => x.ModifierId == itemOld.ModifierId);
                        if (newItem != null)
                        {
                            //itemOld.BarCodeId = newItem.BarCodeId <= 0 ? null: newItem.BarCodeId;
                            itemOld.Quantity = newItem.Quantity;
                            itemOld.IsMandatory = newItem.IsMandatory;
                            itemOld.Status = StatusTypes.Active.ToInt();
                            itemOld.ModifiedBy = data.ModifiedBy;
                            itemOld.ModifiedOn = data.ModifiedOn;
                            model.InvItemModifiers.Remove(item: newItem);
                        }
                    }

                    var itemModifiers = Map<List<InvItemModifiers>>(obj: model.InvItemModifiers);
                    foreach (var newItem in itemModifiers)
                    {
                        newItem.Status = StatusTypes.Active.ToInt();
                        newItem.CompanyId = data.CompanyId;
                        newItem.CreatedBy = data.ModifiedBy;
                        newItem.CreatedOn = data.ModifiedOn;
                        data.InvItemModifiers.Add(item: newItem);
                    }

                    var itemAssAccount = await _accountRepository.GetDetails(accountId: data.AssAccountId, companyId: data.CompanyId);
                    var itemExpAccount = await _accountRepository.GetDetails(accountId: data.ExpAccountId, companyId: data.CompanyId);

                    if (itemAssAccount!= null && itemAssAccount.Title != $"{model.FullName}")
                    {
                        itemAssAccount.Title = $"{model.FullName} (Ass.)";
                        itemAssAccount.ModifiedBy = data.ModifiedBy;

                        await _accountRepository.Edit(accAccountDto: itemAssAccount, forceEdit: true);
                    }

                    if (itemExpAccount != null && itemExpAccount.Title != $"{model.FullName}")
                    {
                        itemExpAccount.Title = $"{model.FullName} (Exp.)";
                        itemExpAccount.ModifiedBy = data.ModifiedBy;
                        await _accountRepository.Edit(accAccountDto: itemExpAccount, forceEdit: true);
                    }
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var returnModel = _mapper.Map<InvItemDto>(source: data);
                    returnModel.Response = Response.Message("Item Updated Successfully.", StatusCodes.Updated);
                    //await _memoryCacheUtil.UpdateCache_Items(data.CompanyId);
                    return returnModel;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }


        //public async Task UpdateItemsWithModifiersToCache(int companyId, IList<InvItemViewDto> dataList = null)
        //{
        //    if (_memoryCache.IsAvailableInCache(GetItemsWithModifiers_CacheKey(companyId)))
        //    {
        //        List<InvItemView> dataListToSave;
        //        if (dataList == null)
        //        {

        //            var query = from item in _dbContext.InvItemView
        //                        where item.CompanyId == companyId && item.Status != StatusTypes.Delete.ToInt()
        //                        select item;
        //            dataListToSave = await query.ToListAsync();
        //            _memoryCache.UpdateListToCache(GetItemsWithModifiers_CacheKey(companyId),
        //                                           dataListToSave, 10, 5, CacheItemPriority.High);
        //        }
        //        else
        //        {
        //            dataListToSave = _mapper.Map<List<InvItemView>>(dataList);
        //        }
        //        _memoryCache.UpdateListToCache(GetItemsWithModifiers_CacheKey(companyId), dataListToSave, 10,5,CacheItemPriority.High);
        //    }
        //}
        public async Task<List<InvItemViewDto>> GetAll(InvItemDto model)
        {
            var query = _dbContext.InvItemView.AsNoTracking()
                                  //.Include(x => x.InvItemBarCode)
                                  .Where(predicate: c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            query = model.Id.HasValue ? query.Where(predicate: r => r.Id == model.Id) :  ApplyModelFilters(query,model);

            query = MatchFilter(query, model);


            var totalRecords = await query.CountAsync(s => s.Status != StatusTypes.Delete.ToInt());
           
            query = Sort(query, model);
            if (model.iDisplayLength > 0)
            {
                query = query.Skip(model.iDisplayStart).Take(model.iDisplayLength);
            }
            var result = (await query
                .Select(item=> new InvItemViewDto {
                    Id = item.Id,
                    ItemCode = item.ItemCode,
                    Name = item.Name,
                    FullName = item.FullName,
                    PurchaseRate = item.PurchaseRate,
                    FinalSalesRate = item.FinalSalesRate,
                    SalesRate = item.SalesRate,
                    ImageUrl = item.ImageUrl,
                    CompanyId = item.CompanyId,
                    DisplayOnPos = item.DisplayOnPos,
                    ItemType = item.ItemType,
                    ItemTypeName = item.ItemTypeName,
                    IsDeal = item.IsDeal,
                    IsRecipe = item.IsRecipe,
                    IsRawItem = item.IsRawItem,
                    Status = item.Status,
                    BarCodeId = item.BarCodeId,
                    BarCode = item.BarCode,
                    CategoryId = item.CategoryId,
                    CategoryCode = item.CategoryCode,
                    CategoryName = item.CategoryName,
                    SubCategoryId = item.SubCategoryId,
                    SubCategoryCode = item.SubCategoryCode,
                    SubCategoryName = item.SubCategoryName,
                    UnitId = item.UnitId,
                    UnitName = item.UnitName,
                    BrandId = item.BrandId,
                    BrandName = item.BrandName,
                    ColorId = item.ColorId,
                    ColorName = item.ColorName,
                    SizeId = item.SizeId,
                    SizeName = item.SizeName,
                    RemainingInventory = item.RemainingInventory,
                }).ToListAsync()).AsEnumerable();
            var invItemViews = result.ToList();
            var resultList = invItemViews.ToList();
            if (invItemViews.Any()) resultList[0].totalRecords = totalRecords;
            return resultList;
        }
        private IQueryable<InvItemView> MatchFilter(IQueryable<InvItemView> query, InvItemDto model) {
            if (!string.IsNullOrEmpty(model.sSearch))
            {
                System.Linq.Expressions.Expression<Func<InvItemView, bool>> searchExpression = x =>
                                    x.ItemCode.Contains(model.sSearch) ||
                                    x.FullName.Contains(model.sSearch)
                                                                  || x.BarCode.Contains(model.sSearch)
                                                                  || x.CategoryCode.Contains(model.sSearch)
                                                                  || x.CategoryName.Contains(model.sSearch)
                                                                  || x.CategoryName.Contains(model.sSearch)
                                                                  || x.SubCategoryName.Contains(model.sSearch)
                                                                  || x.PurchaseRate.ToString().Contains(model.sSearch)
                                                                  || x.FinalSalesRate.ToString().Contains(model.sSearch);
                query = query.Where(searchExpression);
            }
            return query;
        }
        static IQueryable<InvItemView> Sort(IQueryable<InvItemView> data, InvItemDto model) {

            var sortColumnIndex = model.iSortCol_0;
            var sortDirection = model.sSortDir_0;
            

            System.Linq.Expressions.Expression<Func<InvItemView, string>> orderingFunction = e =>
                                    sortColumnIndex == 0 ? e.ItemCode :
                                    sortColumnIndex == 1 ? e.Name :
                                    sortColumnIndex == 2 ? e.CategoryName :
                                    sortColumnIndex == 3 ? e.SubCategoryName :
                                    sortColumnIndex == 4 ? e.PurchaseRate.ToString() : e.FinalSalesRate.ToString();


            var orderedQuery = sortDirection == "asc" ? data.OrderBy(orderingFunction) : data.OrderByDescending(orderingFunction);
            return orderedQuery;
        }
        public async Task<List<InvItemViewDto>> GetAllWithModifiers(InvItemDto model/*, bool fromCache = false*/)
        {
            var query = from item in _dbContext.InvItemView
                        where item.CompanyId == model.CompanyId
                        select item;

            IList<InvItemView> items;
            
            //if (fromCache)
            //{
            //    if (!model.DisplayDeleted) query = query.Where(predicate: c => c.Status != StatusTypes.Delete.ToInt()).AsNoTracking();
            //    query = (await _memoryCache.GetListFromCache(GetItemsWithModifiers_CacheKey(model.CompanyId),
            //                                                query,
            //                                                10,
            //                                                5,
            //                                                CacheItemPriority.High)).AsQueryable();

            //    query = ApplyModelFilters(query,model);
            //    items = query.ToList();
            //}
            //else
            //{
                query = ApplyModelFilters(query, model);
                items = await query.AsNoTracking().ToListAsync();
            //}
            
            var itemIds = items.Select(selector: x => x.Id).ToList();
            var itemModifiers =
                _mapper.Map<List<InvItemModifierDto>>(source: await (
                                                          from modifiers in
                                                              _dbContext.InvItemModifiers.Include(navigationPropertyPath
                                                                                                  : x => x.Modifier)
                                                          where itemIds.Contains(modifiers.ItemId)
                                                          select modifiers).AsNoTracking().ToListAsync());
            var result = items.Select(selector: item => new InvItemViewDto
                                                        {
                                                            Id = item.Id,
                                                            ItemCode = item.ItemCode,
                                                            Name = item.Name,
                                                            FullName = item.FullName,
                                                            Measurement = item.Measurement,
                                                            CompanyId = item.CompanyId,
                                                            DisplayOnPos = item.DisplayOnPos,
                                                            ManageStock = item.ManageStock,
                                                            ItemType = item.ItemType,
                                                            ItemTypeName = item.ItemTypeName,
                                                            IsDeal = item.IsDeal,
                                                            IsRecipe = item.IsRecipe,
                                                            IsRawItem = item.IsRawItem,
                                                            MinimumQuantity = item.MinimumQuantity,
                                                            PurchaseRate = item.PurchaseRate,
                                                            SalesRate = item.SalesRate,
                                                            DiscountAmount = item.DiscountAmount,
                                                            IsDiscountInPercent = item.IsDiscountInPercent,
                                                            FinalSalesRate = item.FinalSalesRate,
                                                            ImageUrl = item.ImageUrl,
                                                            AllowBackOrder = item.AllowBackOrder,
                                                            Status = item.Status,
                                                            CreatedOn = item.CreatedOn,
                                                            ModifiedOn = item.ModifiedOn,
                                                            BarCodeId = item.BarCodeId,
                                                            BarCode = item.BarCode,
                                                            CategoryId = item.CategoryId,
                                                            CategoryCode = item.CategoryCode,
                                                            CategoryName = item.CategoryName,
                                                            CategoryImageUrl = item.CategoryImageUrl,
                                                            CategoryDisplayOnPos = item.CategoryDisplayOnPos,
                                                            CategoryStatus = item.CategoryStatus,
                                                            SubCategoryId = item.SubCategoryId,
                                                            SubCategoryCode = item.SubCategoryCode,
                                                            SubCategoryName = item.SubCategoryName,
                                                            SubCategoryImageUrl = item.SubCategoryImageUrl,
                                                            SubCategoryDisplayOnPos = item.SubCategoryDisplayOnPos,
                                                            SubCategoryStatus = item.SubCategoryStatus,
                                                            UnitId = item.UnitId,
                                                            UnitName = item.UnitName,
                                                            UnitDescription = item.UnitDescription,
                                                            UnitStatus = item.UnitStatus,
                                                            BrandId = item.BrandId,
                                                            BrandName = item.BrandName,
                                                            BrandStatus = item.BrandStatus,
                                                            ColorId = item.ColorId,
                                                            ColorName = item.ColorName,
                                                            ColorValue = item.ColorValue,
                                                            ColorStatus = item.ColorStatus,
                                                            SizeId = item.SizeId,
                                                            SizeName = item.SizeName,
                                                            SizeStatus = item.SizeStatus,
                                                            TaxId = item.TaxId,
                                                            TaxName = item.TaxName,
                                                            TaxAmount = item.TaxAmount,
                                                            TaxIsInPercent = item.TaxIsInPercent,
                                                            TaxStatus = item.TaxStatus,
                                                            CreatedById = item.CreatedById,
                                                            CreatedByFirstName = item.CreatedByFirstName,
                                                            CreatedByLastName = item.CreatedByLastName,
                                                            ModifiedById = item.ModifiedById,
                                                            ModifiedByFirstName = item.ModifiedByFirstName,
                                                            ModifiedByLastName = item.ModifiedByLastName,
                                                            RemainingInventory = item.RemainingInventory,
                                                            ItemModifiers = 
                                                                itemModifiers
                                                                    .Where(predicate: x => x.ItemId == item.Id).ToList()
                                                        }).ToList();
            return result;
        }

        //public async Task<IList<InvItemViewDto>> GetAllWithModifiersFromCache(InvItemDto model)
        //{
        //    var cacheData = _memoryCache.Get(GetItemsWithModifiers_CacheKey(model.CompanyId));
        //    if (cacheData == null) return null;

        //    var query = ((IList<InvItemView>)cacheData).AsQueryable();

        //    query = ApplyModelFilters(query, model);
        //    var items = query.ToList();
            
        //    var itemIds = items.Select(selector: x => x.Id).ToList();
        //    var itemModifiers =
        //        _mapper.Map<List<InvItemModifierDto>>(source: await (
        //                                                  from modifiers in
        //                                                      _dbContext.InvItemModifiers.Include(navigationPropertyPath
        //                                                                                          : x => x.Modifier)
        //                                                  where itemIds.Contains(modifiers.ItemId)
        //                                                  select modifiers).AsNoTracking().ToListAsync());
        //    var result = items.Select(selector: item => new InvItemViewDto
        //    {
        //        Id = item.Id,
        //        ItemCode = item.ItemCode,
        //        Name = item.Name,
        //        FullName = item.FullName,
        //        Measurement = item.Measurement,
        //        CompanyId = item.CompanyId,
        //        DisplayOnPos = item.DisplayOnPos,
        //        ManageStock = item.ManageStock,
        //        ItemType = item.ItemType,
        //        ItemTypeName = item.ItemTypeName,
        //        IsDeal = item.IsDeal,
        //        IsRecipe = item.IsRecipe,
        //        IsRawItem = item.IsRawItem,
        //        MinimumQuantity = item.MinimumQuantity,
        //        PurchaseRate = item.PurchaseRate,
        //        SalesRate = item.SalesRate,
        //        DiscountAmount = item.DiscountAmount,
        //        IsDiscountInPercent = item.IsDiscountInPercent,
        //        FinalSalesRate = item.FinalSalesRate,
        //        ImageUrl = item.ImageUrl,
        //        AllowBackOrder = item.AllowBackOrder,
        //        Status = item.Status,
        //        CreatedOn = item.CreatedOn,
        //        ModifiedOn = item.ModifiedOn,
        //        BarCodeId = item.BarCodeId,
        //        BarCode = item.BarCode,
        //        CategoryId = item.CategoryId,
        //        CategoryCode = item.CategoryCode,
        //        CategoryName = item.CategoryName,
        //        CategoryImageUrl = item.CategoryImageUrl,
        //        CategoryDisplayOnPos = item.CategoryDisplayOnPos,
        //        CategoryStatus = item.CategoryStatus,
        //        SubCategoryId = item.SubCategoryId,
        //        SubCategoryCode = item.SubCategoryCode,
        //        SubCategoryName = item.SubCategoryName,
        //        SubCategoryImageUrl = item.SubCategoryImageUrl,
        //        SubCategoryDisplayOnPos = item.SubCategoryDisplayOnPos,
        //        SubCategoryStatus = item.SubCategoryStatus,
        //        UnitId = item.UnitId,
        //        UnitName = item.UnitName,
        //        UnitDescription = item.UnitDescription,
        //        UnitStatus = item.UnitStatus,
        //        BrandId = item.BrandId,
        //        BrandName = item.BrandName,
        //        BrandStatus = item.BrandStatus,
        //        ColorId = item.ColorId,
        //        ColorName = item.ColorName,
        //        ColorValue = item.ColorValue,
        //        ColorStatus = item.ColorStatus,
        //        SizeId = item.SizeId,
        //        SizeName = item.SizeName,
        //        SizeStatus = item.SizeStatus,
        //        TaxId = item.TaxId,
        //        TaxName = item.TaxName,
        //        TaxAmount = item.TaxAmount,
        //        TaxIsInPercent = item.TaxIsInPercent,
        //        TaxStatus = item.TaxStatus,
        //        CreatedById = item.CreatedById,
        //        CreatedByFirstName = item.CreatedByFirstName,
        //        CreatedByLastName = item.CreatedByLastName,
        //        ModifiedById = item.ModifiedById,
        //        ModifiedByFirstName = item.ModifiedByFirstName,
        //        ModifiedByLastName = item.ModifiedByLastName,
        //        RemainingInventory = item.RemainingInventory,
        //        ItemModifiers = itemModifiers.Where(predicate: x => x.ItemId == item.Id).ToList()
        //    }).ToList();
        //    return result;
        //}


        public async Task<IList<BulkUploadItemsResponse>> ItemsBulkUpload(InvItemDto model, List<BulkUploadItemsTvp> data)
        {
            if (model.CreatedBy == null) throw new Exception(message: "CreatedBy can not be null.");

            var dt = await _dbContext.Inv_ItemsBulkImport(data,model.CompanyId,model.CreatedBy.Value);
            var response = (from DataRow dr in dt.Rows
                            select new BulkUploadItemsResponse
                                   {
                                       Message = Convert.ToString(dr["Message"]),
                                       Action = Convert.ToString(dr["Action"]),
                                       ItemCode = Convert.ToString(dr["ItemCode"]),
                                       ItemName = Convert.ToString(dr["ItemName"]),
                                       BarCode = Convert.ToString(dr["BarCode"]),
                                       Measurement = Convert.ToString(dr["Measurement"]),
                                       UnitName = Convert.ToString(dr["UnitName"]),
                                       PurchaseRate = Convert.ToDouble( dr["PurchaseRate"]),
                                       SalesRate = Convert.ToDouble(dr["SalesRate"]),
                                       DiscountAmount = Convert.ToDouble(dr["DiscountAmount"]),
                                       IsDiscountInPercentage = Convert.ToBoolean(dr["IsDiscountInPercentage"]),
                                       CategoryName = Convert.ToString(dr["CategoryName"]),
                                       SubCategoryName = Convert.ToString(dr["SubCategoryName"]),
                                       MinimumQuantity = Convert.ToDouble(dr["MinimumQuantity"]),
                                       BrandName = Convert.ToString(dr["BrandName"]),
                                       ColorName = Convert.ToString(dr["ColorName"]),
                                       SizeName = Convert.ToString(dr["SizeName"]),
                                       IsReturnable = Convert.ToBoolean(dr["IsReturnable"]),
                                       DisplayOnPos = Convert.ToBoolean(dr["DisplayOnPos"]),
                                       IsRawItem = Convert.ToBoolean(dr["IsRawItem"]),
                                       //AllowBackOrder = Convert.ToBoolean(dr["AllowBackOrder"])
                                   }).ToList();
            //update items data in cache.
           // await _memoryCacheUtil.UpdateCache_Items(model.CompanyId);
            //update categories data in cache.
            //await _memoryCacheUtil.UpdateCache_Category(model.CompanyId);
            //update subcategories data in cache.
            //await _memoryCacheUtil.UpdateCache_SubCategory(model.CompanyId);
            return response;
        }


        public async Task<InvItemDto> GetDetails(InvItemDto model)
        {
            //getting item data from view
            var itemViewData = await _dbContext
                                     .InvItemView
                                     .Where(predicate: itm =>
                                                itm.Id == model.Id && itm.CompanyId == model.CompanyId)
                                     .FirstOrDefaultAsync();
            var itemData = await _dbContext.InvItem
                                           .Include(navigationPropertyPath: x => x.InvItemModifiers)
                                           .ThenInclude(navigationPropertyPath: x => x.Modifier)
                                           .Include(navigationPropertyPath: x => x.InvItemRecipeParent)
                                           .ThenInclude(navigationPropertyPath: x => x.Item)
                                           .Include(navigationPropertyPath: x => x.InvItemRecipeParent)
                                           .ThenInclude(navigationPropertyPath: x => x.BarCode)
                                           .Include(navigationPropertyPath: x => x.InvItemBarCode)
                                           .Where(predicate: itm =>
                                                      itm.Id == model.Id && itm.CompanyId == model.CompanyId)
                                           .FirstOrDefaultAsync();
            //mapping item data from itemView to item model
            var item = Map<InvItemDto>(obj: Map<InvItemViewDto>(obj: itemViewData));

            if (item == null || itemData == null) return item;

            item.InvItemModifiers =
                Map<List<InvItemModifierDto>>(obj: itemData.InvItemModifiers.Where(predicate: x =>
                                                  x.Status == StatusTypes
                                                              .Active
                                                              .ToInt())) ??
                new List<InvItemModifierDto>();
            item.InvItemRecipeChild =
                Map<List<InvItemRecipeDto>>(obj: itemData.InvItemRecipeParent.Where(predicate: x =>
                                                x.Status == StatusTypes
                                                            .Active
                                                            .ToInt())) ??
                new List<InvItemRecipeDto>();
            item.InvItemBarCode =
                Map<List<InvItemBarCodeDto>>(obj: itemData.InvItemBarCode.Where(predicate: x =>
                                                                                    x.Status == StatusTypes
                                                                                        .Active.ToInt())) ??
                new List<InvItemBarCodeDto>();

            return item;
        }


        public async Task<IList<InvItem_SLM>> GetSelectList(InvItemDto model)
        {
            var query = _dbContext.InvItemView.AsNoTracking().Where(predicate: x => x.CompanyId == model.CompanyId);

            //if getting by id, don't apply filters.
            query = model.Id.HasValue ? query.Where(predicate: r => r.Id == model.Id) : ApplyModelFilters(query, model);

            return await query
                .Select(x=>new InvItem_SLM
                { 
                    Value = x.Id.ToString(), Text = x.Name, BarCode = x.BarCode, FullName = x.FullName, Measurement = x.Measurement, UnitId = Convert.ToString(x.UnitId),
                    UnitName = x.UnitName,SalesRate = Convert.ToString(x.SalesRate), PurchaseRate = Convert.ToString(x.PurchaseRate), FinalSalesRate = Convert.ToString(x.FinalSalesRate),
                    //IsDiscountInPercent = x.IsDiscountInPercent, 
                    //DiscountAmount = x.DiscountAmount
                })
                .ToListAsync();
        }

        public async Task<bool> IsExist(InvItemDto itemDto)
        {
            return await _dbContext.InvItem.AsNoTracking()
                .AnyAsync(predicate: x =>
                                x.Name == itemDto.Name &&
                                x.BrandId == itemDto.BrandId &&
                                x.ColorId == itemDto.ColorId &&
                                x.SizeId == itemDto.SizeId &&
                                x.CompanyId == itemDto.CompanyId &&
                                x.Status != StatusTypes.Delete.ToInt() &&
                                x.Id != itemDto.Id);
        }


        public async Task<bool> UpdateImagePath(InvItemDto model)
        {
            var data = await _dbContext.InvItem.FindAsync(model.Id);
            if (data is null) return false;
            data.ImageUrl = model.ImageUrl;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return true;
        }


        private IQueryable<InvItemView> ApplyModelFilters(IQueryable<InvItemView> queryable, InvItemDto model)
        {
            if (model.Id.HasValue) queryable = queryable.Where(predicate: r => r.Id == model.Id);
            if (!model.DisplayDeleted) queryable = queryable.Where(predicate: c => c.Status != StatusTypes.Delete.ToInt());
            if (model.CategoryId.HasValue) queryable = queryable.Where(predicate: c => c.CategoryId == model.CategoryId);

            if (model.SubCategoryId.HasValue)
                queryable = queryable.Where(predicate: c => c.SubCategoryId == model.SubCategoryId);

            if (model.BrandId.HasValue) queryable = queryable.Where(predicate: c => c.BrandId == model.BrandId);

            if (model.UnitId.HasValue) queryable = queryable.Where(predicate: c => c.UnitId == model.UnitId);

            if (model.ColorId.HasValue) queryable = queryable.Where(predicate: c => c.ColorId == model.ColorId);

            if (model.SizeId.HasValue) queryable = queryable.Where(predicate: c => c.SizeId == model.SizeId);

            if (model.TaxId.HasValue) queryable = queryable.Where(predicate: r => r.TaxId == model.TaxId);

            //if (!model.DisplayDeleted) queryable = queryable.Where(predicate: c => c.Status != StatusTypes.Delete.ToInt());

            if (model.Status.HasValue) queryable = queryable.Where(predicate: r => r.Status == model.Status);
            if (model.DisplayOnPos) queryable = queryable.Where(predicate: x => x.DisplayOnPos == true);

            if (model.ItemTypesFilter != null)
                queryable = queryable.Where(predicate: x => model.ItemTypesFilter.Contains(x.ItemType.Value));

            if (!string.IsNullOrEmpty(value: model.SearchText))
                queryable = queryable.Where(predicate: r => r.Name.ToLower().Contains(model.SearchText.ToLower()));
            return queryable;
        }
    }
}