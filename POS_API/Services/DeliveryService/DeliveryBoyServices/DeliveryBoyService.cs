using System.Linq;
using Models;
using Models.DTO.DeliveryService;
using Models.Enums;
using POS_API.Repositories.DeliveryService.DeliveryBoyRepos;
using System.Threading.Tasks;

namespace POS_API.Services.DeliveryService.DeliveryBoyServices
{
    public class DeliveryBoyService: IDeliveryBoyService, IService
    {
        private readonly IDeliveryBoyRepository _deliveryBoyRepository;
        public DeliveryBoyService(IDeliveryBoyRepository deliveryBoyRepository) => _deliveryBoyRepository = deliveryBoyRepository;

        public async Task<Response> Create(DeliDeliveryBoyDto deliDeliveryBoyDto)
        {
            var returnModel = await _deliveryBoyRepository.Create(deliDeliveryBoyDto: deliDeliveryBoyDto);
            var response = returnModel.Response;
            response.Model = deliDeliveryBoyDto;
            return response;
        }

        public async Task<bool> Delete(DeliDeliveryBoyDto model) => await _deliveryBoyRepository.Delete(model);

        public async Task<Response> Edit(DeliDeliveryBoyDto model)
        {
            var response = new Response();
            var responseModel = await _deliveryBoyRepository.Edit(model: model);
            if (responseModel is null)
            {
                response.SetError("Delivery Boy Not Found.", StatusCodes.Not_Found);
            }
            else
            {
                response = responseModel.Response;
                response.Model = responseModel;
            }
            return response;
        }

        public async Task<Response> GetAll(DeliDeliveryBoyDto model)
        {
            var response = new Response();
            var res = await _deliveryBoyRepository.GetAll(model);
            if (res.Any())
                response.SetError(null, model: res);
            else
                response.SetMessage("No Delivery Boy Found.", StatusCodes.Not_Found);
            return response;
        }

        public async Task<Response> GetDetails(DeliDeliveryBoyDto model)
        {
            var response = new Response();
            var res = await _deliveryBoyRepository.GetDetails(model);
            if (res != null)
                response.SetMessage(null, model: res);
            else
                response.SetMessage("Delivery Boy Not Found", StatusCodes.Not_Found);
            return response;
        }

        public async Task<Response> GetSelectList(DeliDeliveryBoyDto model)
        {
            var response = new Response();
            var itemsList = await _deliveryBoyRepository.GetSelectList(model);
            if (itemsList != null)
                response.SetMessage(null, model: itemsList);
            else
                response.SetMessage("Delivery Boy Not Found.", StatusCodes.Not_Found);
            return response;
        }
        public async Task<bool> IsExist(DeliDeliveryBoyDto model) => await _deliveryBoyRepository.IsExist(model);
    }
}
