using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.GoodsReceivedNoteServices
{
    public class GrnService : ServiceBase, IGrnService, IService
    {
        public GrnService(IClientManager clientManager) : base("api/GoodsReceivedNote/", clientManager){}

        public async Task<Response> Create(string token, InvGrnMasterDto model)
            => DeserializeResponseModel<InvGrnMasterDto>(await Client.Post<Response>(Route + nameof(Create), model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<InvGrnMasterDto> Details(string token, int id)
        {
            var model = new InvGrnMasterDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvGrnMasterDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, InvGrnMasterDto model)
            => DeserializeResponseModel<InvGrnMasterDto>(await Client.Post<Response>(Route + nameof(Edit), model, token: token));

        public async Task<InvGrnMasterDto> Get(string token, InvGrnMasterDto model = null)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null) url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");

            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new InvGrnMasterDto();
            model.Response = res;
            model.GoodsReceivedNotes = JsonConvert.DeserializeObject<IList<InvGrnMasterDto>>(res.Model.String());
            return model;
        }

        public async Task<IList<InvGrnMaster_SLM>> GetSelectList(string token, InvGrnMasterDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (IList<InvGrnMaster_SLM>) res.Model : new List<InvGrnMaster_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, InvGrnMasterDto model = null)
        {
            var res = await Client.Post<Response>($"{Route}GetSelectList", model ?? new InvGrnMasterDto(), token);
            if (res.Model != null)
            {
                res.Model = JsonConvert.DeserializeObject<IList<InvGrnMaster_SLM>>(res.Model.String());
            }
            return res;
        }
    }
}
