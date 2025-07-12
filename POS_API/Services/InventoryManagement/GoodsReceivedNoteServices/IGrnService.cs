using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.GoodsReceivedNoteServices
{
    public interface IGrnService
    {
        Task<Response> GetAll(InvGrnMasterDto model);
        Task<Response> Create(InvGrnMasterDto model);
        Task<Response> Edit(InvGrnMasterDto model);
        Task<Response> Delete(InvGrnMasterDto model);
        //Task<bool> IsExist(InvGrnMasterDTO model);
        Task<Response> GetDetails(InvGrnMasterDto model);
        Task<Response> GetSelectList(InvGrnMasterDto model);
    }
}