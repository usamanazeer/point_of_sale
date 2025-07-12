using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using POS_API.Data;
using Models;
using Models.DTO.InventoryManagement.ViewDTO;
using POS_API.Utilities;
using StatusType = Models.Enums.StatusTypes;
namespace POS_API.Repositories.MemoryCache
{
    public class MemoryCacheUtil:RepositoryBase, IMemoryCacheUtil, IRepository
    {
        private readonly IMemoryCache _memoryCache;
        private string GetSubCategoriesCacheKey(int companyId) => $"subCategories{companyId}";
        private string GetCategoriesCacheKey(int companyId) => $"Categories{companyId}";
        private string GetItemsWithModifiers_CacheKey(int companyId) => $"itemsWithModifiers{companyId}";
        public MemoryCacheUtil(PosDB_Context dbContext, IMapper mapper, IMemoryCache memoryCache) : base(dbContext, mapper) => _memoryCache = memoryCache;


        public async Task UpdateCache_Category(int companyId)
        {
            if (_memoryCache.IsAvailableInCache(GetCategoriesCacheKey(companyId)))
            {
                var updatedData = await _dbContext.InvCategory
                    .Where(x => x.CompanyId == companyId && x.Status == StatusType.Active.ToInt()).AsNoTracking().ToListAsync();
                _memoryCache.UpdateListToCache(GetCategoriesCacheKey(companyId), updatedData, 10, 5, CacheItemPriority.High);
            }
        }


        public async Task UpdateCache_SubCategory(int companyId)
        {
            if (_memoryCache.IsAvailableInCache(GetSubCategoriesCacheKey(companyId)))
            {
                var updatedData= await _dbContext.InvSubCategory.Include(x => x.Category)
                                                  .Where(x => x.CompanyId == companyId &&
                                                              x.Status == StatusType.Active.ToInt()).AsNoTracking().ToListAsync();
                _memoryCache.UpdateListToCache(GetSubCategoriesCacheKey(companyId), updatedData, 10, 5, CacheItemPriority.High);
            }
        }


        public async Task UpdateCache_Items(int companyId, IList<InvItemViewDto> dataList = null)
        {
            if (_memoryCache.IsAvailableInCache(GetItemsWithModifiers_CacheKey(companyId)))
            {
                List<InvItemView> dataListToSave;
                if (dataList is null)
                {

                    var query = from item in _dbContext.InvItemView
                                where item.CompanyId == companyId && item.Status != StatusType.Delete.ToInt()
                                select item;
                    dataListToSave = await query.AsNoTracking().ToListAsync();
                    _memoryCache.UpdateListToCache(GetItemsWithModifiers_CacheKey(companyId), dataListToSave,10,5,CacheItemPriority.High);
                }
                else
                {
                    dataListToSave = _mapper.Map<List<InvItemView>>(dataList);
                }
                _memoryCache.UpdateListToCache(GetItemsWithModifiers_CacheKey(companyId),
                                               dataListToSave,
                                               10,
                                               5,
                                               CacheItemPriority.High);
            }
        }
    }
}
