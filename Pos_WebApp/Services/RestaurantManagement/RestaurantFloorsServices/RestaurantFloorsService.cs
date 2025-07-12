using Models;
using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.RestaurantManagement.RestaurantFloorsServices
{
    public class RestaurantFloorsService: ServiceBase, IRestaurantFloorsService, IService
    {
        public RestaurantFloorsService(IClientManager clientManager) : base("api/restaurantFloor/", clientManager){}
        public async Task<Response> Create(string token, RestRestaurantFloorsDto model)
            => DeserializeResponseModel<RestRestaurantFloorsDto>(await Client.Post<Response>(Route + nameof(Create), model, token: token));
        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>(url: $"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }
        
        public async Task<Response> Edit(string token, RestRestaurantFloorsDto model)
            => DeserializeResponseModel<RestRestaurantFloorsDto>(await Client.Post<Response>(Route + nameof(Edit), model, token: token));
        
        public async Task<RestRestaurantFloorsDto> Get(string token, RestRestaurantFloorsDto model = null)
        {
            var url = new StringBuilder($"{Route}Get");
            if (model != null)
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new RestRestaurantFloorsDto();
            model.Response = res;
            model.RestaurantFloors = JsonConvert.DeserializeObject<List<RestRestaurantFloorsDto>>(res.Model.String());
            return model;
        }

        public async Task<RestRestaurantFloorsDto> Details(string token, int id)
        {
            var model = new RestRestaurantFloorsDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<RestRestaurantFloorsDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<IList<RestRestaurantFloors_SLM>> GetSelectList(string token, RestRestaurantFloorsDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (IList<RestRestaurantFloors_SLM>) res.Model : new List<RestRestaurantFloors_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, RestRestaurantFloorsDto model = null)
        {
            var url = $"{Route}GetSelectList";
            var res = await Client.Post<Response>(url, model ?? new RestRestaurantFloorsDto(), token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<RestRestaurantFloors_SLM>>(res.Model.String());
            return res;
        }
    }
}
