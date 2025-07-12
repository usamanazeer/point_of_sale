using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.ItemBarCodeServices
{
    public interface IItemBarCodeService
    {
        Task<InvItemBarCodeDto> Get(string token, InvItemBarCodeDto model = null);
        Task<Response> Create(string token, InvItemBarCodeDto model);
        Task<Response> Edit(string token, InvItemBarCodeDto model);
        Task<Response> Delete(string token, int id);
        Task<InvItemBarCodeDto> Details(string token, int id);
        Task<Response> GetSelectListResponse(string tOKEN, InvItemBarCodeDto model = null);
        Task<IList<InvItemBarCode_SLM>> GetSelectList(string token, InvItemBarCodeDto model = null);
    }
}
