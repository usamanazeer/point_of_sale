using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO.DeliveryService;
using Models.DTO.ViewModels.SelectList.DeliveryService;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.DeliveryService.DeliveryServiceVendorServices
{
    public class DeliveryServiceVendorService : ServiceBase, IDeliveryServiceVendorService, IService
    {
        public DeliveryServiceVendorService(IClientManager clientManager) : base("api/DeliveryServiceVendor/", clientManager)
        {}

        public async Task<Response> Create(string token, DeliDeliveryServiceVendorDto model)
            =>DeserializeResponseModel<DeliDeliveryServiceVendorDto>(await Client.Post<Response>(Route + nameof(Create), model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>(url: $"{Route}Delete/{id}",token: token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<DeliDeliveryServiceVendorDto> Details(string token, int id)
        {
            var model = new DeliDeliveryServiceVendorDto();
            var response = await Client.Get<Response>(url: $"{Route}Details?id={id}", token: token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<DeliDeliveryServiceVendorDto>(value: response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, DeliDeliveryServiceVendorDto model)
            => DeserializeResponseModel<DeliDeliveryServiceVendorDto>(await Client.Post<Response>(Route + nameof(Edit), model, token: token));


        public async Task<DeliDeliveryServiceVendorDto> Get(string token, DeliDeliveryServiceVendorDto model = null)
        {
            var url = new StringBuilder($"{Route}Get");
            if (model != null)
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url: url.ToString(), token: token);
            model ??= new DeliDeliveryServiceVendorDto();
            model.Response = res;
            model.DeliveryServiceVendors = JsonConvert.DeserializeObject<List<DeliDeliveryServiceVendorDto>>(value: res.Model.String());
            return model;
        }

        public async Task<IList<DeliveryServiceVendor_SLM>> GetSelectList(string token, DeliDeliveryServiceVendorDto model = null)
        {
            var res = await GetSelectListResponse(token: token,model: model);
            return res.Model != null ? (IList<DeliveryServiceVendor_SLM>) res.Model : new List<DeliveryServiceVendor_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, DeliDeliveryServiceVendorDto model = null)
        {
            var res = await Client.Post<Response>(url: $"{Route}GetSelectList", obj: model ?? new DeliDeliveryServiceVendorDto(), token: token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<DeliveryServiceVendor_SLM>>(value: res.Model.String());
            return res;
        }

        public async Task<Response> IsSelfExist(string token)
        {
            var response = await Client.Get<Response>(url: Route + "IsSelfExist", token: token);
            response.Model = (bool) response.Model;
            return response;
        }
    }
}