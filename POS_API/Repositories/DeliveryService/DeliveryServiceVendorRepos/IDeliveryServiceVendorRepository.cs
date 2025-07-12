using Models.DTO.DeliveryService;
using Models.DTO.ViewModels.SelectList.DeliveryService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.DeliveryService.DeliveryServiceVendorRepos
{
    public interface IDeliveryServiceVendorRepository
    {
        Task<DeliDeliveryServiceVendorDto> Create(DeliDeliveryServiceVendorDto deliveryServiceVendorDto);
        Task<DeliDeliveryServiceVendorDto> Edit(DeliDeliveryServiceVendorDto deliveryServiceVendorDto);
        Task<List<DeliDeliveryServiceVendorDto>> GetAll(DeliDeliveryServiceVendorDto deliveryServiceVendorDto);
        Task<bool> Delete(DeliDeliveryServiceVendorDto deliveryServiceVendorDto);
        Task<DeliDeliveryServiceVendorDto> GetDetails(DeliDeliveryServiceVendorDto deliveryServiceVendorDto);
        Task<IList<DeliveryServiceVendor_SLM>> GetSelectList(DeliDeliveryServiceVendorDto deliveryServiceVendorDto);
        Task<bool> IsExist(DeliDeliveryServiceVendorDto deliveryServiceVendorDto);
        Task<bool> IsSelfExist(int companyId);
    }
}