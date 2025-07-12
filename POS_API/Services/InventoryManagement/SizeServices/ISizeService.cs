using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.SizeServices
{
    public interface ISizeService
    {
        Task<Response> GetAll(InvSizeDto model);
        Task<Response> Create(InvSizeDto model);
        Task<Response> Edit(InvSizeDto model);
        Task<Response> Delete(InvSizeDto model);
        Task<bool> IsExist(InvSizeDto model);
        Task<Response> GetDetails(InvSizeDto model);
    }
}
