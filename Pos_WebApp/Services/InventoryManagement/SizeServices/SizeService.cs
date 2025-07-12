using Models;
using Models.DTO.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.SizeServices
{
    public class SizeService : ServiceBase, ISizeService, IService
    {

        public SizeService(IClientManager clientManager) : base("api/size/", clientManager){}

        public async Task<Response> Create(string token, InvSizeDto model)
            => DeserializeResponseModel<InvSizeDto>(await Client.Post<Response>($"{Route}Create", model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<InvSizeDto> Details(string token, int id)
        {
            var model = new InvSizeDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvSizeDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, InvSizeDto model)
            => DeserializeResponseModel<InvSizeDto>(await Client.Post<Response>($"{Route}Edit", model, token: token));

        public async Task<InvSizeDto> Get(string token, InvSizeDto model = null)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
                url.Append( $"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new InvSizeDto();
            model.Response = res;
            model.Sizes = JsonConvert.DeserializeObject<List<InvSizeDto>>(res.Model.String());
            return model;
        }
    }
}
