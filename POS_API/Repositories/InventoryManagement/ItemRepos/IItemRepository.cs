using Models.DTO.InventoryManagement;
using Models.DTO.InventoryManagement.ViewDTO;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS_API.Data.TVPs;

namespace POS_API.Repositories.InventoryManagement.ItemRepos
{
    public interface IItemRepository
    {
        Task<InvItemDto> Create(InvItemDto model);
        Task<InvItemDto> Edit(InvItemDto model);
        Task<List<InvItemViewDto>> GetAll(InvItemDto model);
        Task<List<InvItemViewDto>> GetAllWithModifiers(InvItemDto model/*, bool fromCache = false*/);
        Task<bool> IsExist(InvItemDto model);
        Task<bool> Delete(InvItemDto model);
        Task<InvItemDto> GetDetails(InvItemDto model);
        Task<IList<InvItem_SLM>> GetSelectList(InvItemDto model);
        Task<bool> UpdateImagePath(InvItemDto model);
        //Task UpdateItemsWithModifiersToCache(int companyId, IList<InvItemViewDto> dataList = null);
        //Task<IList<InvItemViewDto>> GetAllWithModifiersFromCache(InvItemDto model);
        Task<IList<BulkUploadItemsResponse>> ItemsBulkUpload(InvItemDto model,List<BulkUploadItemsTvp> data);
    }
}
