using Models;
using Models.DTO.UserManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.UserManagement.UserServices
{
    public class UserService : ServiceBase, IUserService, IService
    {
        public UserService(IClientManager clientManager) : base("api/user/", clientManager)
        {}
        public async Task<Response> Create(UserDto user, string token)
        {
            var response = await Client.Post<Response>($"{Route}Create", user, token: token);
            response.Model = JsonConvert.DeserializeObject<UserDto>(response.Model.String());
            return response;
        }

        public async Task<Response> Edit(string token, UserDto user)
        {
            var response = await Client.Post<Response>($"{Route}Edit", user, token: token);
            response.Model = JsonConvert.DeserializeObject<UserDto>(response.Model.String());
            return response;
        }

        public async Task<Response> Get(string token, int? id = null, int? status = null, bool? getDeleted = null)
        {
            var response = await Client.Get<Response>($"{Route}Get?id={id}&status={status}&getDeleted={getDeleted}", token);
            response.Model = JsonConvert.DeserializeObject<List<UserDto>>(response.Model.String());
            return response;
        }

        public async Task<UserDto> GetById(int id, string token) => await Client.Get<UserDto>($"{Route}GetById?id={id}", token);

        public Task<bool> IsExist(UserDto user, string token) => throw new NotImplementedException();


        public async Task<Response> Login(AuthenticationDto user)
        {
            var response = await Client.Post<Response>($"{Route}Login", user);
            response.Model = JsonConvert.DeserializeObject<AuthenticationDto>(response.Model.String());
            return response;
        }
        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }
    }
}
