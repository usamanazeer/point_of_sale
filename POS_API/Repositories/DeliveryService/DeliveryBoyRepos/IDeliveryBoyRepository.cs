using Models.DTO.DeliveryService;
using Models.DTO.ViewModels.SelectList.DeliveryService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.DeliveryService.DeliveryBoyRepos
{
    public interface IDeliveryBoyRepository
    {
        Task<DeliDeliveryBoyDto> Create(DeliDeliveryBoyDto deliDeliveryBoyDto);
        Task<DeliDeliveryBoyDto> Edit(DeliDeliveryBoyDto model);
        Task<List<DeliDeliveryBoyDto>> GetAll(DeliDeliveryBoyDto model);
        Task<bool> Delete(DeliDeliveryBoyDto model);
        Task<DeliDeliveryBoyDto> GetDetails(DeliDeliveryBoyDto model);
        Task<IList<DeliveryBoy_SLM>> GetSelectList(DeliDeliveryBoyDto model);
        Task<bool> IsExist(DeliDeliveryBoyDto model);
    }
}
