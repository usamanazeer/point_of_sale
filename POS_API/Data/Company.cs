using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class Company
    {
        public Company()
        {
            Branch = new HashSet<Branch>();
            CompanyModules = new HashSet<CompanyModules>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int BusinessTypeId { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public string OnDeskPrinter { get; set; }
        public string OffDeskPrinter { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual BusinessType BusinessType { get; set; }
        public virtual ICollection<Branch> Branch { get; set; }
        public virtual ICollection<CompanyModules> CompanyModules { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
