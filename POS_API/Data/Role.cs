using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class Role
    {
        public Role()
        {
            NotiRoleNotification = new HashSet<NotiRoleNotification>();
            RoleRights = new HashSet<RoleRights>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public bool Editable { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<NotiRoleNotification> NotiRoleNotification { get; set; }
        public virtual ICollection<RoleRights> RoleRights { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
