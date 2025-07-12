using Models.DTO.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.BrandRepos
{
    public interface IBrandRepository
    {
        Task<InvBrandDto> Create(InvBrandDto model);
        Task<InvBrandDto> Edit(InvBrandDto model);
        Task<List<InvBrandDto>> GetAll(InvBrandDto model);
        Task<bool> IsExist(InvBrandDto model);
        Task<bool> Delete(InvBrandDto model);
        Task<InvBrandDto> GetDetails(InvBrandDto model);
    }
}
