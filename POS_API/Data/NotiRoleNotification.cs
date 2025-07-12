using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class NotiRoleNotification
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? NotificationTypeId { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual NotiNotificationType NotificationType { get; set; }
        public virtual Role Role { get; set; }
    }
}
