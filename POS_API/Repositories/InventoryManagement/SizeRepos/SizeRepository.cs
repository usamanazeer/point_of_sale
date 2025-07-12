using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Data;
using POS_API.Repositories.InventoryManagement.ItemRepos;
using StatusType = Models.Enums.StatusTypes;

namespace POS_API.Repositories.InventoryManagement.SizeRepos
{
    public class SizeRepository : RepositoryBase, ISizeRepository, IRepository
    {
        private readonly IItemRepository _itemRepository;
        public SizeRepository(PosDB_Context dbContext, IMapper mapper, IItemRepository itemRepository) 
            : base(dbContext: dbContext, mapper: mapper) =>
            _itemRepository = itemRepository;

        public async Task<InvSizeDto> Create(InvSizeDto model)
        {
            var data = _mapper.Map<InvSize>(source: model);
            data.Status = StatusType.Active.ToInt();
            await _dbContext.InvSize.AddAsync(entity: data);
            await _dbContext.SaveChangesAsync();
            return model;
        }


        public async Task<bool> Delete(InvSizeDto model)
        {
            var size = await _dbContext.InvSize.FirstOrDefaultAsync(predicate: x =>
                x.Id == model.Id && x.CompanyId == model.CompanyId &&
                x.Status != StatusType.Delete.ToInt());
            if (size is null) return false;
            size.ModifiedBy = model.ModifiedBy;
            size.ModifiedOn = model.ModifiedOn;
            size.Status = StatusType.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<InvSizeDto> Edit(InvSizeDto model)
        {
            var data = await _dbContext.InvSize.FindAsync(model.Id);
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
                invItems = (await _dbContext.InvItem.Where(x => x.SizeId == model.Id).ToListAsync()).Select(item =>
                {
                    item.FullName = item.FullName.Replace($"/{oldName}", $"/{data.Name}");
                    return item;
                }).ToList();
            }
            await _dbContext.SaveChangesAsync();
            ////start updation in cache
            //var cacheData = await _itemRepository.GetAllWithModifiersFromCache(new InvItemDto{ CompanyId = data.CompanyId });

            //if (nameUpdatedFlag && cacheData is not null)
            //{
            //    foreach (var updatedItem in invItems)
            //    {
            //        var cacheItem = cacheData.FirstOrDefault(x => x.Id == updatedItem.Id);
            //        if (cacheItem is not null)
            //        {
            //            cacheItem.FullName = updatedItem.FullName;
            //            cacheItem.SizeName = data.Name;
            //        }
            //    }
            //    await _itemRepository.UpdateItemsWithModifiersToCache(data.CompanyId, cacheData);
            //}
            ////end updation in cache
            return _mapper.Map<InvSizeDto>(source: data);
        }


        public async Task<List<InvSizeDto>> GetAll(InvSizeDto model)
        {
            var query = _dbContext.InvSize.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(predicate: r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted) query = query.Where(predicate: c => c.Status != StatusType.Delete.ToInt());
                if (model.Status.HasValue) query = query.Where(predicate: r => r.Status == model.Status);
            }

            return await query
            .Select(size => new InvSizeDto {Id = size.Id, Name = size.Name, Status = size.Status, CreatedOn = size.CreatedOn, ModifiedOn = size.ModifiedOn})
            .ToListAsync();
        }


        public async Task<InvSizeDto> GetDetails(InvSizeDto model)
        {
            return (await _dbContext.InvSize.AsNoTracking().Select(selector: size =>
                new
                {
                    Size = new InvSizeDto {
                        Id = size.Id, Name = size.Name, Status = size.Status, CreatedOn = size.CreatedOn, ModifiedOn = size.ModifiedOn, CompanyId = size.CompanyId,
                        CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => size.CreatedBy == cu.Id),
                        ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => size.ModifiedBy == bu.Id)
                    }
                }).FirstOrDefaultAsync(predicate: b => b.Size.Id == model.Id && b.Size.CompanyId == model.CompanyId))?.Size;
        }


        public async Task<bool> IsExist(InvSizeDto model)
        {   
            return await _dbContext.InvSize.AsNoTracking().AnyAsync(predicate: x => x.Status != StatusType.Delete.ToInt() &&
                x.Name == model.Name && x.CompanyId == model.CompanyId && x.Id != model.Id);
        }
    }
}