using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.UserManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Models.Enums;
using POS_API.Data;

namespace POS_API.Repositories.InventoryManagement.ItemBarCodeRepos
{
    public class ItemBarCodeRepository : RepositoryBase, IItemBarCodeRepository, IRepository
    {
        public ItemBarCodeRepository(PosDB_Context dbContext,
                                     IMapper mapper) : base(dbContext: dbContext,
                                                            mapper: mapper)
        {}


        public async Task<InvItemBarCodeDto> Create(InvItemBarCodeDto model)
        {
            var data = _mapper.Map<InvItemBarCode>(source: model);
            data.BarCode = data.BarCode.Trim();
            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.InvItemBarCode.AddAsync(entity: data);
            await _dbContext.SaveChangesAsync();
            return model;
        }


        public async Task<bool> Delete(InvItemBarCodeDto model)
        {
            var vendor = await _dbContext.InvItemBarCode.FirstOrDefaultAsync(predicate: x =>
                                                                          x.Id == model.Id && x.CompanyId == model.CompanyId &&
                                                                          x.Status != StatusTypes.Delete.ToInt());
            if (vendor is null) return false;

            vendor.ModifiedBy = model.ModifiedBy;
            vendor.ModifiedOn = model.ModifiedOn;
            vendor.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;


        }


        public async Task<InvItemBarCodeDto> Edit(InvItemBarCodeDto model)
        {
            var data = await _dbContext.InvItemBarCode.FindAsync(model.Id);
            if (data is null) return null;

            data.BarCode = model.BarCode;
            data.ItemId = model.ItemId;
            data.CompanyId = model.CompanyId;
            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<InvItemBarCodeDto>(source: data);
        }


        public async Task<List<InvItemBarCodeDto>> GetAll(InvItemBarCodeDto model)
        {
            var query = _dbContext.InvItemBarCode.AsNoTracking().Include(navigationPropertyPath: x => x.Item)
                                  .Where(predicate: c => c.CompanyId == model.CompanyId);
            if (model != null)
            {
                //if getting by id, don't apply filters.
                if (model.Id.HasValue)
                {
                    query = query.Where(predicate: r => r.Id == model.Id);
                }
                else
                {
                    if (model.ItemId.HasValue) query = query.Where(predicate: c => c.ItemId == model.ItemId);
                    if (!model.DisplayDeleted) query = query.Where(predicate: c => c.Status != StatusTypes.Delete.ToInt());
                    if (model.Status.HasValue) query = query.Where(predicate: r => r.Status == model.Status);
                    if (model.Item != null)
                    {
                        if (model.Item.ExceptDealItems)
                            query = query.Where(predicate: x => x.Item.ItemType != ItemTypes.DealItem.ToInt());
                        if (model.Item.ExceptRawItems)
                            query = query.Where(predicate: x => x.Item.ItemType != ItemTypes.RawItem.ToInt());
                        if (model.Item.ExceptRecipeItems)
                            query = query.Where(predicate: x => x.Item.ItemType != ItemTypes.RecipeItem.ToInt());
                        if (model.Item.Status.HasValue)
                            query = query.Where(predicate: x => x.Item.Status == model.Item.Status);
                    }
                }
            }
            query = query.OrderBy(keySelector: x => x.ItemId).ThenBy(keySelector: x => x.CreatedOn);
            var data = await query.Where(predicate: x => x.Item.Status == StatusTypes.Active.ToInt())
                .Select(x=> new InvItemBarCodeDto { 
                    Id = x.Id, BarCode = x.BarCode, Status = x.Status,ItemId = x.ItemId,
                    Item = x.Item != null ? new InvItemDto { 
                        Id = x.Item.Id, Name = x.Item.Name, FullName = x.Item.FullName, Status = x.Item.Status
                    }:null
                }).ToListAsync();
            return data;
        }


        public async Task<InvItemBarCodeDto> GetDetails(InvItemBarCodeDto model)
        {
            var data = await _dbContext.InvItemBarCode.AsNoTracking().Include(navigationPropertyPath: x => x.Item)
                .Select(selector: barCode =>
                    new
                    {
                        barCode = new InvItemBarCodeDto {
                            Id = barCode.Id, BarCode = barCode.BarCode, Status = barCode.Status, ItemId = barCode.ItemId, 
                            CreatedOn = barCode.CreatedOn, ModifiedOn = barCode.ModifiedOn, CompanyId = barCode.CompanyId,
                            Item = barCode.Item != null ? new InvItemDto
                            { Id = barCode.Item.Id, Name = barCode.Item.Name, FullName = barCode.Item.FullName, Status = barCode.Item.Status } : null
                        },
                        CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => barCode.CreatedBy == cu.Id),
                        ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => barCode.ModifiedBy == bu.Id)
                    }).FirstOrDefaultAsync(predicate: b => b.barCode.Id == model.Id && b.barCode.CompanyId == model.CompanyId);
            if (data is null) return null;
            data.barCode.CreatedByUser = data.CreatedByUser;
            data.barCode.ModifiedByUser = data.ModifiedByUser;
            return data.barCode;
        }


        public async Task<IList<InvItemBarCode_SLM>> GetSelectList(InvItemBarCodeDto model)
        {
            var query = _dbContext.InvItemBarCode.AsNoTracking().Include(navigationPropertyPath: x => x.Item)
                                  .Where(predicate: c => c.CompanyId == model.CompanyId);
            if (model != null)
            {
                if (model.ItemId.HasValue) query = query.Where(predicate: c => c.ItemId == model.ItemId);
                if (!model.DisplayDeleted) query = query.Where(predicate: c => c.Status != StatusTypes.Delete.ToInt());

                if (model.Status.HasValue) query = query.Where(predicate: r => r.Status == model.Status);

                if (model.Item != null)
                {
                    if (model.Item.ExceptDealItems)
                        query = query.Where(predicate: x => x.Item.ItemType != ItemTypes.DealItem.ToInt());
                    if (model.Item.ExceptRawItems)
                        query = query.Where(predicate: x => x.Item.ItemType != ItemTypes.RawItem.ToInt());
                    if (model.Item.ExceptRecipeItems)
                        query = query.Where(predicate: x => x.Item.ItemType != ItemTypes.RecipeItem.ToInt());
                }
            }

            query = query.OrderBy(keySelector: x => x.ItemId).ThenBy(keySelector: x => x.CreatedOn);
            return await query
                .Where(predicate: x => x.Item.Status == StatusTypes.Active.ToInt())
                .Select(x=>new InvItemBarCode_SLM
                { 
                    Value = x.Id.String(),
                    Text = x.BarCode,
                    ItemId = x.ItemId.Value,
                }).ToListAsync();
        }


        public async Task<bool> IsExist(InvItemBarCodeDto model)
        {
            return await _dbContext.InvItemBarCode.AsNoTracking()
                .AnyAsync(predicate: x =>
                    x.BarCode == model.BarCode.Trim() &&
                    x.CompanyId == model.CompanyId &&
                    x.Status != StatusTypes.Delete.ToInt() &&
                    x.Id != model.Id);
        }
    }
}