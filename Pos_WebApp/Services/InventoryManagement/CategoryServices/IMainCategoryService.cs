
using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.CategoryServices
{
    public interface IMainCategoryService
    {
        Task<InvCategoryDto> Get(string token, InvCategoryDto model = null);
        Task<InvCategoryDto> GetById(int id, string token);
        Task<Response> Edit(string token, InvCategoryDto model, IFormFile categoryImage = null);
        Task<Response> Create(string token, InvCategoryDto model, IFormFile categoryImage = null);
        Task<Response> Delete(string token, int id);
    }
}
