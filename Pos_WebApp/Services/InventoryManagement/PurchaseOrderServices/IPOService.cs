using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.PurchaseOrderServices
{
    // ReSharper disable once InconsistentNaming
    public interface IPOService
    {
        Task<InvPoMasterDto> Get(string token, InvPoMasterDto model = null);
        Task<Response> Create(string token, InvPoMasterDto model);
        Task<Response> Edit(string token, InvPoMasterDto model);
        Task<Response> Delete(string token, int id);
        Task<InvPoMasterDto> Details(string token, int id);
        Task<Response> GetDetailsResponse(string token, int id);
        Task<Response> GetSelectListResponse(string tOKEN, InvPoMasterDto model = null);
        Task<IList<InvPoMaster_SLM>> GetSelectList(string tOKEN, InvPoMasterDto model = null);
    }
}
