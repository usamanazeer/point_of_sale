using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.PurchaseOrderRepos
{
    // ReSharper disable once InconsistentNaming
    public interface IPORepository
    {
        Task<InvPoMasterDto> Create(InvPoMasterDto model);
        Task<InvPoMasterDto> Edit(InvPoMasterDto model);
        Task<List<InvPoMasterDto>> GetAll(InvPoMasterDto model);
        //Task<bool> IsExist(InvPoMasterDTO model);
        Task<bool> Delete(InvPoMasterDto model);
        Task<InvPoMasterDto> GetDetails(InvPoMasterDto model);
        Task<IList<InvPoMaster_SLM>> GetSelectList(InvPoMasterDto model);
    }
}
