using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class NotiNotificationType
    {
        public NotiNotificationType()
        {
            NotiNotification = new HashSet<NotiNotification>();
            NotiRoleNotification = new HashSet<NotiRoleNotification>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public string CssClasses { get; set; }
        public string Color { get; set; }
        public int? Sequence { get; set; }
        public string ReferenceTable { get; set; }
        public string ReferenceColumn { get; set; }

        public virtual ICollection<NotiNotification> NotiNotification { get; set; }
        public virtual ICollection<NotiRoleNotification> NotiRoleNotification { get; set; }
    }
}
