using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTO.InventoryManagement;
using POS_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Models;
using POS_API.Repositories.MemoryCache;
using POS_API.Utilities;
using StatusType = Models.Enums.StatusTypes;

namespace POS_API.Repositories.InventoryManagement.CategoryRepos
{
    public class MainCategoryRepository : RepositoryBase, IMainCategoryRepository, IRepository
    {
        private string GetCategoriesCacheKey(int companyId) => "Categories" + companyId;
        private readonly IMemoryCache _memoryCache;
        private readonly IMemoryCacheUtil _memoryCacheUtil;
        public MainCategoryRepository(PosDB_Context dbContext,
                                      IMapper mapper,
                                      IMemoryCache memoryCache,
                                      IMemoryCacheUtil memoryCacheUtil) : base(dbContext,
                                                                       mapper)
        {
            _memoryCache = memoryCache;
            _memoryCacheUtil = memoryCacheUtil;
        }
        public async Task<InvCategoryDto> Create(InvCategoryDto model)
        {
            var data = _mapper.Map<InvCategory>(model);
            var next = (await _dbContext.InvCategory.CountAsync(x => x.CompanyId == data.CompanyId) + 1);
            // ReSharper disable once StringLiteralTypo
            data.CategoryCode = "MCAT-" + next.ToString().PadLeft(4, '0');
            data.Status = StatusType.Active.ToInt();
            await _dbContext.InvCategory.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            await _memoryCacheUtil.UpdateCache_Category(model.CompanyId);
            return _mapper.Map<InvCategoryDto>(data);
        }

        public async Task<bool> Delete(InvCategoryDto model)
        {
            var mainCategory = await _dbContext.InvCategory.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusType.Delete.ToInt());
            if (mainCategory is null) return false;

            mainCategory.ModifiedBy = model.ModifiedBy;
            mainCategory.ModifiedOn = model.ModifiedOn;
            mainCategory.Status = StatusType.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            await _memoryCacheUtil.UpdateCache_Category(model.CompanyId);
            await _memoryCacheUtil.UpdateCache_Items(mainCategory.CompanyId);
            return true;

        }

        public async Task<InvCategoryDto> Edit(InvCategoryDto model)
        {
            var data = await _dbContext.InvCategory.FindAsync(model.Id);
            if (data is null) return null;

            data.Name = model.Name;
            if (model.ImageUrl != null) data.ImageUrl = model.ImageUrl;

            data.DisplayOnPos = model.DisplayOnPos;
            data.CompanyId = model.CompanyId;

            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            await _memoryCacheUtil.UpdateCache_Category(model.CompanyId);
            await _memoryCacheUtil.UpdateCache_Items(model.CompanyId);
            return _mapper.Map<InvCategoryDto>(data);
        }

        public async Task<List<InvCategoryDto>> GetAll(InvCategoryDto model)
        {
            var query = _dbContext.InvCategory.Where(c => c.CompanyId == model.CompanyId).AsNoTracking();

            var categoriesData =  (await _memoryCache.GetListFromCache(GetCategoriesCacheKey(model.CompanyId), query,10,5,CacheItemPriority.High)).AsQueryable();
            
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                categoriesData = categoriesData.Where(r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted)
                    categoriesData = categoriesData.Where(c => c.Status != StatusType.Delete.ToInt());
                if (model.DisplayOnPos)
                    categoriesData = categoriesData.Where(r => r.DisplayOnPos == model.DisplayOnPos);
                if (model.Status.HasValue)
                    categoriesData = categoriesData.Where(r => r.Status == model.Status);
            }
            return categoriesData
                .Select(x=>new InvCategoryDto { 
                    Id= x.Id, CategoryCode = x.CategoryCode, Name = x.Name,DisplayOnPos = x.DisplayOnPos??false, Status = x.Status, ImageUrl = x.ImageUrl,
                    CreatedBy = x.CreatedBy, CreatedOn = x.CreatedOn, ModifiedBy= x.ModifiedBy, ModifiedOn = x.ModifiedOn, 
                }).ToList();
        }
        public async Task<bool> IsExist(InvCategoryDto model)
        {
            return await _dbContext.InvCategory.AsNoTracking().AnyAsync(x =>
                x.Name == model.Name &&
                x.CompanyId == model.CompanyId &&
                x.Status != StatusType.Delete.ToInt() &&
                x.Id != model.Id);
        }
    }
}
