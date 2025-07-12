using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.ItemBarCodeRepos
{
    public interface IItemBarCodeRepository
    {
        Task<InvItemBarCodeDto> Create(InvItemBarCodeDto model);
        Task<InvItemBarCodeDto> Edit(InvItemBarCodeDto model);
        Task<List<InvItemBarCodeDto>> GetAll(InvItemBarCodeDto model);
        Task<bool> IsExist(InvItemBarCodeDto model);
        Task<bool> Delete(InvItemBarCodeDto model);
        Task<InvItemBarCodeDto> GetDetails(InvItemBarCodeDto model);
        Task<IList<InvItemBarCode_SLM>> GetSelectList(InvItemBarCodeDto model);
    }
}
