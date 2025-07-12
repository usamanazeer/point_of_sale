
using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.ItemServices
{
    public interface IItemService
    {
        Task<Response> GetAll(InvItemDto model, bool forPos = false , bool withModifiers = false/*, bool fromCache = false*/);
        Task<Response> Create(InvItemDto model);
        Task<Response> SaveImage(InvItemDto model, IFormFile file);
        Task<Response> Edit(InvItemDto model);
        Task<Response> Delete(InvItemDto model);
        Task<bool> IsExist(InvItemDto model);
        Task<Response> GetDetails(InvItemDto model);
        Task<Response> GetSelectList(InvItemDto model);
        string GetBulkImportSampleFilePath();
        Task<Response> BulkUpload(InvItemDto model, IFormFile file);
    }
}
