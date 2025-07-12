using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.CategoryServices
{
    public interface ISubCategoryService
    {
        Task<InvSubCategoryDto> Get(string token, InvSubCategoryDto model = null);
        Task<InvSubCategoryDto> GetById(int id, string token);
        Task<Response> Edit(string token, InvSubCategoryDto model, IFormFile categoryImage = null);
        Task<Response> Create(string token, InvSubCategoryDto model, IFormFile categoryImage = null);
        Task<Response> Delete(string token, int id);
        Task<Response> GetSelectListResponse(string token, InvSubCategoryDto filter);
    }
}
