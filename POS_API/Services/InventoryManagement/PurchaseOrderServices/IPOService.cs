using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.PurchaseOrderServices
{
    // ReSharper disable once InconsistentNaming
    public interface IPOService
    {
        Task<Response> GetAll(InvPoMasterDto model);
        Task<Response> Create(InvPoMasterDto model);
        Task<Response> Edit(InvPoMasterDto model);
        Task<Response> Delete(InvPoMasterDto model);
        //Task<bool> IsExist(InvPoMasterDTO model);
        Task<Response> GetDetails(InvPoMasterDto model);
        Task<Response> GetSelectList(InvPoMasterDto model);
    }
}
