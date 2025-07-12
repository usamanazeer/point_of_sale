using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.InventoryManagement.ColorServices
{
    public class ColorService : ServiceBase, IColorService, IService
    {
        public ColorService(IClientManager clientManager) : base("api/color/", clientManager){}

        public async Task<Response> Create(string token,InvColorDto model)
            => DeserializeResponseModel<InvColorDto>(await Client.Post<Response>(url: $"{Route}Create", obj: model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>(url: $"{Route}Delete/{id}", token: token);
            response.Model = (bool) response.Model;
            return response;
        }

        public async Task<InvColorDto> Details(string token,int id)
        {
            var model = new InvColorDto();
            var response = await Client.Get<Response>(url: $"{Route}Details?id={id}",token: token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvColorDto>(value: response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, InvColorDto model)
            => DeserializeResponseModel<InvColorDto>(await Client.Post<Response>(url: Route + "Edit", obj: model, token: token));

        public async Task<InvColorDto> Get(string token, InvColorDto model = null)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token: token);
            model ??= new InvColorDto();
            model.Response = res;
            model.Colors = JsonConvert.DeserializeObject<List<InvColorDto>>(value: res.Model.String());
            return model;
        }
    }
}