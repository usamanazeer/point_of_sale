using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvColor
    {
        public InvColor()
        {
            InvItem = new HashSet<InvItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorValue { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<InvItem> InvItem { get; set; }
    }
}
