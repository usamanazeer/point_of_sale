using Models;
using Models.DTO.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.UnitServices
{
    public class UnitService : ServiceBase, IUnitService, IService
    {

        public UnitService(IClientManager clientManager) : base("api/unit/", clientManager){}
        public async Task<Response> Create(string token, InvUnitDto model)
            => DeserializeResponseModel<InvUnitDto>(await Client.Post<Response>($"{Route}Create", model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<InvUnitDto> Details(string token, int id)
        {
            var model = new InvUnitDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvUnitDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, InvUnitDto model)
            => DeserializeResponseModel<InvUnitDto>(await Client.Post<Response>(Route + "Edit", model, token: token));

        public async Task<InvUnitDto> Get(string token, InvUnitDto model = null)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
                url.Append( $"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new InvUnitDto();
            model.Response = res;
            model.Units = JsonConvert.DeserializeObject<List<InvUnitDto>>(res.Model.String());
            return model;
        }
    }
}
