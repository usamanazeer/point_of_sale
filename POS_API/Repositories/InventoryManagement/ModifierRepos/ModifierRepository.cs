using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Models.Enums;
using POS_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using POS_API.Repositories.InventoryManagement.ItemRepos;

namespace POS_API.Repositories.InventoryManagement.ModifierRepos
{
    public class ModifierRepository : RepositoryBase, IModifierRepository, IRepository
    {
        private readonly IItemRepository _itemRepository;
        public ModifierRepository(PosDB_Context dbContext, IMapper mapper,
                                  IItemRepository itemRepository) : base(dbContext, mapper) =>
            _itemRepository = itemRepository;


        public async Task<InvModifierDto> Create(InvModifierDto model)
        {
            var data = _mapper.Map<InvModifier>(model);
            foreach (var item in data.InvModifierItems)
            {
                item.CompanyId = data.CompanyId;
                item.CreatedBy = data.CreatedBy;
                item.CreatedOn = data.CreatedOn;
            }
            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.InvModifier.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return model;
        }
        public async Task<bool> Delete(InvModifierDto model)
        {
            var modifier = await _dbContext.InvModifier.Where(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt()).FirstOrDefaultAsync();
            if (modifier == null) return false;

            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<InvModifierDto> Edit(InvModifierDto model)
        {
            var data = await _dbContext.InvModifier.Include(x=>x.InvModifierItems).FirstOrDefaultAsync(x => x.Id == model.Id);
            if (data is null) return null;

            data.Name = model.Name;
            if (model.Status.HasValue) data.Status =  model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;

            foreach (var itemOld in data.InvModifierItems)
            {
                itemOld.Status = StatusTypes.Delete.ToInt();
                var newItem = model.InvModifierItems.FirstOrDefault(x => x.ItemId == itemOld.ItemId);
                if (newItem != null)
                {
                    itemOld.Quantity = newItem.Quantity;
                    itemOld.BarCodeId = newItem.BarCodeId;
                    itemOld.CompanyId = data.CompanyId;
                    itemOld.Status = StatusTypes.Active.ToInt();
                    itemOld.ModifiedBy = data.ModifiedBy;
                    itemOld.ModifiedOn = data.ModifiedOn;
                    model.InvModifierItems.Remove(newItem);
                }
            }
            var subItems = Map<List<InvModifierItems>>(model.InvModifierItems);
            foreach (var newItem in subItems)
            {
                newItem.CompanyId = data.CompanyId;
                newItem.Status = StatusTypes.Active.ToInt();
                newItem.CreatedBy = data.ModifiedBy;
                newItem.CreatedOn = data.ModifiedOn;

                data.InvModifierItems.Add(newItem);
            }
            await _dbContext.SaveChangesAsync();
            ////update itemsDataInCache
            //await _itemRepository.UpdateItemsWithModifiersToCache(data.CompanyId);
            
            return _mapper.Map<InvModifierDto>(await GetDetails(model));
        }

        public async Task<List<InvModifierDto>> GetAll(InvModifierDto model)
        {
            var query = _dbContext.InvModifier.AsNoTracking().Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted)
                    query = query.Where(c => c.Status != StatusTypes.Delete.ToInt());

                if (model.Status.HasValue)
                    query = query.Where(r => r.Status == model.Status);
            }
            return await query
                .Select(x=> new InvModifierDto {
                    Id = x.Id, Name = x.Name, ModifierCharges = x.ModifierCharges, Status = x.Status
                })
                .ToListAsync();
        }

        public async Task<InvModifierDto> GetDetails(InvModifierDto model)
        {
            var data = await _dbContext.InvModifier.AsNoTracking().Include(x => x.InvModifierItems).ThenInclude(x => x.Item).ThenInclude(x=>x.Unit)
                .Select(modifier =>
                    new
                    {
                        modifier = new InvModifierDto
                        { 
                            Id = modifier.Id, Name = modifier.Name, ModifierCharges = modifier.ModifierCharges, CreatedOn = modifier.CreatedOn, ModifiedOn = modifier.ModifiedOn,
                            Status = modifier.Status, CompanyId = modifier.CompanyId
                        },
                        modifierItems = modifier.InvModifierItems.Select(mi => new InvModifierItemDto
                        {
                            Id = mi.Id,
                            ModifierId = mi.ModifierId,
                            ItemId = mi.ItemId,
                            BarCodeId = mi.BarCodeId,
                            Quantity = mi.Quantity,
                            Status = mi.Status,
                            Item = new InvItemDto
                            {
                                Id = mi.Item.Id,
                                ItemCode = mi.Item.ItemCode,
                                Name = mi.Item.Name,
                                FullName = mi.Item.FullName,
                                PurchaseRate = mi.Item.PurchaseRate,
                                SalesRate = mi.Item.SalesRate,
                                FinalSalesRate = mi.Item.FinalSalesRate,
                                Unit = new InvUnitDto
                                {
                                    Id = mi.Item.Unit.Id,
                                    Name = mi.Item.Unit.Name,
                                }
                            }
                        }).ToList(),
                        CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => modifier.CreatedBy == cu.Id),
                        ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => modifier.ModifiedBy == bu.Id)
                    }).FirstOrDefaultAsync(x => x.modifier.Id == model.Id && x.modifier.CompanyId == model.CompanyId && x.modifier.Status != StatusTypes.Delete.ToInt());

            if (data?.modifier is null) return null;

            data.modifier.InvModifierItems = data.modifierItems;
            data.modifier.CreatedByUser = data.CreatedByUser;
            data.modifier.ModifiedByUser = data.ModifiedByUser;
            return data.modifier;
        }

        public async Task<bool> IsExist(InvModifierDto model)
        {
            return await _dbContext.InvModifier.AsNoTracking()
               .AnyAsync(x => x.Status != StatusTypes.Delete.ToInt() &&
                    x.Name == model.Name &&
                    x.CompanyId == model.CompanyId &&
                    x.Id != model.Id);
        }
        public async Task<IList<InvModifier_SLM>> GetSelectList(InvModifierDto model)
        {
            return await _dbContext.InvModifier.AsNoTracking().Where(x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .Select(x=> new InvModifier_SLM { 
                    Value = x.Id.ToString(), Text = x.Name, ModifierCharges = x.ModifierCharges
                }).ToListAsync();
        }
    }
}
