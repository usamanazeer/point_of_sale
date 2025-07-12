using Models.DTO.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.SizeRepos
{
    public interface ISizeRepository
    {
        Task<InvSizeDto> Create(InvSizeDto model);
        Task<InvSizeDto> Edit(InvSizeDto model);
        Task<List<InvSizeDto>> GetAll(InvSizeDto model);
        Task<bool> IsExist(InvSizeDto model);
        Task<bool> Delete(InvSizeDto model);
        Task<InvSizeDto> GetDetails(InvSizeDto model);
    }
}
