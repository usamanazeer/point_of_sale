using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvPhysicalInventory
    {
        public InvPhysicalInventory()
        {
            InvPhysicalInventoryItem = new HashSet<InvPhysicalInventoryItem>();
        }

        public int Id { get; set; }
        public DateTime BillDate { get; set; }
        public string BillNo { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<InvPhysicalInventoryItem> InvPhysicalInventoryItem { get; set; }
    }
}
