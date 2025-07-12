using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class SalesOrderBilling
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? TaxId { get; set; }
        public double? TaxAmount { get; set; }
        public bool? IsTaxInPercent { get; set; }
        public double? TotalBillAmount { get; set; }
        public double? TotalAmountPaid { get; set; }
        public int? PaymentType { get; set; }
        public double? CashReceived { get; set; }
        public double? CashReturn { get; set; }
        public int Status { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual SalesOrderMaster Order { get; set; }
        public virtual Tax Tax { get; set; }
    }
}
