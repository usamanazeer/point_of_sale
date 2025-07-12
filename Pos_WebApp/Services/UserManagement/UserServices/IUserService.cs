using Models;
using Models.DTO.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.UserManagement.UserServices
{
    public interface IUserService
    {
        //Task<UserDTO> Register(UserDTO user);
        Task<Response> Login(AuthenticationDto user);
        Task<bool> IsExist(UserDto user, string token);
        Task<Response> Get(string token, int? id = null, int? status = null, bool? getDeleted = null);
        Task<UserDto> GetById(int id, string token);
        Task<Response> Edit( string token, UserDto user);
        Task<Response> Create(UserDto user, string token);
        Task<Response> Delete(string token, int id);
    }
}
