using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.GoodsReceivedNoteServices
{
    public interface IGrnService
    {
        Task<InvGrnMasterDto> Get(string token, InvGrnMasterDto model = null);
        Task<Response> Create(string token, InvGrnMasterDto model);
        Task<Response> Edit(string token, InvGrnMasterDto model);
        Task<Response> Delete(string token, int id);
        Task<InvGrnMasterDto> Details(string token, int id);
        Task<Response> GetSelectListResponse(string token, InvGrnMasterDto model = null);
        Task<IList<InvGrnMaster_SLM>> GetSelectList(string token, InvGrnMasterDto model = null);
    }
}
