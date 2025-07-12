using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.ModifierServices
{
    public interface IModifierService
    {
        Task<InvModifierDto> Get(string token, InvModifierDto model = null);
        Task<Response> Create(string token, InvModifierDto model);
        Task<Response> Edit(string token, InvModifierDto model);
        Task<Response> Delete(string token, int id);
        Task<InvModifierDto> Details(string token, int id);
        Task<Response> GetSelectListResponse(string tOKEN, InvModifierDto model = null);
        Task<IList<InvModifier_SLM>> GetSelectList(string tOKEN, InvModifierDto model = null);
    }
}
