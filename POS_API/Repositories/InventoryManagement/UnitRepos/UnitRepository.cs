using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTO.InventoryManagement;
using POS_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using POS_API.Repositories.InventoryManagement.ItemRepos;
using StatusType = Models.Enums.StatusTypes;

namespace POS_API.Repositories.InventoryManagement.UnitRepos
{
    public class UnitRepository : RepositoryBase, IUnitRepository, IRepository
    {
        private readonly IItemRepository _itemRepository;
        public UnitRepository(PosDB_Context dbContext, IMapper mapper,IItemRepository itemRepository) : base(dbContext, mapper) => _itemRepository = itemRepository;


        public async Task<InvUnitDto> Create(InvUnitDto model)
        {
            var data = _mapper.Map<InvUnit>(model);

            data.Status = StatusType.Active.ToInt();
            await _dbContext.InvUnit.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(InvUnitDto model)
        {
            var unit = await _dbContext.InvUnit.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusType.Delete.ToInt());
            if (unit is null) return false;

            unit.ModifiedBy = model.ModifiedBy;
            unit.ModifiedOn = model.ModifiedOn;
            unit.Status = StatusType.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<InvUnitDto> Edit(InvUnitDto model)
        {
            var data = await _dbContext.InvUnit.FindAsync(model.Id);
            if (data is null) return null;

            var nameUpdatedFlag = data.Name != model.Name;
            var oldName = data.Name;
            data.Name = model.Name;
            data.Description = model.Description;
            data.CompanyId = model.CompanyId;
            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;

            IList<InvItem> invItems = null;
            if (nameUpdatedFlag)
            {
                invItems = (await _dbContext.InvItem.Where(x => x.UnitId == model.Id).ToListAsync()).Select(item =>
                {
                    item.FullName = item.FullName.Replace($"/{oldName}", $"/{data.Name}");
                    return item;
                }).ToList();
            }

            await _dbContext.SaveChangesAsync();
            ////start updation in cache
            //var cacheData = await _itemRepository.GetAllWithModifiersFromCache(new InvItemDto { CompanyId = data.CompanyId });

            //if (nameUpdatedFlag && cacheData is not null)
            //{
            //    foreach (var updatedItem in invItems)
            //    {
            //        var cacheItem = cacheData.FirstOrDefault(x => x.Id == updatedItem.Id);
            //        if (cacheItem is not null) cacheItem.UnitName = data.Name;
            //    }
            //    await _itemRepository.UpdateItemsWithModifiersToCache(data.CompanyId, cacheData);
            //}
            ////end updation in cache
            return _mapper.Map<InvUnitDto>(data);
        }

        public async Task<List<InvUnitDto>> GetAll(InvUnitDto model)
        {
            var query = _dbContext.InvUnit.AsNoTracking().Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
                query = query.Where(r => r.Id == model.Id);
            else
            {
                if (!model.DisplayDeleted)
                    query = query.Where(c => c.Status != StatusType.Delete.ToInt());

                if (model.Status.HasValue)
                    query = query.Where(r => r.Status == model.Status);
            }
            return _mapper.Map<List<InvUnitDto>>(await query
                .Select(unit=> new InvUnitDto { 
                    Id= unit.Id, Name = unit.Name, Description = unit.Description, Status = unit.Status, CreatedOn = unit.CreatedOn, ModifiedOn = unit.ModifiedOn
                })
                .ToListAsync());
        }

        public async Task<InvUnitDto> GetDetails(InvUnitDto model)
        {
            return (await _dbContext.InvUnit.AsNoTracking()
                .Select(unit =>
                    new
                    {
                        Unit = new InvUnitDto { Id = unit.Id, Name = unit.Name, Description = unit.Description, Status = unit.Status, 
                            CreatedOn = unit.CreatedOn, ModifiedOn = unit.ModifiedOn, CompanyId = unit.CompanyId,
                        CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => unit.CreatedBy == cu.Id),
                        ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => unit.ModifiedBy == bu.Id)
                        }
                    }).FirstOrDefaultAsync(u => u.Unit.Id == model.Id && u.Unit.CompanyId == model.CompanyId))?.Unit;
        }

        public async Task<bool> IsExist(InvUnitDto model)
        {
            var res =  await _dbContext.InvUnit.AsNoTracking()
                                         .AnyAsync(x => (x.Status != StatusType.Delete.ToInt() && x.Name == model.Name && x.CompanyId == model.CompanyId && x.Id != model.Id));
            return res;
        }
    }
}
