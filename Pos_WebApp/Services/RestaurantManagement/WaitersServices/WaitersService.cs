using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using Newtonsoft.Json;
using Pos_WebApp.Services.RastaurantManagement.WaitersServices;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.RestaurantManagement.WaitersServices
{
    public class WaitersService : ServiceBase, IWaitersService, IService
    {
        public WaitersService(IClientManager clientManager) : base("api/waiter/", clientManager) {}
        public async Task<Response> Create(string token, RestWaiterDto model)
            => DeserializeResponseModel<RestWaiterDto>(await Client.Post<Response>(Route + nameof(Create), model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<RestWaiterDto> Details(string token, int id)
        {
            var model = new RestWaiterDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<RestWaiterDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, RestWaiterDto model) =>
            DeserializeResponseModel<RestWaiterDto>(await Client.Post<Response>(Route + nameof(Edit), model, token: token));


        public async Task<RestWaiterDto> Get(string token, RestWaiterDto model = null)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new RestWaiterDto();
            model.Response = res;
            model.Waiters = JsonConvert.DeserializeObject<List<RestWaiterDto>>(res.Model.String());
            return model;
        }

        public async Task<IList<RestWaiter_SLM>> GetSelectList(string token, RestWaiterDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (IList<RestWaiter_SLM>) res.Model : new List<RestWaiter_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, RestWaiterDto model = null)
        {
            var res = await Client.Post<Response>($"{Route}GetSelectList", model ?? new RestWaiterDto(), token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<RestWaiter_SLM>>(res.Model.String());
            return res;
        }
    }
}
