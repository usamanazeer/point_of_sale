using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.ModifierServices
{
    public class ModifierService : ServiceBase, IModifierService, IService
    {
        public ModifierService(IClientManager clientManager) : base("api/modifier/", clientManager) {}
        public async Task<Response> Create(string token, InvModifierDto model)
            => DeserializeResponseModel<InvModifierDto>(await Client.Post<Response>($"{Route}Create", model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<InvModifierDto> Details(string token, int id)
        {
            var model = new InvModifierDto();
            var response = await Client.Get<Response>(url: Route + "Details?id=" + id, token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvModifierDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, InvModifierDto model)
            => DeserializeResponseModel<InvModifierDto>(await Client.Post<Response>($"{Route}Edit", model, token: token));

        public async Task<InvModifierDto> Get(string token, InvModifierDto model = null)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new InvModifierDto();
            model.Response = res;
            model.Modifiers = JsonConvert.DeserializeObject<List<InvModifierDto>>(res.Model.String());
            return model;
        }

        public async Task<IList<InvModifier_SLM>> GetSelectList(string token, InvModifierDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (IList<InvModifier_SLM>) res.Model : new List<InvModifier_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, InvModifierDto model = null)
        {
            var res = await Client.Post<Response>($"{Route}GetSelectList", model ?? new InvModifierDto(), token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<InvModifier_SLM>>(res.Model.String());
            return res;
        }
    }
}
