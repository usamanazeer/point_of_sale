using Models;
using Models.DTO.DeliveryService;
using System.Threading.Tasks;

namespace POS_API.Services.DeliveryService.DeliveryServiceVendorServices
{
    public interface IDeliveryServiceVendorService
    {
        Task<Response> GetAll(DeliDeliveryServiceVendorDto model);
        Task<Response> Create(DeliDeliveryServiceVendorDto model);
        Task<Response> Edit(DeliDeliveryServiceVendorDto model);
        Task<bool> Delete(DeliDeliveryServiceVendorDto model);
        Task<Response> GetDetails(DeliDeliveryServiceVendorDto model);
        Task<Response> GetSelectList(DeliDeliveryServiceVendorDto model);
        Task<bool> IsExist(DeliDeliveryServiceVendorDto model);
        Task<bool> IsSelfExist(int companyId);
    }
}
