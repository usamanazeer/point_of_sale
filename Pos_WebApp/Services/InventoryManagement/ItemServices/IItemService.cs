using Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;

namespace Pos_WebApp.Services.InventoryManagement.ItemServices
{
    public interface IItemService
    {
        Task<Response> Delete(string token, int id);
        Task<InvItemDto> Details(string token, int id);
        Task<string> GetItemsBulkUploadSamplePath(string token);
        Task<InvItemDto> Get(string token, InvItemDto model = null);
        Task<Response> GetSelectListResponse(string token, InvItemDto model = null);
        Task<IList<InvItem_SLM>> GetSelectList(string token, InvItemDto model = null);
        Task<Response> Edit(string token, InvItemDto model, IFormFile itemImage = null);
        Task<Response> Create(string token, InvItemDto model, IFormFile itemImage = null);
        Task<Response> BulkUpload(IFormFile file, string token);
    }
}
