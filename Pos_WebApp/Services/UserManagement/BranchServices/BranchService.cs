using Models;
using Models.DTO.UserManagement;
using Models.DTO.ViewModels.SelectList.UserManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.UserManagement.BranchServices
{
    public class BranchService : ServiceBase, IBranchService, IService
    {
        public BranchService(IClientManager clientManager) : base("api/branch/", clientManager)
        {
            
        }
        public async Task<BranchDto> Get(string token, BranchDto model)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new BranchDto();
            model.Response = res;
            model.Branches = JsonConvert.DeserializeObject<List<BranchDto>>(res.Model.String());
            return model;
        }

        public async Task<BranchDto> Create(string token, BranchDto model)
        {
            var res = await Client.Post<Response>($"{Route}Create", model, token: token);
            if (res.Model != null)
                model = JsonConvert.DeserializeObject<BranchDto>(res.Model.String());
            model.Response = res;
            return model;
        }

        public async Task<BranchDto> Edit(string token, BranchDto model)
        {
            var response = await Client.Post<Response>($"{Route}Edit", model, token: token);
            if (response.Model != null)
            {
                model = JsonConvert.DeserializeObject<BranchDto>(response.Model.String());
            }
            model.Response = response;
            return model;
        }
        public async Task<BranchDto> Details(string token, int id)
        {
            var model = new BranchDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
            {
                model = JsonConvert.DeserializeObject<BranchDto>(response.Model.String());
            }
            model.Response = response;
            return model;
        }

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<IList<Branch_SLM>> GetSelectList(string token, BranchDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (IList<Branch_SLM>) res.Model : new List<Branch_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, BranchDto model = null)
        {
            var url = $"{Route}GetSelectList";
            var res = await Client.Post<Response>(url, model ?? new BranchDto(), token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<Branch_SLM>>(res.Model.String());
            return res;
        }
    }
}
