using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class DeliDeliveryServiceVendor
    {
        public DeliDeliveryServiceVendor()
        {
            SalesOrderMaster = new HashSet<SalesOrderMaster>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ServiceDiscount { get; set; }
        public bool IsServiceDiscountInPercent { get; set; }
        public bool IsSelf { get; set; }
        public string AccountNo { get; set; }
        public int? RecAccountId { get; set; }
        public int? ExpAccountId { get; set; }
        public int Status { get; set; }
        public int CompanyId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<SalesOrderMaster> SalesOrderMaster { get; set; }
    }
}
