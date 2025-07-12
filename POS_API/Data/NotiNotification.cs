using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class NotiNotification
    {
        public NotiNotification()
        {
            NotiNotificationRecipient = new HashSet<NotiNotificationRecipient>();
        }

        public int Id { get; set; }
        public int? TypeId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public int? Status { get; set; }
        public int CompanyId { get; set; }
        public int? ReferenceKey { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual NotiNotificationType Type { get; set; }
        public virtual ICollection<NotiNotificationRecipient> NotiNotificationRecipient { get; set; }
    }
}
