using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO.DeliveryService;
using Models.Enums;
using POS_API.Repositories.DeliveryService.DeliveryServiceVendorRepos;

namespace POS_API.Services.DeliveryService.DeliveryServiceVendorServices
{
    public class DeliveryServiceVendorService : IDeliveryServiceVendorService, IService
    {
        private readonly IDeliveryServiceVendorRepository _deliveryServiceVendorRepository;

        public DeliveryServiceVendorService(IDeliveryServiceVendorRepository deliveryServiceVendorRepository) => _deliveryServiceVendorRepository = deliveryServiceVendorRepository;

        public async Task<Response> Create(DeliDeliveryServiceVendorDto model)
        {
            var returnModel = await _deliveryServiceVendorRepository.Create(model);
            var response = returnModel.Response;
            response.Model = model;
            return response;
        }

        public async Task<bool> Delete(DeliDeliveryServiceVendorDto model) => await _deliveryServiceVendorRepository.Delete( model);

        public async Task<Response> Edit(DeliDeliveryServiceVendorDto model)
        {
            var response = new Response();
            var responseModel = await _deliveryServiceVendorRepository.Edit( model);
            if (responseModel is null)
            {
                response.SetError("Delivery Service Vendor Not Found.", StatusCodes.Not_Found);
            }
            else
            {
                response = responseModel.Response;
                response.Model = responseModel;
            }
            return response;
        }

        public async Task<Response> GetAll(DeliDeliveryServiceVendorDto model)
        {
            var response = new Response();
            var res = await _deliveryServiceVendorRepository.GetAll(model);
            if (res.Any())
                response.SetMessage(null, model: res);
            else
                response.SetMessage("No Delivery Service Vendor Found.", StatusCodes.Not_Found);
            return response;
        }

        public async Task<Response> GetDetails(DeliDeliveryServiceVendorDto model)
        {
            var response = new Response();
            var res = await _deliveryServiceVendorRepository.GetDetails( model);
            if (res != null)
                response.SetMessage(null, model: res);
            else
                response.SetMessage("Delivery Service Vendor Not Found", StatusCodes.Not_Found);
            return response;
        }

        public async Task<Response> GetSelectList(DeliDeliveryServiceVendorDto model)
        {
            var response = new Response();
            var itemsList = await _deliveryServiceVendorRepository.GetSelectList( model);
            if (itemsList != null)
                response.SetMessage(null, model: itemsList);
            else
                response.SetMessage("Delivery Service Vendor Not Found.", StatusCodes.Not_Found);
            return response;
        }

        public async Task<bool> IsExist(DeliDeliveryServiceVendorDto model) => await _deliveryServiceVendorRepository.IsExist( model);

        public async Task<bool> IsSelfExist(int companyId) => await _deliveryServiceVendorRepository.IsSelfExist(companyId: companyId);
    }
}