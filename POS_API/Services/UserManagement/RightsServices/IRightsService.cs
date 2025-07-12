using Models;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Services.UserManagement.RightsServices
{
    public interface IRightsService
    {
        Task<RightsDto> Create(RightsDto model);
        Task<RightsDto> Edit(RightsDto model);
        Task<bool> ChangeStatus(RightsDto model);
        Task<IEnumerable<CompanyModulesDto>> GetAll(CompanyModulesDto model);
        Task<IEnumerable<RightModel>> GetRightsByRole(RoleDto role);
        Task<Response> ClaimRight(RoleRightsDto model);
        Task<Response> ClaimRight1(RoleRightsDto model);
    }
}
