using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class BusinessType
    {
        public BusinessType()
        {
            Company = new HashSet<Company>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Company> Company { get; set; }
    }
}
