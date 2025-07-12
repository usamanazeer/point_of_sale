using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.RestaurantManagement.DiningTableServices
{
    public class DiningTableService : ServiceBase, IDiningTableService, IService
    {

        public DiningTableService(IClientManager clientManager) : base("api/diningtable/", clientManager)
        {}

        public async Task<Response> Create(string token, RestDiningTableDto model)
            => DeserializeResponseModel<RestDiningTableDto>(await Client.Post<Response>(Route + nameof(Create),
                                                             model,
                                                             token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<RestDiningTableDto> Details(string token, int id)
        {
            var model = new RestDiningTableDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<RestDiningTableDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, RestDiningTableDto model)
            => DeserializeResponseModel<RestDiningTableDto>(await Client.Post<Response>(Route + nameof(Edit), model, token: token));

        public async Task<RestDiningTableDto> Get(string token, RestDiningTableDto model = null)
        {
            var url = new StringBuilder($"{Route}Get");
            if (model != null)
            {
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            }
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new RestDiningTableDto();
            model.Response = res;
            model.DiningTables = JsonConvert.DeserializeObject<List<RestDiningTableDto>>(res.Model.String());
            return model;
        }

        public async Task<IList<RestDiningTable_SLM>> GetSelectList(string token, RestDiningTableDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            if (res.Model != null)
            {
                return (IList<RestDiningTable_SLM>)res.Model;
            }
            return new List<RestDiningTable_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, RestDiningTableDto model = null)
        {
            var url = $"{Route}GetSelectList";
            var res = await Client.Post<Response>(url, model ?? new RestDiningTableDto(), token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<RestDiningTable_SLM>>(res.Model.String());
            return res;
        }


        public async Task<Response> ReleaseOrOccupy(string token, RestDiningTableDto model)
        {
            var response = await Client.Post<Response>($"{Route}ReleaseOrOccupy", model, token);
            response.Model = (bool)response.Model;
            return response;
        }
    }
}
