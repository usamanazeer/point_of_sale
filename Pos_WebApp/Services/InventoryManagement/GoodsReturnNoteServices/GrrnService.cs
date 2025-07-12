using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.GoodsReturnNoteServices
{
    public class GrrnService : ServiceBase, IGrrnService, IService
    {

        public GrrnService(IClientManager clientManager) : base("api/GoodsReturnNote/", clientManager){}
        public async Task<Response> Create(string token, InvGrrnMasterDto model)
            => DeserializeResponseModel<InvGrrnMasterDto>(await Client.Post<Response>(Route + nameof(Create), model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<InvGrrnMasterDto> Details(string token, int id)
        {
            var model = new InvGrrnMasterDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvGrrnMasterDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, InvGrrnMasterDto model)
            => DeserializeResponseModel<InvGrrnMasterDto>(await Client.Post<Response>(Route + nameof(Edit), model, token: token));

        public async Task<InvGrrnMasterDto> Get(string token, InvGrrnMasterDto model = null)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
            {
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            }
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new InvGrrnMasterDto();
            model.Response = res;
            model.GoodsRetrunNotes = JsonConvert.DeserializeObject<IList<InvGrrnMasterDto>>(res.Model.String());
            return model;
        }

        public async Task<IList<InvGrrnMaster_SLM>> GetSelectList(string token, InvGrrnMasterDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (IList<InvGrrnMaster_SLM>) res.Model : new List<InvGrrnMaster_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, InvGrrnMasterDto model = null)
        {
            var url = $"{Route}GetSelectList";
            var res = await Client.Post<Response>(url, model ?? new InvGrrnMasterDto(), token);
            if (res.Model != null)
            {
                res.Model = JsonConvert.DeserializeObject<IList<InvGrrnMaster_SLM>>(res.Model.String());
            }
            return res;
        }
    }
}
