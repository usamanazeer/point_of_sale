using Models;
using Models.DTO.DeliveryService;
using Models.DTO.ViewModels.SelectList.DeliveryService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.DeliveryService.DeliveryServiceVendorServices
{
    public interface IDeliveryServiceVendorService
    {
        Task<Response> Create(string token, DeliDeliveryServiceVendorDto model);
        Task<Response> Delete(string token, int id);
        Task<Response> Edit(string token, DeliDeliveryServiceVendorDto model);
        Task<DeliDeliveryServiceVendorDto> Get(string token, DeliDeliveryServiceVendorDto model = null);
        Task<DeliDeliveryServiceVendorDto> Details(string token, int id);
        Task<IList<DeliveryServiceVendor_SLM>> GetSelectList(string token, DeliDeliveryServiceVendorDto model = null);
        Task<Response> GetSelectListResponse(string token, DeliDeliveryServiceVendorDto model = null);
        Task<Response> IsSelfExist(string token);
    }
}
