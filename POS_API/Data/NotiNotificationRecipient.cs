using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class NotiNotificationRecipient
    {
        public int Id { get; set; }
        public int? RecipientId { get; set; }
        public int? NotificationId { get; set; }
        public bool? IsSeen { get; set; }
        public DateTime? SeenTime { get; set; }
        public int CompanyId { get; set; }

        public virtual NotiNotification Notification { get; set; }
        public virtual User Recipient { get; set; }
    }
}
