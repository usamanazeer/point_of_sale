using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.CategoryRepos
{
    public interface ISubCategoryRepository
    {
        Task<List<InvSubCategoryDto>> GetAll(InvSubCategoryDto model);
        Task<InvSubCategoryDto> Create(InvSubCategoryDto model);
        Task<InvSubCategoryDto> Edit(InvSubCategoryDto model);
        Task<bool> Delete(InvSubCategoryDto model);
        Task<bool> IsExist(InvSubCategoryDto model);
        Task<IList<InvSubCategory_SLM>> GetSelectList(InvSubCategoryDto model, bool forPos = false);
    }
}
