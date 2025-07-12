using Models;
using Models.DTO.DeliveryService;
using System.Threading.Tasks;

namespace POS_API.Services.DeliveryService.DeliveryBoyServices
{
    public interface IDeliveryBoyService
    {
        Task<Response> GetAll(DeliDeliveryBoyDto model);
        Task<Response> Create(DeliDeliveryBoyDto model);
        Task<Response> Edit(DeliDeliveryBoyDto model);
        Task<bool> Delete(DeliDeliveryBoyDto model);
        Task<Response> GetDetails(DeliDeliveryBoyDto model);
        Task<Response> GetSelectList(DeliDeliveryBoyDto model);
        Task<bool> IsExist(DeliDeliveryBoyDto model);
    }
}
