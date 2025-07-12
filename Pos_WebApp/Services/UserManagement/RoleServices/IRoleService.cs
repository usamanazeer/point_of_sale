using Models;
using Models.DTO.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.UserManagement.RoleServices
{
    public interface IRoleService
    {
        Task<Response> Get(string token, int? id = null, int? status = null, bool? getDeleted = null);
        Task<Response> Create(RoleDto model, string token);
        Task<Response> Edit(string token, RoleDto user);
        Task<Response> Delete(string token, int id);
        Task<Response> Active(RoleDto model, string token);
        Task<Response> Inctive(RoleDto model, string token);
        Task<Response> GetDetails(string token, int id);
    }
}
