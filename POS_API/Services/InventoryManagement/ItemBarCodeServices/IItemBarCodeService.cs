using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.ItemBarCodeServices
{
    public interface IItemBarCodeService
    {
        Task<Response> GetAll(InvItemBarCodeDto model);
        Task<Response> Create(InvItemBarCodeDto model);
        Task<Response> Edit(InvItemBarCodeDto model);
        Task<Response> Delete(InvItemBarCodeDto model);
        Task<bool> IsExist(InvItemBarCodeDto model);
        Task<Response> GetDetails(InvItemBarCodeDto model);
        Task<Response> GetSelectList(InvItemBarCodeDto model);
    }
}
