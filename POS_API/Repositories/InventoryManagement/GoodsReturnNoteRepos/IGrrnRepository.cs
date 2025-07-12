using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.GoodsReturnNoteRepos
{
    public interface IGrrnRepository
    {
        Task<InvGrrnMasterDto> Create(InvGrrnMasterDto model);
        Task<InvGrrnMasterDto> Edit(InvGrrnMasterDto model);
        Task<List<InvGrrnMasterDto>> GetAll(InvGrrnMasterDto model);
        Task<bool> Delete(InvGrrnMasterDto model);
        Task<InvGrrnMasterDto> GetDetails(InvGrrnMasterDto model);
        Task<IList<InvGrrnMaster_SLM>> GetSelectList(InvGrrnMasterDto model);
    }
}
