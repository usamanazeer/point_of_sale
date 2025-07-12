using Models.DTO.Notifications;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.UserManagement
{
    public sealed class RoleDto : BaseModel
    {
        public RoleDto()
        {

            Roles = new List<RoleDto>();
            NotiRoleNotification = new List<NotiRoleNotificationDto>();
            //DB PROPS
            RoleRights = new List<RoleRightsDto>();
            User = new List<UserDto>();
        }
        [DisplayName("Role Name")]
        public string Name { get; set; }
        //public int CompanyId { get; set; }
        public string Description { get; set; }
        public bool Editable { get; set; }

        public IList<RoleRightsDto> RoleRights { get; set; }
        public IList<UserDto> User { get; set; }

        public IList<NotiRoleNotificationDto> NotiRoleNotification { get; set; }

        public IList<RoleDto> Roles { get; set; }
        public IList<CompanyModulesDto> CompanyModules { get; set; }
    }
}
