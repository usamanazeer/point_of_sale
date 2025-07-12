using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.ModifierRepos
{
    public interface IModifierRepository
    {
        Task<InvModifierDto> Create(InvModifierDto model);
        Task<InvModifierDto> Edit(InvModifierDto model);
        Task<List<InvModifierDto>> GetAll(InvModifierDto model);
        Task<bool> IsExist(InvModifierDto model);
        Task<bool> Delete(InvModifierDto model);
        Task<InvModifierDto> GetDetails(InvModifierDto model);
        Task<IList<InvModifier_SLM>> GetSelectList(InvModifierDto model);
    }
}
