using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using Models.Enums;
using POS_API.Data;

namespace POS_API.Repositories.InventoryManagement.PhysicalInventoryRepos
{
    public class PhysicalInventoryRepository : RepositoryBase, IPhysicalInventoryRepository, IRepository
    {
        public PhysicalInventoryRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext: dbContext,mapper: mapper)
        {}


        public async Task<InvPhysicalInventoryDto> AddPhysicalInventory(InvPhysicalInventoryDto model)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //14
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var data = _mapper.Map<InvPhysicalInventory>(source: model);
                    foreach (var item in data.InvPhysicalInventoryItem)
                    {
                        item.RemainingQuantity = item.Quantity;
                        item.Status = StatusTypes.Active.ToInt();
                        item.CreatedBy = data.CreatedBy;
                        item.CreatedOn = data.CreatedOn;
                        item.CompanyId = data.CompanyId;
                    }

                    //save main entry
                    data.Status = StatusTypes.Active.ToInt();
                    await _dbContext.InvPhysicalInventory.AddAsync(entity: data);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return _mapper.Map<InvPhysicalInventoryDto>(source: data);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
            
        }


        public async Task<List<InvPhysicalInventoryDto>> GetAll(InvPhysicalInventoryDto model)
        {
            var query = _dbContext.InvPhysicalInventory.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(predicate: r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted) query = query.Where(predicate: c => c.Status != StatusTypes.Delete.ToInt());
                if (model.Status.HasValue) query = query.Where(predicate: r => r.Status == model.Status);
            }

            var data = await query
                .Select(x=> new InvPhysicalInventoryDto{ 
                    Id = x.Id, BillDate = x.BillDate, BillNo = x.BillNo, Status = x.Status,
                    CreatedOn = x.CreatedOn, ModifiedBy = x.ModifiedBy}).ToListAsync();
            return data;
        }


        public async Task<List<InvPhysicalInventoryViewDto>> GetLowInventory(PhysicalInventoryViewFilter filters = null)
        {
            var query = from record in _dbContext.InvPhysicalInventoryView.AsNoTracking()
                group record by new { record.ItemId } into g
                select new InvPhysicalInventoryViewDto { ItemId = g.Key.ItemId,RemainingQuantity = g.Sum(x => x.RemainingQuantity) };
            query = query.Where(predicate: inv =>
                                    inv.RemainingQuantity < _dbContext
                                                            .InvItem.FirstOrDefault(x => x.Id == inv.ItemId)
                                                            .MinimumQuantity);
            if (filters?.ItemIds is not null)
                if (filters.ItemIds.Any())
                    query = query.Where(predicate: x => filters.ItemIds.Contains(x.ItemId));

            var data = _mapper.Map<List<InvPhysicalInventoryViewDto>>(source: await query.ToListAsync());
            return data;
        }


        //public async Task<PhysicalInventoryViewDTO> GetDetails(PhysicalInventoryViewFilter filter)
        //{
        //    var data = (await _dbContext.InvPhysicalInventory.Include(x => x.InvPhysicalInventoryItem)
        //        .Where(b => b.Id == filter.Id && b.CompanyId == filter.CompanyId).ToListAsync());
        //        //.Select(inventory =>
        //        //    new
        //        //    {
        //        //        inventory,
        //        //        cu = _dbContext.Set<User>().Include(r => r.Role).Where(cu => inventory.CreatedBy == cu.Id),
        //        //        bu = _dbContext.Set<User>().Include(r => r.Role).Where(bu => inventory.ModifiedBy == bu.Id)
        //        //    })
        //        //.Select(r => new InvPhysicalInventoryDTO
        //        //{
        //        //    Id = r.inventory.Id,
        //        //    BillNo = r.inventory.BillNo,
        //        //    BillDate = r.inventory.BillDate,
        //        //    Status = r.inventory.Status,
        //        //    CreatedOn = r.inventory.CreatedOn,
        //        //    ModifiedOn = r.inventory.ModifiedOn,
        //        //    InvPhysicalInventoryItem = _mapper.Map<IList<InvPhysicalInventoryItemDTO>>(r.inventory.InvPhysicalInventoryItem),
        //        //    CreatedByUser = _mapper.Map<UserDTO>(r.cu.FirstOrDefault()),
        //        //    ModifiedByUser = _mapper.Map<UserDTO>(r.bu.FirstOrDefault())
        //        //})
        //        //.ToListAsync()).FirstOrDefault();

        //    return _mapper.Map<PhysicalInventoryViewDTO>(data);
        //}


        public async Task<List<InvPhysicalInventoryViewDto>> GetPhysicalInventory_View(PhysicalInventoryViewFilter filters = null)
        {
            var query = _dbContext.InvPhysicalInventoryView.AsNoTracking().Where(predicate: c => c.CompanyId == filters.CompanyId);
            //if getting by id, don't apply filters.
            if (filters?.Id is not null)
            {
                query = query.Where(predicate: r => r.BillId == filters.Id);
            }
            else
            {
                if (filters?.BillIds is not null && filters.BillIds.Any()) query = query.Where(predicate: r => filters.BillIds.Contains(r.BillId));
                if (filters?.ItemIds is not null && filters.ItemIds.Any()) query = query.Where(predicate: r => filters.ItemIds.Contains(r.ItemId));
                if (filters?.ItemBarCodeIds is not null && filters.ItemBarCodeIds.Any()) query = query.Where(predicate: r => filters.ItemBarCodeIds.Contains(r.ItemBarCodeId ?? 0));
                if (filters?.VendorIds is not null && filters.VendorIds.Any()) query = query.Where(predicate: r => filters.VendorIds.Contains(r.VendorId ?? 0));
                if (filters?.ExpiryDate is not null)  query = query.Where(predicate: r => r.ExpiryDate <= filters.ExpiryDate);
                if (filters?.BillDateStart is not null && filters.BillDateEnd is not null) query = query.Where(predicate: r => r.BillDate == filters.BillDateStart);
                else if (filters?.BillDateStart is not null && filters.BillDateEnd is not null) query = query.Where(predicate: r => r.BillDate >= filters.BillDateStart && r.BillDate <= filters.BillDateEnd);
                //only if remaining quantity is > 0
                if (filters is {OnlyIfRemaining: true}) query = query.Where(predicate: r => r.RemainingQuantity > 0);
                if (filters?.CategoryId is not null) query = query.Where(predicate: r => r.CategoryId == filters.CategoryId);
                if (filters?.SubCategoryId is not null) query = query.Where(predicate: r => r.SubCategoryId == filters.SubCategoryId);
                if (filters?.ItemTypes is not null) query = query.Where(predicate: r => filters.ItemTypes.Contains( r.ItemType.Value));
                if (filters?.SearchText is not null) query = query.Where(predicate: r => r.ItemName.ToLower().Contains(filters.SearchText.ToLower()));
            }

            var data = _mapper.Map<List<InvPhysicalInventoryViewDto>>(source: await query.ToListAsync());
            return data;
        }


        public async Task<bool> IsPhysicalInventoryExists(InvPhysicalInventoryDto model)
        {
            return await _dbContext.InvPhysicalInventory.AsNoTracking()
                .AnyAsync(predicate: x => x.Status != StatusTypes.Delete.ToInt() &&
                    x.BillNo == model.BillNo && x.CompanyId == model.CompanyId && x.Id != model.Id);
        }
    }
}