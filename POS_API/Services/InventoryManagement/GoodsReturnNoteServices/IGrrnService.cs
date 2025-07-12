using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.GoodsReturnNoteServices
{
    public interface IGrrnService
    {
        Task<Response> GetAll(InvGrrnMasterDto model);
        Task<Response> Create(InvGrrnMasterDto model);
        Task<Response> Edit(InvGrrnMasterDto model);
        Task<Response> Delete(InvGrrnMasterDto model);
        Task<Response> GetDetails(InvGrrnMasterDto model);
        Task<Response> GetSelectList(InvGrrnMasterDto model);
    }
}
