using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.UnitServices
{
    public interface IUnitService
    {
        Task<Response> GetAll(InvUnitDto model);
        Task<Response> Create(InvUnitDto model);
        Task<Response> Edit(InvUnitDto model);
        Task<Response> Delete(InvUnitDto model);
        Task<bool> IsExist(InvUnitDto model);
        Task<Response> GetDetails(InvUnitDto model);
    }
}
