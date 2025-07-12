//using Models.DTO.DeliveryService;
//using Models.DTO.ViewModels.SelectList.DeliveryService;
//using POS_API.Repositories.DeliveryService.DeliveryBoyRepos;
//using POS_API.Repositories.DeliveryService.DeliveryServiceVendorRepos;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace POS_API.Repositories.DeliveryService
//{
//    public class DeliveryServiceUnit : IDeliveryServiceUnit
//    {
//        private readonly IDeliveryServiceVendorRepository _deliveryServiceVendorRepository;
//        private readonly IDeliveryBoyRepository _deliveryBoyRepository;
//        public DeliveryServiceUnit(IDeliveryServiceVendorRepository deliveryServiceVendorRepository,
//            IDeliveryBoyRepository deliveryBoyRepository
//            )
//        {
//            _deliveryServiceVendorRepository = deliveryServiceVendorRepository;
//            _deliveryBoyRepository = deliveryBoyRepository;
//        }

//        #region DeliveryServiceVendor

//        public async Task<DeliDeliveryServiceVendorDto> CreateDeliveryServiceVendor(DeliDeliveryServiceVendorDto model)
//        {
//            return await _deliveryServiceVendorRepository.Create(model);
//        }


//        public async Task<bool> DeleteDeliveryServiceVendor(DeliDeliveryServiceVendorDto model)
//        {
//            return await _deliveryServiceVendorRepository.Delete(model);
//        }

//        public async Task<DeliDeliveryServiceVendorDto> EditDeliveryServiceVendor(DeliDeliveryServiceVendorDto model)
//        {
//            return await _deliveryServiceVendorRepository.Edit(model);
//        }

//        public async Task<List<DeliDeliveryServiceVendorDto>> GetAllDeliveryServiceVendors(DeliDeliveryServiceVendorDto model)
//        {
//            return await _deliveryServiceVendorRepository.GetAll(model);
//        }

//        public async Task<DeliDeliveryServiceVendorDto> GetDeliveryServiceVendorDetails(DeliDeliveryServiceVendorDto model)
//        {
//            return await _deliveryServiceVendorRepository.GetDetails(model);
//        }

//        public async Task<IList<DeliveryServiceVendor_SLM>> GetDeliveryServiceVendorsSelectList(DeliDeliveryServiceVendorDto model)
//        {
//            return await _deliveryServiceVendorRepository.GetSelectList(model);
//        }

//        public async Task<bool> IsDeliveryServiceVendorExist(DeliDeliveryServiceVendorDto model)
//        {
//            return await _deliveryServiceVendorRepository.IsExist(model);
//        }
//        public async Task<bool> IsSelfDeliveryServiceVendorExist(int companyId)
//        {
//            return await _deliveryServiceVendorRepository.IsSelfExist(companyId);
//        }
//        #endregion



//        #region DeliveryBoy

//        public async Task<DeliDeliveryBoyDto> CreateDeliveryBoy(DeliDeliveryBoyDto deliDeliveryBoyDto)
//        {
//            return await _deliveryBoyRepository.Create(deliDeliveryBoyDto);
//        }
//        public async Task<bool> DeleteDeliveryBoy(DeliDeliveryBoyDto model)
//        {
//            return await _deliveryBoyRepository.Delete(model);
//        }
//        public async Task<DeliDeliveryBoyDto> EditDeliveryBoy(DeliDeliveryBoyDto model)
//        {
//            return await _deliveryBoyRepository.Edit(model);
//        }
//        public async Task<List<DeliDeliveryBoyDto>> GetAllDeliveryBoys(DeliDeliveryBoyDto model)
//        {
//            return await _deliveryBoyRepository.GetAll(model);
//        }

//        public async Task<DeliDeliveryBoyDto> GetDeliveryBoyDetails(DeliDeliveryBoyDto model)
//        {
//            return await _deliveryBoyRepository.GetDetails(model);
//        }

//        public async Task<IList<DeliveryBoy_SLM>> GetDeliveryBoysSelectList(DeliDeliveryBoyDto model)
//        {
//            return await _deliveryBoyRepository.GetSelectList(model);
//        }
//        public async Task<bool> IsDeliveryBoyExist(DeliDeliveryBoyDto model)
//        {
//            return await _deliveryBoyRepository.IsExist(model);
//        }
//        #endregion
//    }
//}
