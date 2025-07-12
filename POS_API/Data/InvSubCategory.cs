using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvSubCategory
    {
        public InvSubCategory()
        {
            InvItem = new HashSet<InvItem>();
        }

        public int Id { get; set; }
        public string CategoryCode { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool? DisplayOnPos { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvCategory Category { get; set; }
        public virtual ICollection<InvItem> InvItem { get; set; }
    }
}
