using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.Notifications;
using Models.DTO.UserManagement;
using Models.Enums;
using POS_API.Repositories.UserManagement.RoleRepos;

namespace POS_API.Services.UserManagement.RolesServices
{
    public class RolesService : IRolesService, IService
    {
        private readonly IRoleRepository _roleRepository;
        public RolesService(IRoleRepository roleRepository) => _roleRepository = roleRepository;

        public async Task<RoleDto> Active(int id) => await _roleRepository.Active(id);

        public async Task<RoleDto> Create(RoleDto role)
        {
            role.Editable = true;
            foreach (var right in role.RoleRights)
            {
                right.CompanyId = role.CompanyId;
                right.Status = StatusTypes.Active.ToInt();
                right.CreatedBy = role.CreatedBy;
                right.CreatedOn = role.CreatedOn;
            }
            foreach (var roleNotification in role.NotiRoleNotification)
            {
                roleNotification.CompanyId = role.CompanyId;
                roleNotification.Status = StatusTypes.Active.ToInt();
                roleNotification.CreatedBy = role.CreatedBy;
                roleNotification.CreatedOn = role.CreatedOn;
            }
            return await _roleRepository.Create(role);
        }

        public async Task<bool> Delete(RoleDto model) => await _roleRepository.Delete(model);

        public async Task<RoleDto> Edit(RoleDto model) => await _roleRepository.Edit(model);

        public async Task<IEnumerable<RoleDto>> GetAll(RoleDto model) => await _roleRepository.GetAll( model);

        public async Task<IList<NotiRoleNotificationDto>> GetRoleNotificationTypes(RoleDto roleDto) => await _roleRepository.GetRoleNotificationTypes(roleDto);

        public async Task<RoleDto> InActive(int id) => await _roleRepository.InActive(id);

        public async Task<bool> IsExists(RoleDto model) => await _roleRepository.IsExists(model);
    }
}
