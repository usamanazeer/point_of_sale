using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.ColorServices
{
    public interface IColorService
    {
        Task<Response> GetAll(InvColorDto model);
        Task<Response> Create(InvColorDto model);
        Task<Response> Edit(InvColorDto model);
        Task<Response> Delete(InvColorDto model);
        Task<bool> IsExist(InvColorDto model);
        Task<Response> GetDetails(InvColorDto model);
    }
}
