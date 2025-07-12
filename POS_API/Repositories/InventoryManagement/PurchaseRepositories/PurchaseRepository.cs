using System;
using Models;
using AutoMapper;
using System.Linq;
using Models.Enums;
using POS_API.Data;
using Models.DTO.Accounts;
using System.Threading.Tasks;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models.DTO.InventoryManagement;
using AccBillStatus = Models.Enums.AccBillStatus;
//using POS_API.Repositories.InventoryManagement.ItemRepos;
using POS_API.Repositories.AccountsManagement.AccountTransactionRepositories;

namespace POS_API.Repositories.InventoryManagement.PurchaseRepositories
{
    public class PurchaseRepository : RepositoryBase, IPurchaseRepository, IRepository
    {
        private readonly IAccountTransactionRepository _transactionRepository;
        //private readonly IItemRepository _itemRepository;

        public PurchaseRepository(PosDB_Context dbContext, IMapper mapper, IAccountTransactionRepository transactionRepository/*, IItemRepository itemRepository*/) 
            : base(dbContext: dbContext,mapper: mapper)
        {
            _transactionRepository = transactionRepository;
            //_itemRepository = itemRepository;
        }


        public async Task<InvPurchaseMasterDto> Create(InvPurchaseMasterDto purchaseMasterDto)
        {
            return await TransactionStrategy.Execute(operation: async () =>
            {
                //7
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var purchaseData = _mapper.Map<InvPurchaseMaster>(source: purchaseMasterDto);
                    var purchaseDataForPhysicalInventory = _mapper.Map<InvPurchaseMaster>(source: purchaseMasterDto);
                    purchaseData.AmountPaid = 0;
                    purchaseData.BillStatusId = AccBillStatus.Unpaid.ToInt();
                    foreach (var item in purchaseData.InvPurchaseDetail)
                    {
                        item.Status = StatusTypes.Active.ToInt();
                        item.CreatedBy = purchaseData.CreatedBy;
                        item.CreatedOn = purchaseData.CreatedOn;
                        item.CompanyId = purchaseData.CompanyId;
                    }
                    
                    
                    //save purchase entry
                    purchaseData.Status = StatusTypes.Active.ToInt();
                    await _dbContext.InvPurchaseMaster.AddAsync(entity: purchaseData);

                    var purchaseItems = purchaseData.InvPurchaseDetail.GroupBy(keySelector: pd => pd.ItemId).Select(selector: itm =>
                    new
                    {
                        ItemId = itm.Key,
                        Total = itm.Sum(x =>x.Quantity * x.PurchaseRate),
                        itm.First().Quantity
                    }).ToList();
                    var itemIds = purchaseItems.Select(selector: x => x.ItemId);

                    //save physical inventory entry
                    var inventory = _mapper.Map<InvPhysicalInventory>(source: purchaseDataForPhysicalInventory);
                    
                    //Start handle negativeInventory
                    var negativeInventory = await _dbContext.InvNegativeInventory.Where(x => itemIds.Contains(x.ItemId) && x.Quantity < 0).ToListAsync();
                    foreach (var negInv in negativeInventory)
                    {
                        var newPurchase = purchaseItems.FirstOrDefault(x => x.ItemId == negInv.ItemId);
                        if (!(newPurchase is null))
                        {
                            var tempList = inventory.InvPhysicalInventoryItem.Where(x => x.ItemId == negInv.ItemId).OrderBy(x=>x.ExpiryDate).ToList();
                            var negQuantity = negInv.Quantity;
                            foreach (var item in tempList)
                            {
                                if (item.RemainingQuantity >= Math.Abs(negQuantity))
                                {
                                    item.RemainingQuantity = item.RemainingQuantity + negQuantity;
                                    negQuantity = 0;
                                }
                                else
                                {
                                    negQuantity = (item.RemainingQuantity??0) + negQuantity;
                                    item.RemainingQuantity = 0;
                                }
                                if (negQuantity == 0)
                                    break;
                            }

                            if (newPurchase.Quantity >= Math.Abs(negInv.Quantity))
                                negInv.Quantity = 0;
                            else
                                negInv.Quantity = negInv.Quantity + newPurchase.Quantity;
                        }
                    }
                    //End handle negativeInventory

                    await _dbContext.InvPhysicalInventory.AddAsync(entity: inventory);
                    await _dbContext.SaveChangesAsync();

                    //save transaction in accounts
                    
                    var itemsInfoList = await (from item in _dbContext.InvItem
                                               join account in _dbContext.AccAccount on item.AssAccountId equals account
                                                       .Id
                                                   into itmAcc
                                               from account in itmAcc.DefaultIfEmpty()
                                               where itemIds.Contains(item.Id)
                                               select new { item, account }).ToListAsync();

                    var vendorInfo = await (from vendor in _dbContext.InvVendor
                                            join account in _dbContext.AccAccount on vendor.AccountId equals account.Id
                                                into vendorAcc
                                            from account in vendorAcc.DefaultIfEmpty()
                                            select new { vendor, account }).FirstOrDefaultAsync( x => x.vendor.Id == purchaseMasterDto.VendorId);
                    var accTransaction = new AccTransactionMasterDto
                     {
                         CompanyId = purchaseData.CompanyId,
                         CreatedBy = purchaseData.CreatedBy,
                         CreatedOn = purchaseData.CreatedOn,
                         Status = StatusTypes.Active.ToInt(),
                         SystemMade = true,
                         TransactionDate = purchaseMasterDto.PurchaseDate.GetValueOrDefault(),
                         ReferenceNo = $"BillNo: {purchaseData.BillNo}",
                         Description = $"Being Inventory Purchased from {vendorInfo.vendor.ContactName} ({vendorInfo.vendor.CompanyName}) On Credit."
                     };

                    foreach (var purchaseItem in purchaseItems)
                    {
                        var itemInfo = itemsInfoList.FirstOrDefault(predicate: x => x.item.Id == purchaseItem.ItemId);
                        if (itemInfo is not null)
                        {
                            var accTransactionForAsset = new AccTransactionDetailDto
                                                         {
                                                             AccountId = itemInfo.account.Id,
                                                             Dr = purchaseItem.Total,
                                                             Statement = $"{itemInfo.account.Title} Dr."
                                                         };
                            accTransaction.AccTransactionDetail.Add(item: accTransactionForAsset);
                        }
                    }




                    var totalPurchaseAmount = accTransaction.AccTransactionDetail.Sum(selector: x => x.Dr);
                    var accTransactionForLiability = new AccTransactionDetailDto
                                                     {
                                                         AccountId = vendorInfo.account?.Id ?? 0,
                                                         Cr = totalPurchaseAmount,
                                                         Statement = $"{vendorInfo.account?.Title} Cr."
                                                     };
                    accTransaction.AccTransactionDetail.Add(item: accTransactionForLiability);

                    await _transactionRepository.Create(accTransactionMaster: accTransaction);

                    await transaction.CommitAsync();
                    //await _itemRepository.UpdateItemsWithModifiersToCache(accTransaction.CompanyId);
                    return purchaseMasterDto;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<bool> IsExist(InvPurchaseMasterDto model)
        {
            return await _dbContext.InvPurchaseMaster.AsNoTracking()
                  .AnyAsync(predicate: x => x.Status != StatusTypes.Delete.ToInt() && x.BillNo == model.BillNo && x.CompanyId == model.CompanyId && x.Id != model.Id);
        }


        public async Task<List<InvPurchaseMasterDto>> GetAll(InvPurchaseMasterDto purchaseMasterDto,
                                                             //bool includeDetails = false,
                                                            // bool includePayments = false,
                                                             bool excludePaidBills = false)
        {

            
            var query = _dbContext.InvPurchaseMaster.AsNoTracking();
            //query = includeDetails ? query.Include(navigationPropertyPath: x => x.InvPurchaseDetail) : query;
           // query = includePayments ? query.Include(navigationPropertyPath: x => x.AccBillPayment) : query;
            query = query.Where(predicate: c => c.CompanyId == purchaseMasterDto.CompanyId);
            query = purchaseMasterDto?.Id is not null ? query.Where(predicate: r => r.Id == purchaseMasterDto.Id) : query;
            query = purchaseMasterDto?.Status is not null ? query.Where(predicate: r => r.Status == purchaseMasterDto.Status) : query;
            query = purchaseMasterDto?.BillDueDate is not null ? query.Where(predicate: c => c.BillDueDate == purchaseMasterDto.BillDueDate) : query;
            query = excludePaidBills ? query.Where(predicate: c => c.BillStatus.Id != AccBillStatus.Paid.ToInt()) : query;
            
            if (purchaseMasterDto?.FromDueDate is not null) query = query.Where(x => x.BillDueDate >= purchaseMasterDto.FromDueDate.Value.Date);
            if (purchaseMasterDto?.ToDueDate is not null) query = query.Where(x => x.BillDueDate <= purchaseMasterDto.ToDueDate.Value.Date);

            return await query.Select(x => new InvPurchaseMasterDto
               {
                   Id = x.Id, PurchaseDate = x.PurchaseDate, BillDueDate = x.BillDueDate,
                   BillAmount = x.BillAmount, AmountPaid = x.AmountPaid, BillNo = x.BillNo,
                   BillStatusId = x.BillStatusId, Status = x.Status, CreatedOn = x.CreatedOn,
                   ModifiedOn = x.ModifiedOn
               }).ToListAsync();
        }


        public async Task<InvPurchaseMasterDto> GetDetails(InvPurchaseMasterDto purchaseMasterDto,bool includePayments = false)
        {
            var purchaseMasterQuery = _dbContext.InvPurchaseMaster.AsNoTracking()
                                                .Include(navigationPropertyPath: x => x.Vendor)
                                                .Include(navigationPropertyPath: x => x.InvPurchaseDetail)
                                                .ThenInclude(navigationPropertyPath: x => x.Item)
                                                .Include(navigationPropertyPath: x => x.InvPurchaseDetail)
                                                .ThenInclude(navigationPropertyPath: x => x.BarCode).AsQueryable();
            
            purchaseMasterQuery = includePayments ? purchaseMasterQuery.Include(navigationPropertyPath: x => x.AccBillPayment): purchaseMasterQuery;

            var purchaseMasterQueryFinal = purchaseMasterQuery
                .Select(selector: purchase =>
                new
                {
                    purchase ,
                    createdBy = _dbContext
                                .Set<User>()
                                .Include(r => r.Role)
                                .Select(x => new UserDto
                                {
                                    Id = x.Id,
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    Role = new RoleDto { Id = x.Role.Id, Name = x.Role.Name }
                                }).FirstOrDefault(cu => purchase.CreatedBy == cu.Id),
                    modifiedBy = _dbContext
                                .Set<User>()
                                .Include(r => r.Role)
                                .Select(x=>new UserDto { 
                                    Id = x.Id, FirstName = x.FirstName, LastName = x.LastName,
                                    Role = new RoleDto { Id = x.Role.Id, Name = x.Role.Name }
                                })
                                .FirstOrDefault(bu => purchase.ModifiedBy == bu.Id)
                });
            var purchaseData = await purchaseMasterQueryFinal.FirstOrDefaultAsync(predicate: b => b.purchase.Id == purchaseMasterDto.Id && b.purchase.CompanyId == purchaseMasterDto.CompanyId);
            if (purchaseData is null)
                return null;

            var purchaseDto = Map<InvPurchaseMasterDto> (purchaseData.purchase);
            //purchaseDto.InvPurchaseDetail = purchaseData.purchaseDet;
            purchaseDto.CreatedByUser = purchaseData.createdBy;
            purchaseDto.ModifiedByUser =purchaseData.modifiedBy;
            return purchaseDto;
        }
    }
}