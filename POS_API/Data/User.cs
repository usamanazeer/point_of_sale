using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class User
    {
        public User()
        {
            NotiNotificationRecipient = new HashSet<NotiNotificationRecipient>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? Gender { get; set; }
        public string Phone { get; set; }
        public string PrimaryEmail { get; set; }
        public string OtherEmail { get; set; }
        public int CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int? RoleId { get; set; }
        public bool MainUser { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<NotiNotificationRecipient> NotiNotificationRecipient { get; set; }
    }
}
