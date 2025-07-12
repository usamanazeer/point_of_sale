using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class NotiUserNotificationsView
    {
        public int NotificationId { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationMessage { get; set; }
        public string NotificationUrl { get; set; }
        public int? NotificationStatus { get; set; }
        public int? CompanyId { get; set; }
        public int? ReferenceKey { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string ReferenceTable { get; set; }
        public string ReferenceColumn { get; set; }
        public string NotificationTypeName { get; set; }
        public string NotificationCssClasses { get; set; }
        public string NotificationIcon { get; set; }
        public string NotificationColor { get; set; }
        public int? Sequence { get; set; }
        public int? RecipientId { get; set; }
        public bool? IsNotificationSeen { get; set; }
        public DateTime? NotificationSeenTime { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string RecipientUserName { get; set; }
        public bool? RecipientGender { get; set; }
        public string RecipientPrimaryEmail { get; set; }
        public string RecipientOtherEmail { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}
