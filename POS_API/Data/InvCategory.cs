using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvCategory
    {
        public InvCategory()
        {
            InvItem = new HashSet<InvItem>();
            InvSubCategory = new HashSet<InvSubCategory>();
        }

        public int Id { get; set; }
        public string CategoryCode { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool? DisplayOnPos { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<InvItem> InvItem { get; set; }
        public virtual ICollection<InvSubCategory> InvSubCategory { get; set; }
    }
}
