using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.Notifications;
using Models.DTO.UserManagement;

namespace POS_API.Services.UserManagement.RolesServices
{
    public interface IRolesService
    {
        Task<RoleDto> Create(RoleDto role);
        Task<bool> Delete(RoleDto model);
        Task<RoleDto> InActive(int id);
        Task<RoleDto> Active(int id);
        Task<IEnumerable<RoleDto>> GetAll(RoleDto model);
        //Task<RoleDTO> Get(SearchModel model);
        Task<RoleDto> Edit(RoleDto model);
        Task<bool> IsExists(RoleDto model);
        Task<IList<NotiRoleNotificationDto>> GetRoleNotificationTypes(RoleDto roleDto);
    }
}
