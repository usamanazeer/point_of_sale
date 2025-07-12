using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.CategoryServices
{
    public interface ISubCategoryService
    {
        Task<Response> GetAll(InvSubCategoryDto model);
        Task<Response> Create(InvSubCategoryDto model, IFormFile file);
        Task<Response> Edit(InvSubCategoryDto model, IFormFile file);
        Task<Response> Delete(InvSubCategoryDto model);
        Task<Response> GetSelectList(InvSubCategoryDto model, bool forPos = false);
    }
}
