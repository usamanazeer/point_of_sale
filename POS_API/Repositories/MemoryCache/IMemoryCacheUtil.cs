using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.InventoryManagement.ViewDTO;

namespace POS_API.Repositories.MemoryCache
{
    public interface IMemoryCacheUtil
    {
        Task UpdateCache_Category(int companyId);
        Task UpdateCache_SubCategory(int companyId);
        Task UpdateCache_Items(int companyId, IList<InvItemViewDto> dataList = null);
    }
}
