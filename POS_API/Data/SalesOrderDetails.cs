using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class SalesOrderDetails
    {
        public SalesOrderDetails()
        {
            SalesOrderItemModifiers = new HashSet<SalesOrderItemModifiers>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal SalesRate { get; set; }
        public decimal? DiscountAmount { get; set; }
        public bool? IsDiscountInPercent { get; set; }
        public int? TaxId { get; set; }
        public decimal? TaxAmount { get; set; }
        public bool? IsTaxInPercent { get; set; }
        public decimal FinalSalesRate { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvItem Item { get; set; }
        public virtual SalesOrderMaster Order { get; set; }
        public virtual ICollection<SalesOrderItemModifiers> SalesOrderItemModifiers { get; set; }
    }
}
