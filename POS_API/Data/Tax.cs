using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class Tax
    {
        public Tax()
        {
            InvItem = new HashSet<InvItem>();
            InvPhysicalInventoryItem = new HashSet<InvPhysicalInventoryItem>();
            SalesOrderBilling = new HashSet<SalesOrderBilling>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsInPercent { get; set; }
        public bool? EnableForPos { get; set; }
        public int AccountId { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<InvItem> InvItem { get; set; }
        public virtual ICollection<InvPhysicalInventoryItem> InvPhysicalInventoryItem { get; set; }
        public virtual ICollection<SalesOrderBilling> SalesOrderBilling { get; set; }
    }
}
