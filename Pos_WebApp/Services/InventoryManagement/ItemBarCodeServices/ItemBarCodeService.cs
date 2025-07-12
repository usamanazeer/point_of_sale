using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.ItemBarCodeServices
{
    public class ItemBarCodeService : ServiceBase, IItemBarCodeService, IService
    {
        public ItemBarCodeService(IClientManager clientManager) : base("api/itembarcode/", clientManager) {}


        public async Task<Response> Create(string token, InvItemBarCodeDto model)
            => DeserializeResponseModel<InvItemBarCodeDto>(response: await Client.Post<Response>(Route + "Create", model, token: token));


        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<InvItemBarCodeDto> Details(string token, int id)
        {
            var model = new InvItemBarCodeDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvItemBarCodeDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, InvItemBarCodeDto model)
            => DeserializeResponseModel<InvItemBarCodeDto>(await Client.Post<Response>(Route + "Edit", model, token: token));

        public async Task<InvItemBarCodeDto> Get(string token, InvItemBarCodeDto model = null)
        {
            var res = await Client.Post<Response>($"{Route}Get", model??new InvItemBarCodeDto(), token);
            model ??= new InvItemBarCodeDto();
            model.Response = res;
            model.ItemBarCodes = JsonConvert.DeserializeObject<List<InvItemBarCodeDto>>(res.Model.String());
            return model;
        }

        public async Task<IList<InvItemBarCode_SLM>> GetSelectList(string token, InvItemBarCodeDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (IList<InvItemBarCode_SLM>) res.Model : new List<InvItemBarCode_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, InvItemBarCodeDto model = null)
        {
            var url = $"{Route}GetSelectList";
            var res = await Client.Post<Response>(url, model ?? new InvItemBarCodeDto(), token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<InvItemBarCode_SLM>>(res.Model.String());
            return res;
        }
    }
}
