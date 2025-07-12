using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.SizeServices
{
    public interface ISizeService
    {
        Task<InvSizeDto> Get(string token, InvSizeDto model = null);
        Task<Response> Create(string token, InvSizeDto model);
        Task<Response> Edit(string token, InvSizeDto model);
        Task<Response> Delete(string token, int id);
        Task<InvSizeDto> Details(string token, int id);
    }
}
