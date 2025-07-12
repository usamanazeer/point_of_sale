//using Models.DTO.DeliveryService;
//using Models.DTO.ViewModels.SelectList.DeliveryService;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace POS_API.Repositories.DeliveryService
//{
//    public interface IDeliveryServiceUnit
//    {
//        #region DeliveryServiceVendor
//        Task<DeliDeliveryServiceVendorDto> CreateDeliveryServiceVendor(DeliDeliveryServiceVendorDto model);
//        Task<DeliDeliveryServiceVendorDto> EditDeliveryServiceVendor(DeliDeliveryServiceVendorDto model);
//        Task<List<DeliDeliveryServiceVendorDto>> GetAllDeliveryServiceVendors(DeliDeliveryServiceVendorDto model);
//        Task<bool> DeleteDeliveryServiceVendor(DeliDeliveryServiceVendorDto model);
//        Task<DeliDeliveryServiceVendorDto> GetDeliveryServiceVendorDetails(DeliDeliveryServiceVendorDto model);
//        Task<IList<DeliveryServiceVendor_SLM>> GetDeliveryServiceVendorsSelectList(DeliDeliveryServiceVendorDto model);
//        Task<bool> IsDeliveryServiceVendorExist(DeliDeliveryServiceVendorDto model);
//        Task<bool> IsSelfDeliveryServiceVendorExist(int companyId);

//        #endregion



//        #region DeliveryBoy
//        Task<DeliDeliveryBoyDto> CreateDeliveryBoy(DeliDeliveryBoyDto deliDeliveryBoyDto);
//        Task<DeliDeliveryBoyDto> EditDeliveryBoy(DeliDeliveryBoyDto model);
//        Task<List<DeliDeliveryBoyDto>> GetAllDeliveryBoys(DeliDeliveryBoyDto model);
//        Task<bool> DeleteDeliveryBoy(DeliDeliveryBoyDto model);
//        Task<DeliDeliveryBoyDto> GetDeliveryBoyDetails(DeliDeliveryBoyDto model);
//        Task<IList<DeliveryBoy_SLM>> GetDeliveryBoysSelectList(DeliDeliveryBoyDto model);
//        Task<bool> IsDeliveryBoyExist(DeliDeliveryBoyDto model);
//        #endregion
//    }
//}
