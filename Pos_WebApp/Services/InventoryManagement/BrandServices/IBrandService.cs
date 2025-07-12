using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.BrandServices
{
    public interface IBrandService
    {
        Task<InvBrandDto> Get(string token, InvBrandDto model = null);
        Task<Response> Create(string token, InvBrandDto model);
        Task<Response> Edit(string token, InvBrandDto model);
        Task<Response> Delete(string token, int id);
        Task<InvBrandDto> Details(string token, int id);
    }
}
