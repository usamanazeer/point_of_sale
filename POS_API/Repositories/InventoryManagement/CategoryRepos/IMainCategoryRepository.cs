using Models.DTO.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.CategoryRepos
{
    public interface IMainCategoryRepository
    {
        Task<List<InvCategoryDto>> GetAll(InvCategoryDto model);
        Task<InvCategoryDto> Create(InvCategoryDto model);
        Task<InvCategoryDto> Edit(InvCategoryDto model);
        Task<bool> Delete(InvCategoryDto model);
        Task<bool> IsExist(InvCategoryDto model);
    }
}
