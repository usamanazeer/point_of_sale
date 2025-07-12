using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class SalesOrderItemModifiers
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int? ModifierId { get; set; }
        public double? Quantity { get; set; }
        public double? Charges { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvModifier Modifier { get; set; }
        public virtual SalesOrderDetails OrderItem { get; set; }
    }
}
