using Models;
using System;
using AutoMapper;
using System.Data;
using System.Linq;
using Models.Enums;
using POS_API.Data;
using POS_API.Data.TVPs;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using POS_API.Utilities.Mapper;
using System.Collections.Generic;
using Models.DTO.SalesManagement;
using Microsoft.EntityFrameworkCore;
using Models.DTO.ViewModels.SelectList.SalesManagement;
//using POS_API.Repositories.InventoryManagement.ItemRepos;
//using POS_API.Repositories.AccountsManagement.AccountTransactionRepositories;

namespace POS_API.Repositories.SalesManagement.OrderRepos
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMember.Global
    internal class OrderRepository : RepositoryBase, IOrderRepository, IRepository
    {
        //private readonly IItemRepository _itemRepository;
        public OrderRepository(PosDB_Context dbContext, IMapper mapper
                               //, IItemRepository itemRepository
        ) : base(dbContext, mapper)
        {
            
        }
        //_itemRepository = itemRepository;


        public async Task<SalesOrderMasterDto> PlaceOrder(SalesOrderMasterDto model)
        {
            var billModel = new SalesOrderBillingDto {
                CashReturn = model.SalesOrderBilling.CashReturn,
                CashReceived = model.SalesOrderBilling.CashReceived,
                PaymentType = model.SalesOrderBilling.PaymentType,
                TotalBillAmount = model.SalesOrderBilling.TotalBillAmount,
                TaxId  = model.SalesOrderBilling.TaxId,
                TaxAmount = model.SalesOrderBilling.TaxAmount,
                IsTaxInPercent = model.SalesOrderBilling.IsTaxInPercent
            };
            model.SalesOrderBilling = null;
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //18
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    //await using var context = _dbContext;
                    //var companyId = model.CompanyId;
                    //var branchId = model.BranchId;
                    var data = _mapper.Map<SalesOrderMaster>(model);

                    var orderTables = ConvertOrderToDataTable(model);
                    var itemsDataTable = orderTables.Item1;
                    var modifiersDataTable = orderTables.Item2;
                    //check if inventory available
                    var consumableInventoryDs = await _dbContext.Sales_GetConsumableInventory(itemsDataTable, modifiersDataTable);
                    var consumableInventoryTable = consumableInventoryDs.Tables[0];
                    var inventoryMessagesTable = consumableInventoryDs.Tables[1];


                    model.Response = new Response();

                    if (inventoryMessagesTable.Rows.Count > 0)
                    {
                        //if inventory not available in requested quantity. return.
                        model.Response.SetError("Requested Quantity is Not Available", StatusCodes.Quantity_Not_Available, ConvertDataTableToInventoryResponseMessageList(inventoryMessagesTable));
                        return model;
                    }
                    //here..... if inventory is available in requested quantity.


                    Map_ItemsAndModifiersInfo_ToOrderDetails(data, itemsDataTable, modifiersDataTable);


                    //place order.
                    data.OrderNo = await GetNextOrderNo(model);
                    data.Status = StatusTypes.Active.ToInt();

                    await _dbContext.SalesOrderMaster.AddAsync(data);
                    if (data.OrderTypeId == OrderTypes.DineIn.ToInt())
                    {
                        var table = await _dbContext.RestDiningTable.FindAsync(data.DiningTableId);
                        table.IsOccupied = true;
                    }
                    await _dbContext.SaveChangesAsync();

                    //consume inventory from inventory table
                    var dt = await _dbContext.Sales_ConsumeInventory(consumableInventoryTable, data.Id);
                    if ((bool)dt.Rows[0]["IsSuccessed"])
                    {
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        throw ((string)dt.Rows[0]["ErrorMessage"]).ToException();
                    }
                    //generate bill
                    model.Id = data.Id;
                    model.OrderNo = data.OrderNo;
                    model.Status = data.Status;
                    //model = Map<SalesOrderMasterDto>(data);
                    //await _itemRepository.UpdateItemsWithModifiersToCache(data.CompanyId);
                    if (model.OrderTypeId == OrderTypes.DineIn.ToInt() || model.OrderTypeId == OrderTypes.Delivery.ToInt())
                        return model;  //await GetDetails(model);

                    model.SalesOrderBilling = billModel;
                    await Checkout(model);
                    return await GetDetails(model);
                }
                catch (Exception )
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
            
        }
        public async Task<SalesOrderMasterDto> Edit(SalesOrderMasterDto model)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //19
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var data = await _dbContext.SalesOrderMaster.Include(x => x.SalesOrderDetails).FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (data is null) return null;

                    var orderTables = ConvertOrderToDataTable(model);
                    var itemsDataTable = orderTables.Item1;
                    var modifiersDataTable = orderTables.Item2;
                    //check if inventory available
                    var consumableInventoryDs = await _dbContext.Sales_GetConsumableInventory(itemsDataTable, modifiersDataTable);
                    var consumableInventoryTable = consumableInventoryDs.Tables[0];
                    var inventoryMessagesTable = consumableInventoryDs.Tables[1];

                    model.Response = new Response();
                    if (inventoryMessagesTable.Rows.Count > 0)
                    {
                        //if inventory not available in requested quantity. return.
                        model.Response.SetError("Requested Quantity is Not Available", StatusCodes.Quantity_Not_Available, ConvertDataTableToInventoryResponseMessageList(inventoryMessagesTable));
                        return model;
                    }


                    //consume inventory from inventory table
                    var dt = await _dbContext.Sales_ConsumeInventory(consumableInventoryTable, data.Id);
                    if ((bool)dt.Rows[0]["IsSuccessed"] == false)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception((string)dt.Rows[0]["ErrorMessage"]);
                    }

                    var orderDetailsOld = await _dbContext.SalesOrderDetails.Include(x => x.SalesOrderItemModifiers).Where(x => x.OrderId == model.Id).ToListAsync();

                    _dbContext.SalesOrderDetails.RemoveRange(orderDetailsOld);
                    await _dbContext.SaveChangesAsync();


                    data.DiscountAmount = model.DiscountAmount;
                    data.IsDiscountInPercent = model.IsDiscountInPercent;
                    data.OrderTypeId = model.OrderTypeId;

                    data.WaiterId = model.WaiterId;
                    data.OrderStatusId = model.OrderStatusId;
                    data.BranchId = model.BranchId;
                    if (model.Status.HasValue) data.Status = model.Status.Value;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;
                    //if dining table is changed for this order.
                    if (data.OrderTypeId == OrderTypes.DineIn.ToInt() && data.DiningTableId != model.DiningTableId)
                    {
                        //set old table to free.
                        var tableOld = await _dbContext.RestDiningTable.FindAsync(data.DiningTableId);
                        tableOld.IsOccupied = false;
                        //set new/current/updated table to occupied.
                        var tableNew = await _dbContext.RestDiningTable.FindAsync(model.DiningTableId);
                        tableNew.IsOccupied = true;
                    }
                    data.DiningTableId = model.DiningTableId;
                    //add new
                    data.SalesOrderDetails = _mapper.Map<IList<SalesOrderDetails>>(model.SalesOrderDetails);

                    Map_ItemsAndModifiersInfo_ToOrderDetails(data, itemsDataTable, modifiersDataTable);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    //await _itemRepository.UpdateItemsWithModifiersToCache(data.CompanyId);
                    return _mapper.Map<SalesOrderMasterDto>(await GetDetails(model));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
            
        }
        private void Map_ItemsAndModifiersInfo_ToOrderDetails(SalesOrderMaster data, DataTable itemsDataTable, DataTable modifiersDataTable)
        {
            var itemIds = itemsDataTable.AsEnumerable().Select(r => r.Field<int>("ItemId")).ToList();
            var modifierIds = modifiersDataTable.AsEnumerable().Select(r => r.Field<int>("ModifierId")).ToList();
            var itemsList = _dbContext.InvItem.Include(x => x.Tax).Where(x => itemIds.Contains(x.Id)).ToList();
            var modifiersList = _dbContext.InvModifier.Where(x => modifierIds.Contains(x.Id)).ToList();

            data.SalesOrderDetails = data.SalesOrderDetails.Select(sod =>
            {
                var item = itemsList.FirstOrDefault(itm => itm.Id == sod.ItemId);
                if (item is not null)
                {
                    sod.SalesRate = item.SalesRate ?? 0;
                    sod.DiscountAmount = item.DiscountAmount;
                    sod.IsDiscountInPercent = item.IsDiscountInPercent;
                    sod.FinalSalesRate = item.FinalSalesRate ?? 0;
                    if (item.Tax is not null)
                    {
                        sod.TaxId = item.TaxId;
                        sod.TaxAmount = item.Tax.Amount;
                        sod.IsTaxInPercent = item.Tax.IsInPercent;
                    }
                }
                // ReSharper disable once IdentifierTypo
                sod.SalesOrderItemModifiers = sod.SalesOrderItemModifiers.Select(soim =>
                {
                    var modifier = modifiersList.FirstOrDefault(itmMod => itmMod.Id == soim.ModifierId);
                    if (modifier is not null) soim.Charges = modifier.ModifierCharges;
                    return soim;
                }).ToList();
                return sod;
            }).ToList();
        }
        
        [Obsolete]
        public async Task<bool> Delete(SalesOrderMasterDto model)
        {
            var order = await _dbContext.SalesOrderMaster.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            if (order is null) return false;
            order.ModifiedBy = model.ModifiedBy;
            order.ModifiedOn = model.ModifiedOn;
            order.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ChangeOrderStatus(SalesOrderMasterDto model)
        {
            var order = await _dbContext.SalesOrderMaster.FindAsync(model.Id);
            if (order is null) return false;

            order.OrderStatusId = model.OrderStatusId;
            order.ModifiedBy = model.ModifiedBy;
            order.ModifiedOn = model.ModifiedOn;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private IQueryable<SalesOrderMaster> ApplyModelFilters(IQueryable<SalesOrderMaster> queryable, SalesOrderMasterDto model)
        {
            if (model.Id.HasValue)
            {
                queryable = queryable.Where(r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted) queryable = queryable.Where(c => c.Status != StatusTypes.Delete.ToInt());

                if (model.Status.HasValue) queryable = queryable.Where(r => r.Status == model.Status);
                if (model.OrderNo != null) queryable = queryable.Where(r => r.OrderNo == model.OrderNo);
                if (model.OrderTypeId.HasValue) queryable = queryable.Where(r => r.OrderTypeId == model.OrderTypeId);
                if (model.OrderStatusId.HasValue)
                    queryable = queryable.Where(r => r.OrderStatusId == model.OrderStatusId);
                if (model.FromDate.HasValue)
                    queryable = queryable.Where(r => r.CreatedOn.Value.Date >= model.FromDate.Value.Date);
                if (model.ToDate.HasValue)
                    queryable = queryable.Where(r => r.CreatedOn.Value.Date <= model.ToDate.Value.Date);
            }
            return queryable;
        }
        public async Task<List<SalesOrderMasterDto>> GetAll(SalesOrderMasterDto model)
        {
            var query = _dbContext.SalesOrderMaster.AsNoTracking().Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            query = ApplyModelFilters(query, model);
            query = MatchFilter(query, model);
            var totalRecords = await query.AsNoTracking().Where(s => s.Status != StatusTypes.Delete.ToInt()).CountAsync();
            query = Sort(query, model);
            if (model.iDisplayLength > 0) query = query.Skip(model.iDisplayStart).Take(model.iDisplayLength);
            var result = await query.Select(x=> new SalesOrderMasterDto 
            { 
                Id = x.Id, OrderNo = x.OrderNo, DiscountAmount = x.DiscountAmount, IsDiscountInPercent = x.IsDiscountInPercent, TaxId = x.TaxId, TaxAmount = x.TaxAmount,
                IsTaxInPercent = x.IsTaxInPercent, OrderTypeId = x.OrderTypeId, DiningTableId = x.DiningTableId, WaiterId = x.WaiterId, DeliveryServiceVendorId = x.DeliveryServiceVendorId, 
                IsSelfDelivery = x.IsSelfDelivery, DeliveryServiceReferenceNo = x.DeliveryServiceReferenceNo, DeliveryBoyId = x.DeliveryBoyId, DeliveryCharges = x.DeliveryCharges, 
                IsChargesInPercent = x.IsChargesInPercent, OrderStatusId = x.OrderStatusId
            }).ToListAsync();

            if (result.Any()) result[0].totalRecords = totalRecords;
            return result;
        }
        private IQueryable<SalesOrderMaster> MatchFilter(IQueryable<SalesOrderMaster> query, SalesOrderMasterDto model)
        {
            if (!string.IsNullOrEmpty(model.sSearch))
            {
                var orderTypesToSearch = new List<int>();

                if (ValuesHelper.OrderType_Delivery.ToLower().Contains(model.sSearch.ToLower().Trim()))
                    orderTypesToSearch.Add(ValuesHelper.Get_OrderTypeId(ValuesHelper.OrderType_Delivery));

                if (ValuesHelper.OrderType_TakeAway.ToLower().Contains(model.sSearch.ToLower().Trim()))
                    orderTypesToSearch.Add(ValuesHelper.Get_OrderTypeId(ValuesHelper.OrderType_TakeAway));

                if (ValuesHelper.OrderType_DineIn.ToLower().Contains(model.sSearch.ToLower().Trim()))
                    orderTypesToSearch.Add(ValuesHelper.Get_OrderTypeId(ValuesHelper.OrderType_DineIn));


                var orderStatusToSearch = new List<int>();

                if (ValuesHelper.OrderStatus_Placed.ToLower().Contains(model.sSearch.ToLower().Trim()))
                    orderStatusToSearch.Add(ValuesHelper.Get_OrderTypeId(ValuesHelper.OrderStatus_Placed));

                if (ValuesHelper.OrderStatus_Served.ToLower().Contains(model.sSearch.ToLower().Trim()))
                    orderStatusToSearch.Add(ValuesHelper.Get_OrderTypeId(ValuesHelper.OrderStatus_Served));

                if (ValuesHelper.OrderStatus_Delivered.ToLower().Contains(model.sSearch.ToLower().Trim()))
                    orderStatusToSearch.Add(ValuesHelper.Get_OrderTypeId(ValuesHelper.OrderStatus_Delivered));

                if (ValuesHelper.OrderStatus_Billed.ToLower().Contains(model.sSearch.ToLower().Trim()))
                    orderStatusToSearch.Add(ValuesHelper.Get_OrderTypeId(ValuesHelper.OrderStatus_Billed));

                if (ValuesHelper.OrderStatus_Cancelled.ToLower().Contains(model.sSearch.ToLower().Trim()))
                    orderStatusToSearch.Add(ValuesHelper.Get_OrderTypeId(ValuesHelper.OrderStatus_Cancelled));

                if (ValuesHelper.OrderStatus_Returned.ToLower().Contains(model.sSearch.ToLower().Trim()))
                    orderStatusToSearch.Add(ValuesHelper.Get_OrderTypeId(ValuesHelper.OrderStatus_Returned));

                query = query.Where(e => e.OrderNo.Contains(model.sSearch) || orderTypesToSearch.Contains(e.OrderTypeId ?? 0) || orderStatusToSearch.Contains(e.OrderStatusId ?? 0));
            }
            return query;
        }
        private IQueryable<SalesOrderMaster> Sort(IQueryable<SalesOrderMaster> query, SalesOrderMasterDto model)
        {
            var sortColumnIndex = model.iSortCol_0;
            var sortDirection = model.sSortDir_0;
            System.Linq.Expressions.Expression <Func<SalesOrderMaster, string>> orderingFunction = e =>
                    sortColumnIndex == 0 ? e.OrderNo :
                    sortColumnIndex == 1 ? e.OrderTypeId.ToString() :
                    sortColumnIndex == 2 ? e.OrderStatusId.ToString() :
                    sortColumnIndex == 3 ? e.CreatedOn.ToString() : e.ModifiedOn.ToString();

            query = sortDirection == "asc" ? query.OrderBy(orderingFunction) : query.OrderByDescending(orderingFunction);
            return query;
        }
        public async Task<SalesOrderMasterDto> GetDetails(SalesOrderMasterDto model)
        {
            var data = (await _dbContext.SalesOrderMaster.AsNoTracking()
                .Include(x => x.SalesOrderDetails).ThenInclude(x => x.Item)
                .Include(x => x.SalesOrderDetails).ThenInclude(x => x.SalesOrderItemModifiers).ThenInclude(x => x.Modifier)
                .Include(x => x.DiningTable).ThenInclude(x => x.Floor)
                .Include(x => x.Waiter)
                .Include(x => x.SalesOrderBilling).ThenInclude(x => x.Tax)
                .Select(order =>
                    new
                    {
                        order,
                        cu = UserWithRoleSelect.FirstOrDefault(cu => order.CreatedBy == cu.Id),
                        bu = UserWithRoleSelect.FirstOrDefault(bu => order.ModifiedBy == bu.Id)
                    })
                    .Select(record => new { record.order, record.cu, record.bu })
                .FirstOrDefaultAsync(x => x.order.Id == model.Id && x.order.CompanyId == model.CompanyId));
            
            if (data is null) return null;
            //data.cu.Company.Branch = await _dbContext.Branch.Where(c => c.CompanyId == model.CompanyId).AsNoTracking().ToListAsync();
            var dataModel = Map<SalesOrderMasterDto>(data.order);
            dataModel.CreatedByUser = data.cu;
            dataModel.ModifiedByUser = data.bu;
            return dataModel;
        }
        public async Task<bool> IsExist(SalesOrderMasterDto model)
        {
            return await _dbContext.SalesOrderMaster.AsNoTracking()
               .AnyAsync(x => x.OrderNo == model.OrderNo &&
                  x.Status != StatusTypes.Delete.ToInt() &&
                  x.BranchId == model.BranchId &&
                  x.CompanyId == model.CompanyId &&
                  x.Id != model.Id);
        }
        public async Task<IList<SalesOrderMaster_SLM>> GetSelectList(SalesOrderMasterDto model)
        {
            var orders = await _dbContext.SalesOrderMaster.AsNoTracking().Where(x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .Select(x => new SalesOrderMaster_SLM { 
                    Value = x.Id.ToString(), Text = x.OrderNo, DiningTableId = x.DiningTableId, DiscountAmount = x.DiscountAmount, IsDiscountInPercent = x.IsDiscountInPercent,
                    OrderTypeId = x.OrderTypeId, WaiterId = x.WaiterId }).ToListAsync();
            return orders;
        }
        private async Task<string> GetNextOrderNo(SalesOrderMasterDto model)
        {
            var nextOrderNo = (await _dbContext.SalesOrderMaster.AsNoTracking().CountAsync(x => x.CompanyId == model.CompanyId && x.BranchId == model.BranchId && x.CreatedOn.Value.Date == DateTime.Now.Date) + 1);
            return model.CreatedOn.ToString("yyMMdd") + nextOrderNo.ToString().PadLeft(4, '0');
        }
        private Tuple<DataTable, DataTable> ConvertOrderToDataTable(SalesOrderMasterDto model)
        {
            var itemsDataTable = _mapper.Map<IList<SalesOrderDetailsDto>, IList<OrderItemTVP>>(model.SalesOrderDetails).ToDataTable();
            var modifiersDataTable = _mapper.Map<IList<SalesOrderItemModifiersDto>, IList<OrderItemModifierTVP>>(model.SalesOrderDetails.SelectMany(x => x.SalesOrderItemModifiers).ToList()).ToDataTable();
            return new Tuple<DataTable, DataTable>(itemsDataTable, modifiersDataTable);
        }
        public async Task<SalesOrderMasterDto> Checkout(SalesOrderMasterDto orderMasterDto)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //20
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var data = await _dbContext.SalesOrderMaster.Include(x => x.DiningTable).Include(x => x.SalesOrderDetails).FirstOrDefaultAsync(x => x.Id == orderMasterDto.Id);
                    if (data is null) return null;
                    
                    if (data.DiningTable is not null) data.DiningTable.IsOccupied = false;

                    if (data.IsSelfDelivery) data.DeliveryBoyId = orderMasterDto.DeliveryBoyId;
                    if (data.OrderTypeId == OrderTypes.DineIn.ToInt())
                    {
                        data.DiscountAmount = orderMasterDto.DiscountAmount;
                        data.IsDiscountInPercent = orderMasterDto.IsDiscountInPercent;
                        if(orderMasterDto.TaxAmount.HasValue)
                        {
                            data.TaxAmount = orderMasterDto.TaxAmount;
                            data.IsTaxInPercent = orderMasterDto.IsTaxInPercent;
                        }
                    }
                    data.OrderStatusId = OrderStatus.Billed.ToInt();
                    var updatedModel = _mapper.Map<SalesOrderMasterDto>(data);
                    updatedModel.SalesOrderBilling = orderMasterDto.SalesOrderBilling;
                    //generate bill
                    var bill = new SalesOrderBillingDto(order: updatedModel)
                    {
                        Status = StatusTypes.Active.ToInt()
                    };
                    var billData = _mapper.Map<SalesOrderBilling>(bill);
                    await _dbContext.SalesOrderBilling.AddAsync(billData);
                    await SaveChangesAsync();

                    //acc transaction for sales.
                    if (orderMasterDto.Id.HasValue)
                    {
                        var parameters = new List<SqlParameter>
                                     { new("@OrderId", orderMasterDto.Id), new ("@IsEditMode", value: false) };
                        await _dbContext.Database.ExecuteSqlRawAsync(@"dbo.[Sales_AccountTransactionOnSale] @OrderId, @IsEditMode", parameters);
                    }
                    else
                    {
                        throw "SalesOrderBilling.Id is null.".ToException();
                    }

                    await transaction.CommitAsync();
                    return _mapper.Map<SalesOrderMasterDto>(await GetDetails(orderMasterDto));
                }
                catch(Exception )
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
        public async Task<IList<SalesOrderStatus_SLM>> GetOrderStatusSelectList(SalesOrderStatusDto filter)
        {
            var ordersStatusList = await _dbContext.SalesOrderStatus.AsNoTracking().Where(x => /*x.CompanyId == filter.CompanyId &&*/ x.Status == StatusTypes.Active.ToInt())
                .Select(x=> new SalesOrderStatus_SLM { Value = x.Id.ToString(), Text= x.Name })
                .ToListAsync();
            return ordersStatusList;
        }
        public async Task<bool> CancelOrder(SalesOrderMasterDto model)
        {
            var order = await _dbContext.SalesOrderMaster.FindAsync(model.Id);
            if (order is not null && model.Id.HasValue && model.ModifiedBy.HasValue)
            {
                await _dbContext.Sales_CancelOrder(model.Id.Value, model.ModifiedBy.Value);
                //await _itemRepository.UpdateItemsWithModifiersToCache(model.CompanyId);
            }
            return false;
        }
        public List<InventoryResponseMessageDto> ConvertDataTableToInventoryResponseMessageList(DataTable dt) {
            return (from DataRow dr in dt.Rows
             select new InventoryResponseMessageDto
             {
                 ItemId = Convert.ToInt32(dr["ItemId"]),
                 ItemName = Convert.ToString(dr["ItemName"]),
                 BarCodeId = Convert.ToInt32(dr["BarCodeId"]),
                 RequestedQuantity = Convert.ToSingle(dr["RequestedQuantity"])
             }).ToList();
        }
    }
}