using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO.DeliveryService;
using Models.DTO.ViewModels.SelectList.DeliveryService;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.DeliveryService.DeliveryBoyServices
{
    public class DeliveryBoyService : ServiceBase, IDeliveryBoyService, IService
    {
        public DeliveryBoyService(IClientManager clientManager) : base("api/deliveryboy/", clientManager)
        {}


        public async Task<Response> Create(string token, DeliDeliveryBoyDto model)
            => DeserializeResponseModel<DeliDeliveryBoyDto>(await Client.Post<Response>(Route + nameof(Create), model, token: token));

        public async Task<Response> Delete(string token,int id)
        {
            var response = await Client.Get<Response>(url: $"{Route}Delete/{id}",token: token);
            response.Model = (bool) response.Model;
            return response;
        }

        public async Task<DeliDeliveryBoyDto> Details(string token, int id)
        {
            var model = new DeliDeliveryBoyDto();
            var response = await Client.Get<Response>(url: $"{Route}Details?id={id}", token: token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<DeliDeliveryBoyDto>(value: response.Model.String());
            model.Response = response;
            return model;
        }


        public async Task<Response> Edit(string token, DeliDeliveryBoyDto model)
            => DeserializeResponseModel<DeliDeliveryBoyDto>(await Client.Post<Response>(Route + nameof(Edit), model, token: token));


        public async Task<DeliDeliveryBoyDto> Get(string token, DeliDeliveryBoyDto model = null)
        {
            var urlBuilder = new StringBuilder($"{Route}Get");
            if (model != null)
                urlBuilder.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url: urlBuilder.ToString(), token: token);
            model ??= new DeliDeliveryBoyDto();
            model.Response = res;
            model.DeliveryBoys = JsonConvert.DeserializeObject<List<DeliDeliveryBoyDto>>(value: res.Model.String());
            return model;
        }


        public async Task<IList<DeliveryBoy_SLM>> GetSelectList(string token, DeliDeliveryBoyDto model = null)
        {
            var res = await GetSelectListResponse(token: token, model: model);
            return res.Model != null ? (IList<DeliveryBoy_SLM>) res.Model : new List<DeliveryBoy_SLM>();
        }


        public async Task<Response> GetSelectListResponse(string token,
                                                          DeliDeliveryBoyDto model = null)
        {
            var res = await Client.Post<Response>(url: $"{Route}GetSelectList",  obj: model ?? new DeliDeliveryBoyDto(), token: token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<DeliveryBoy_SLM>>(value: res.Model.String());
            return res;
        }
    }
}