using Models.DTO.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.UnitRepos
{
    public interface IUnitRepository
    {
        Task<InvUnitDto> Create(InvUnitDto model);
        Task<InvUnitDto> Edit(InvUnitDto model);
        Task<List<InvUnitDto>> GetAll(InvUnitDto model);
        Task<bool> IsExist(InvUnitDto model);
        Task<bool> Delete(InvUnitDto model);
        Task<InvUnitDto> GetDetails(InvUnitDto model);
    }
}
