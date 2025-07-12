using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.DeliveryService;
using Models.DTO.ViewModels.SelectList.DeliveryService;

namespace Pos_WebApp.Services.DeliveryService.DeliveryBoyServices
{
    public interface IDeliveryBoyService
    {
        Task<Response> Create(string token, DeliDeliveryBoyDto model);
        Task<Response> Delete(string token, int id);
        Task<Response> Edit(string token, DeliDeliveryBoyDto model);
        Task<DeliDeliveryBoyDto> Get(string token, DeliDeliveryBoyDto model = null);
        Task<DeliDeliveryBoyDto> Details(string token, int id);
        Task<IList<DeliveryBoy_SLM>> GetSelectList(string token, DeliDeliveryBoyDto model = null);
        Task<Response> GetSelectListResponse(string token, DeliDeliveryBoyDto model = null);
    }
}
