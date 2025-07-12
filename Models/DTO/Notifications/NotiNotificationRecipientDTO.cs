using System;
using Models.DTO.UserManagement;

namespace Models.DTO.Notifications
{
    public class NotiNotificationRecipientDto
    {
        public int Id { get; set; }
        public int? RecipientId { get; set; }
        public int? NotificationId { get; set; }
        public bool? IsSeen { get; set; }
        public DateTime? SeenTime { get; set; }
        public int? CompanyId { get; set; }

        public virtual NotiNotificationDto Notification { get; set; }
        public virtual UserDto Recipient { get; set; }
    }
}
