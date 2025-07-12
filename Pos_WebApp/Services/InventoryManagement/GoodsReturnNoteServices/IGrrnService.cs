using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.GoodsReturnNoteServices
{
    public interface IGrrnService
    {
        Task<InvGrrnMasterDto> Get(string token, InvGrrnMasterDto model = null);
        Task<Response> Create(string token, InvGrrnMasterDto model);
        Task<Response> Edit(string token, InvGrrnMasterDto model);
        Task<Response> Delete(string token, int id);
        Task<InvGrrnMasterDto> Details(string token, int id);
        Task<Response> GetSelectListResponse(string token, InvGrrnMasterDto model = null);
        Task<IList<InvGrrnMaster_SLM>> GetSelectList(string token, InvGrrnMasterDto model = null);
    }
}
