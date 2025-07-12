using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.CategoryServices
{
    public interface IMainCategoryService
    {
        Task<Response> GetAll(InvCategoryDto model);
        Task<Response> Create(InvCategoryDto model, IFormFile file);
        Task<Response> Edit(InvCategoryDto model, IFormFile file);
        Task<Response> Delete(InvCategoryDto model);
    }
}
