using Models;
using Models.DTO.UserManagement;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.UserManagement.RightsServices
{
    public interface IRightsService
    {
        //Rights
        Task<Response> Get (string token, int? id = null, int? parentId = null, int? status = null);
        Task<RightModel> GetRightsByRole(string token, int? roleId);
        //Response ClaimRight(string token, RoleRightsDTO model);
        Response ClaimRight(string token, RoleRightsDto model);
    }
}
