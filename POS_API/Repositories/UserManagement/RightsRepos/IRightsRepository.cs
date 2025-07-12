using Models.DTO.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace POS_API.Repositories.UserManagement.RightsRepos
{
    public interface IRightsRepository
    {
        Task<RightsDto> Create(RightsDto model);
        Task<RightsDto> Edit(RightsDto model);
        Task<bool> ChangeStatus(RightsDto model);
        Task<IEnumerable<CompanyModulesDto>> GetAll(CompanyModulesDto model);
        Task<IEnumerable<RightModel>> GetByRole(RoleDto role);
        Task<bool> ClaimRight(RoleRightsDto model);
        Task<RightsDto> ClaimRight1(RoleRightsDto model);
    }
}
