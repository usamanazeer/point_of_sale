using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.GoodsReceivedNoteRepos
{
    public interface IGrnRepository
    {
        Task<InvGrnMasterDto> Create(InvGrnMasterDto model);
        Task<InvGrnMasterDto> Edit(InvGrnMasterDto model);
        Task<List<InvGrnMasterDto>> GetAll(InvGrnMasterDto model);
        //Task<bool> IsExist(InvGrnMasterDTO model);
        Task<bool> Delete(InvGrnMasterDto model);
        Task<InvGrnMasterDto> GetDetails(InvGrnMasterDto model);
        Task<IList<InvGrnMaster_SLM>> GetSelectList(InvGrnMasterDto model);
    }
}
