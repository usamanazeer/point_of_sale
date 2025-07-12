using Models.DTO.InventoryManagement;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.PhysicalInventoryRepos
{
    public interface IPhysicalInventoryRepository
    {
        Task<InvPhysicalInventoryDto> AddPhysicalInventory(InvPhysicalInventoryDto model);
        Task<bool> IsPhysicalInventoryExists(InvPhysicalInventoryDto model);
        Task<List<InvPhysicalInventoryDto>> GetAll(InvPhysicalInventoryDto model);
        Task<List<InvPhysicalInventoryViewDto>> GetPhysicalInventory_View(PhysicalInventoryViewFilter filters = null);
        Task<List<InvPhysicalInventoryViewDto>> GetLowInventory(PhysicalInventoryViewFilter filters = null);

        //Task<PhysicalInventoryViewDTO> GetDetails(PhysicalInventoryViewFilter model);
    }
}
