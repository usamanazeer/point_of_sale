using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;


namespace POS_API.Services.InventoryManagement.BrandServices
{
    public interface IBrandService
    {
        Task<Response> GetAll(InvBrandDto model);
        Task<Response> Create(InvBrandDto model);
        Task<Response> Edit(InvBrandDto model);
        Task<Response> Delete(InvBrandDto model);
        Task<bool> IsExist(InvBrandDto model);
        Task<Response> GetDetails(InvBrandDto model);
    }
}
