using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.ModifierServices
{
    public interface IModifierService
    {
        Task<Response> GetAll(InvModifierDto model);
        Task<Response> Create(InvModifierDto model);
        Task<Response> Edit(InvModifierDto model);
        Task<Response> Delete(InvModifierDto model);
        Task<bool> IsExist(InvModifierDto model);
        Task<Response> GetDetails(InvModifierDto model);
        Task<Response> GetSelectList(InvModifierDto model);
    }
}
