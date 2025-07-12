using Models;
using Models.DTO.UserManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Pos_WebApp.Services.UserManagement.RightsServices
{
    public class RightsService : ServiceBase, IRightsService, IService
    {

        public RightsService(IClientManager clientManager) : base("api/rights/", clientManager)
        {}

        //Rights
        public async Task<Response> Get(string token, int? id = null, int? parentId = null, int? status = null)
        {
            var response = await Client.Get<Response>($"{Route}Get?id={id}&parentId={parentId}&status={status}", token);
            response.Model = JsonConvert.DeserializeObject<List<CompanyModulesDto>>(response.Model.String());
            return response;
        }

        public async Task<RightModel> GetRightsByRole(string token, int? roleId)
        {
            var model = new RightModel();
            var response = await Client.Get<Response>($"{Route}GetRightsByRole?id={roleId}", token);
            model.Rights = JsonConvert.DeserializeObject<List<RightModel>>(response.Model.String());
            //model.Response = response;
            return model;
        }

        public Response ClaimRight(string token, RoleRightsDto model)
        {
            var response = Client.Post<Response>($"{Route}ClaimRight", model, token: token).Result;
            if (response.Model  == null)
                return response;
            response.Model = JsonConvert.DeserializeObject<RightsDto>(response.Model.String());
            return response;
        }
    }
}
