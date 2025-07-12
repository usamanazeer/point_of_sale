using Models;
using Models.DTO.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.BrandServices
{
    public class BrandService : ServiceBase, IBrandService, IService
    {

        public BrandService(IClientManager clientManager) : base("api/brand/", clientManager)
        {
        }
        public async Task<Response> Create(string token, InvBrandDto model)
            => DeserializeResponseModel<InvBrandDto>(await Client.Post<Response>($"{Route}Create", model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>(Route + "Delete/" + id, token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<Response> Edit(string token, InvBrandDto model)
            => DeserializeResponseModel<InvBrandDto>(await Client.Post<Response>(Route + "Edit", model, token: token));

        public async Task<InvBrandDto> Get(string token, InvBrandDto model = null)
        {
            var url = new StringBuilder($"{Route}Get");
            if (model != null)
            {
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            }
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new InvBrandDto();
            model.Response = res;
            model.Brands = JsonConvert.DeserializeObject<List<InvBrandDto>>(res.Model.String());
            return model;
        }

        public async Task<InvBrandDto> Details(string token, int id)
        {
            var model = new InvBrandDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
            {
                model = JsonConvert.DeserializeObject<InvBrandDto>(response.Model.String());
            }
            model.Response = response;
            return model;
        }
    }
}
