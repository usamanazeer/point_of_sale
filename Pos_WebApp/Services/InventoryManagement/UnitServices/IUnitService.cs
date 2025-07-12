using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.UnitServices
{
    public interface IUnitService
    {
        Task<InvUnitDto> Get(string token, InvUnitDto model = null);
        Task<Response> Create(string token, InvUnitDto model);
        Task<Response> Edit(string token, InvUnitDto model);
        Task<Response> Delete(string token, int id);
        Task<InvUnitDto> Details(string token, int id);
    }
}
