using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.Notifications;
using Models.DTO.UserManagement;

namespace POS_API.Repositories.UserManagement.RoleRepos
{
    public interface IRoleRepository
    {
        Task<RoleDto> Create(RoleDto model);
        Task<RoleDto> Active(int id);
        Task<bool> Delete(RoleDto model);
        Task<RoleDto> Get(SearchModel model);
        Task<IEnumerable<RoleDto>> GetAll(RoleDto model);
        Task<RoleDto> InActive(int id);
        Task<RoleDto> Edit(RoleDto model);
        Task<bool> IsExists(RoleDto model);
        Task<IList<NotiRoleNotificationDto>> GetRoleNotificationTypes(RoleDto roleDto);
    }
}
