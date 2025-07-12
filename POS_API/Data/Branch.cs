using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class Branch
    {
        public Branch()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public bool? IsMainBranch { get; set; }
        public int? CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Mobile { get; set; }
        public bool? ShowPhoneOnBill { get; set; }
        public bool? ShowMobileOnBill { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
