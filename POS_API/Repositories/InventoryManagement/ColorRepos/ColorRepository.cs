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

namespace POS_API.Repositories.InventoryManagement.ColorRepos
{
    public class ColorRepository:RepositoryBase, IColorRepository, IRepository
    {
        private readonly IItemRepository _itemRepository;
        public ColorRepository(PosDB_Context dbContext, IMapper mapper,
                               IItemRepository itemRepository) : base(dbContext, mapper) =>
            _itemRepository = itemRepository;


        public async Task<InvColorDto> Create(InvColorDto model)
        {
            var data = _mapper.Map<InvColor>(model);

            data.Status = StatusType.Active.ToInt();
            await _dbContext.InvColor.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(InvColorDto model)
        {
            var color = await _dbContext.InvColor.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusType.Delete.ToInt());
            if (color is null) return false;

            color.ModifiedBy = model.ModifiedBy;
            color.ModifiedOn = model.ModifiedOn;
            color.Status = StatusType.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            ////start updation in cache
            //var cacheData = await _itemRepository.GetAllWithModifiersFromCache(new InvItemDto() { CompanyId = color.CompanyId });
            //if (cacheData != null)
            //{
            //    var updatedCacheData = cacheData.Where(x => x.ColorId != color.Id).ToList();
            //    await _itemRepository.UpdateItemsWithModifiersToCache(color.CompanyId, updatedCacheData);
            //}
            ////end updation in cache
            return true;
        }

        public async Task<InvColorDto> Edit(InvColorDto model)
        {
            var data = await _dbContext.InvColor.FindAsync(model.Id);
            if (data is null) return null;

            var nameUpdatedFlag = data.Name != model.Name;
            var oldName = data.Name;

            data.Name = model.Name;
            data.CompanyId = model.CompanyId;
            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            IList<InvItem> invItems = null;
            if (nameUpdatedFlag)
            {
                invItems = (await _dbContext.InvItem.Where(x => x.ColorId == model.Id).ToListAsync()).Select(item =>
                {
                    item.FullName = item.FullName.Replace($"/{oldName}", $"/{data.Name}");
                    return item;
                }).ToList();
            }
            await _dbContext.SaveChangesAsync();

            ////start updation in cache
            //var cacheData = await _itemRepository.GetAllWithModifiersFromCache(new InvItemDto() { CompanyId = data.CompanyId });

            //if (nameUpdatedFlag && cacheData != null)
            //{
            //    foreach (var updatedItem in invItems)
            //    {
            //        var cacheItem = cacheData.FirstOrDefault(x => x.Id == updatedItem.Id);
            //        if (cacheItem != null)
            //        {
            //            cacheItem.FullName = updatedItem.FullName;
            //            cacheItem.ColorName = data.Name;
            //        }
            //    }
            //    await _itemRepository.UpdateItemsWithModifiersToCache(data.CompanyId, cacheData);
            //}
            ////end updation in cache
            return _mapper.Map<InvColorDto>(data);
        }

        public async Task<List<InvColorDto>> GetAll(InvColorDto model)
        {
            var query = _dbContext.InvColor.AsNoTracking().Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted)
                    query = query.Where(c => c.Status != StatusType.Delete.ToInt());

                if (model.Status.HasValue)
                    query = query.Where(r => r.Status == model.Status);
            }
            return await query .Select(x=> new InvColorDto{ Id = x.Id, Name = x.Name, Status = x.Status, ColorValue = x.ColorValue }).ToListAsync();
        }

        public async Task<InvColorDto> GetDetails(InvColorDto model)
        {
            var data = await _dbContext.InvColor.AsNoTracking()
                .Select(color =>
                    new
                    {
                        Color = new InvColorDto
                        {
                            Id = color.Id, Name = color.Name, Status = color.Status, ColorValue = color.ColorValue, CreatedBy = color.CreatedBy,
                            CreatedOn = color.CreatedOn, ModifiedBy = color.ModifiedBy, ModifiedOn = color.ModifiedOn, CompanyId = color.CompanyId
                        },
                        CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => color.CreatedBy == cu.Id),
                        ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => color.ModifiedBy == bu.Id)
                    }).FirstOrDefaultAsync(c => c.Color.Id == model.Id && c.Color.CompanyId == model.CompanyId);

            if (data is null) return null;
            data.Color.CreatedByUser = data.CreatedByUser;
            data.Color.ModifiedByUser = data.ModifiedByUser;
            return data.Color;
        }

        public async Task<bool> IsExist(InvColorDto model)
        {
            return await _dbContext.InvColor.AsNoTracking()
               .AnyAsync(x => x.Status != StatusType.Delete.ToInt() &&  x.Name == model.Name && x.CompanyId == model.CompanyId &&  x.Id != model.Id);
        }
    }
}
