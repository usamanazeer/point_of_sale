using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.PhysicalInventory.PhysicalInventoryServices
{
    public interface IPhysicalInventoryService
    {
        Task<Response> Add(InvPhysicalInventoryDto model);
        Task<bool> IsExist(InvPhysicalInventoryDto model);
        Task<Response> GetAll(InvPhysicalInventoryDto model);
        Task<Response> GetPhysicalInventory_View(PhysicalInventoryViewFilter filters = null);
        Task<Response> GetBillDetails(PhysicalInventoryViewFilter model);
        Task<Response> GetLowInventory(PhysicalInventoryViewFilter filters = null);
    }
}
