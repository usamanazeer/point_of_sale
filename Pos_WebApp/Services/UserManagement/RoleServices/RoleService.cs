using Models;
using Models.DTO.UserManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Pos_WebApp.Services.UserManagement.RoleServices
{
    public class RoleService : ServiceBase, IRoleService, IService
    {
        public RoleService(IClientManager clientManager) : base("api/roles/", clientManager)
        {}
        public async Task<Response> Get(string token, int? id = null, int? status = null, bool? getDeleted = null)
        {
            var response = await Client.Get<Response>($"{Route}Get?id={id}&status={status}&getDeleted={getDeleted}", token);
            response.Model = JsonConvert.DeserializeObject<List<RoleDto>>(response.Model.String());
            return response;
        }

        public async Task<Response> Create(RoleDto model, string token)
        {
            var response = await Client.Post<Response>($"{Route}Create",model, token: token);
            response.Model = JsonConvert.DeserializeObject<RoleDto>(response.Model.String());
            return response;
        }
        public async Task<Response> Edit(string token, RoleDto model)
        {
            var response = await Client.Post<Response>($"{Route}Edit", model, token: token);
            response.Model = JsonConvert.DeserializeObject<RoleDto>(response.Model.String());
            return response;
        }

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public Task<Response> Active(RoleDto model, string token) => throw new NotImplementedException();


        public Task<Response> Inctive(RoleDto model, string token) => throw new NotImplementedException();


        public async Task<Response> GetDetails(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}GetDetails/{id}",token);
            var data = JsonConvert.DeserializeObject<List<object>>(response.Model.String());
            response.Model = JsonConvert.DeserializeObject<List<object>>(response.Model.String());
            response.Model = new List<object> {
                JsonConvert.DeserializeObject<RoleDto>(data[0].String()),
                JsonConvert.DeserializeObject<List<CompanyModulesDto>>(data[1].String())
            };
            return response;
        }
    }
}
