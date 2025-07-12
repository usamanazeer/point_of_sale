using Models.DTO.UserManagement;

namespace Models.DTO.Notifications
{
    public class NotiRoleNotificationDto : BaseModel
    {

        public int? RoleId { get; set; }
        public int? NotificationTypeId { get; set; }

        public virtual NotiNotificationTypeDto NotificationType { get; set; }
        public virtual RoleDto Role { get; set; }
    }
}
