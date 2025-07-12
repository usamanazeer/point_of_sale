using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using POS_API.Data;
using POS_API.Repositories.InventoryManagement.ItemRepos;

namespace POS_API.Repositories.InventoryManagement.BrandRepos
{
    public class BrandRepository : RepositoryBase, IBrandRepository, IRepository
    {
        private readonly IItemRepository _itemRepository;


        public BrandRepository(PosDB_Context dbContext,
                               IMapper mapper,IItemRepository itemRepository) : base(dbContext: dbContext,
                                                      mapper: mapper) =>
            _itemRepository = itemRepository;


        public async Task<InvBrandDto> Create(InvBrandDto model)
        {
            var data = _mapper.Map<InvBrand>(source: model);
            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.InvBrand.AddAsync(entity: data);
            await _dbContext.SaveChangesAsync();
            return model;
        }


        public async Task<bool> Delete(InvBrandDto model)
        {
            var brand = await _dbContext.InvBrand.FirstOrDefaultAsync(predicate: x =>
                                                                          x.Id == model.Id &&
                                                                          x.CompanyId == model.CompanyId &&
                                                                          x.Status != StatusTypes.Delete.ToInt());
            if (brand is null) return false;

            brand.ModifiedBy = model.ModifiedBy;
            brand.ModifiedOn = model.ModifiedOn;
            brand.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            ////start updation in cache
            //var cacheData = await _itemRepository.GetAllWithModifiersFromCache(new InvItemDto() { CompanyId = brand.CompanyId });
            //if (cacheData != null)
            //{
            //    var updatedCacheData = cacheData.Where(x => x.BrandId != brand.Id).ToList();
            //    await _itemRepository.UpdateItemsWithModifiersToCache(brand.CompanyId, updatedCacheData);
            //}
            ////end updation in cache
            return true;
        }


        public async Task<InvBrandDto> Edit(InvBrandDto model)
        {
            var data = await _dbContext.InvBrand.FindAsync(model.Id);
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
                invItems = (await _dbContext.InvItem.Where(x => x.BrandId == model.Id).ToListAsync()).Select(item =>
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
            //            cacheItem.BrandName = data.Name;
            //        }
            //    }
            //    await _itemRepository.UpdateItemsWithModifiersToCache(data.CompanyId, cacheData);
            //}
            ////end updation in cache
            return _mapper.Map<InvBrandDto>(source: data);
        }


        public async Task<List<InvBrandDto>> GetAll(InvBrandDto model)
        {
            var query = _dbContext.InvBrand.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId);
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
            return await query.Select(x=>new InvBrandDto { Id = x.Id, Name = x.Name, Status = x.Status }).ToListAsync();
        }


        public async Task<InvBrandDto> GetDetails(InvBrandDto model)
        {
            var brandData = await _dbContext.Set<InvBrand>().AsNoTracking()
                .Select(selector: brand =>
                    new
                    {
                        brand = new InvBrandDto { 
                            Id = brand.Id, Name = brand.Name, Status = brand.Status, CreatedBy = brand.CreatedBy, CreatedOn = brand.CreatedOn,
                            ModifiedBy = brand.ModifiedBy, ModifiedOn = brand.ModifiedOn, CompanyId =brand.CompanyId
                        },
                        createdBy = UserWithRoleSelect.FirstOrDefault(cu => brand.CreatedBy == cu.Id),
                        modifiedBy = UserWithRoleSelect.FirstOrDefault(bu => brand.ModifiedBy == bu.Id)
                    }).FirstOrDefaultAsync(b => b.brand.Id == model.Id && b.brand.CompanyId == model.CompanyId);
            var brandDto = brandData?.brand;
            if (brandDto != null)
            {
                brandDto.CreatedByUser = brandData.createdBy;
                brandDto.ModifiedByUser = brandData.modifiedBy;
                
            }
            return brandDto;
        }

        public async Task<bool> IsExist(InvBrandDto model)
        {
            return await _dbContext.InvBrand.AsNoTracking()
                .AnyAsync(predicate: x => x.Status != StatusTypes.Delete.ToInt() && x.Name == model.Name && x.CompanyId == model.CompanyId && x.Id != model.Id);
        }
    }
}