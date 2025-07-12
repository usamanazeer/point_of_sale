using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.ColorServices
{
    public interface IColorService
    {
        Task<InvColorDto> Get(string token, InvColorDto model = null);
        Task<Response> Create(string token, InvColorDto model);
        Task<Response> Edit(string token, InvColorDto model);
        Task<Response> Delete(string token, int id);
        Task<InvColorDto> Details(string token, int id);
    }
}
