using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using POS_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using POS_API.Repositories.MemoryCache;
using POS_API.Utilities;
using StatusType = Models.Enums.StatusTypes;

namespace POS_API.Repositories.InventoryManagement.CategoryRepos
{
    public class SubCategoryRepository : RepositoryBase, ISubCategoryRepository, IRepository
    {
        private string GetSubCategoriesCacheKey(int companyId) => "subCategories" + companyId;
        private readonly IMemoryCache _memoryCache;
        private readonly IMemoryCacheUtil _memoryCacheUtil;
        public SubCategoryRepository(PosDB_Context dbContext,
                                     IMapper mapper,
                                     IMemoryCache memoryCache,
                                     IMemoryCacheUtil memoryCacheUtil) : base(dbContext,
                                                                      mapper)
        {
            
            _memoryCache = memoryCache;
            _memoryCacheUtil = memoryCacheUtil;
        }
        public async Task<InvSubCategoryDto> Create(InvSubCategoryDto model)
        {
            var data = _mapper.Map<InvSubCategory>(model);
            var next = (await _dbContext.InvSubCategory.CountAsync(x => x.CompanyId == data.CompanyId) + 1);
            data.CategoryCode = "SCAT-" + next.ToString().PadLeft(4, '0');
            data.Status = StatusType.Active.ToInt();
            await _dbContext.InvSubCategory.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            await _memoryCacheUtil.UpdateCache_SubCategory(model.CompanyId);
            return model;
        }

        public async Task<bool> Delete(InvSubCategoryDto model)
        {
            var subCategory = await _dbContext.InvSubCategory.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusType.Delete.ToInt());
            if (subCategory is null) return false;

            subCategory.ModifiedBy = model.ModifiedBy;
            subCategory.ModifiedOn = model.ModifiedOn;
            subCategory.Status = StatusType.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            await _memoryCacheUtil.UpdateCache_SubCategory(model.CompanyId);
            await _memoryCacheUtil.UpdateCache_Items(subCategory.CompanyId);
            return true;

        }

        public async Task<InvSubCategoryDto> Edit(InvSubCategoryDto model)
        {
            var data = await _dbContext.InvSubCategory.FindAsync(model.Id);
            if (data is null) return null;

            data.Name = model.Name;
            if (model.ImageUrl != null) data.ImageUrl = model.ImageUrl;
            data.DisplayOnPos = model.DisplayOnPos;
            data.CategoryId = model.CategoryId ?? 0;
            data.CompanyId = model.CompanyId;

            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            await _memoryCacheUtil.UpdateCache_SubCategory(model.CompanyId);
            await _memoryCacheUtil.UpdateCache_Items(model.CompanyId);
            return _mapper.Map<InvSubCategoryDto>(data);
        }

        public async Task<List<InvSubCategoryDto>> GetAll(InvSubCategoryDto model)
        {
            var query = _dbContext.InvSubCategory.AsNoTracking().Include(x=>x.Category).Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(r => r.Id == model.Id);
            }
            else
            {
                if (model.CategoryId.HasValue)
                    query = query.Where(c => c.CategoryId == model.CategoryId);
                if (!model.DisplayDeleted)
                    query = query.Where(c => c.Status != StatusType.Delete.ToInt());
                if (model.DisplayOnPos)
                    query = query.Where(r => r.DisplayOnPos == model.DisplayOnPos);
                if (model.Status.HasValue)
                    query = query.Where(r => r.Status == model.Status);
            }
            return _mapper.Map<List<InvSubCategoryDto>>(await query.Select(_subcategorySelector).ToListAsync());
        }

        public async Task<bool> IsExist(InvSubCategoryDto model)
        {
            var exist = await _dbContext.InvSubCategory.AsNoTracking()
               .AnyAsync(x =>
                                             x.Name == model.Name &&
                                             x.CategoryId == model.CategoryId &&
                                             x.CompanyId == model.CompanyId &&
                                             x.Status != StatusType.Delete.ToInt() &&
                                             x.Id != model.Id);

            return exist;
        }
        readonly System.Linq.Expressions.Expression<Func<InvSubCategory, InvSubCategory>> _subcategorySelector = x => new InvSubCategory
        {
            Id = x.Id,
            CategoryCode = x.CategoryCode,
            Name = x.Name,
            ImageUrl = x.ImageUrl,
            DisplayOnPos = x.DisplayOnPos ?? false,
            CategoryId = x.CategoryId,
            Status = x.Status,
            CompanyId = x.CompanyId,
            CreatedBy = x.CreatedBy,
            CreatedOn = x.CreatedOn,
            ModifiedBy = x.ModifiedBy,
            ModifiedOn = x.ModifiedOn,
            Category = x.Category != null ? new InvCategory
            {
                Id = x.Category.Id,
                Name = x.Category.Name,
                CategoryCode = x.Category.CategoryCode,
                Status = x.Status
            } : null
        };
        public async Task<IList<InvSubCategory_SLM>> GetSelectList(InvSubCategoryDto model, bool forPos = false)
        {
            var query = _dbContext.InvSubCategory.AsNoTracking().Include(x => x.Category)
                .Where(x => x.CompanyId == model.CompanyId && x.Status == StatusType.Active.ToInt())
                .Select(_subcategorySelector);
            var cacheKey = GetSubCategoriesCacheKey(model.CompanyId);
            var subCategories = await _memoryCache.GetListFromCache(cacheKey, query, 10, 5, CacheItemPriority.High);
            if (forPos)
            {
                subCategories = subCategories.Where(x => x.DisplayOnPos == true && x.Category.DisplayOnPos == true).ToList();
            }
            else
            {
                if (model.DisplayOnPos)
                    subCategories = subCategories.Where(x => x.DisplayOnPos == model.DisplayOnPos).ToList();
            }

            if (model.CategoryId.HasValue)
                subCategories = subCategories.Where(x => x.CategoryId == model.CategoryId).ToList();

            return Map<IList<InvSubCategory_SLM>>(Map<IList<InvSubCategoryDto>>(subCategories));
        }
    }
}
