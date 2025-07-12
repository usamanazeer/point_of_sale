using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.PurchaseOrderServices
{
    // ReSharper disable once InconsistentNaming
    public class POService : ServiceBase, IPOService, IService
    {

        public POService(IClientManager clientManager) : base("api/PurchaseOrder/", clientManager){}
        public async Task<Response> Create(string token, InvPoMasterDto model)
            => DeserializeResponseModel<InvPoMasterDto>(await Client.Post<Response>(Route + nameof(Create), model, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }



        public async Task<Response> GetDetailsResponse(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                response.Model = JsonConvert.DeserializeObject<InvPoMasterDto>(response.Model.String());
            return response;
        }
        public async Task<InvPoMasterDto> Details(string token, int id)
        {
            var model = new InvPoMasterDto();
            var response = await GetDetailsResponse(token, id);
            if (response.Model != null)
                model = (InvPoMasterDto)(response.Model);
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, InvPoMasterDto model)
            => DeserializeResponseModel<InvPoMasterDto>(await Client.Post<Response>(Route + nameof(Edit),model, token: token));

        public async Task<InvPoMasterDto> Get(string token, InvPoMasterDto model = null)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
                url.Append( $"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new InvPoMasterDto();
            model.Response = res;
            model.PurchaseOrders = JsonConvert.DeserializeObject<IList<InvPoMasterDto>>(res.Model.String());
            return model;
        }

        public async Task<IList<InvPoMaster_SLM>> GetSelectList(string token, InvPoMasterDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (IList<InvPoMaster_SLM>) res.Model : new List<InvPoMaster_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, InvPoMasterDto model = null)
        {
            var res = await Client.Post<Response>($"{Route}GetSelectList", model ?? new InvPoMasterDto(), token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<InvPoMaster_SLM>>(res.Model.String());
            return res;
        }
    }
}
