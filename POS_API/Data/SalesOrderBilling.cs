using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class SalesOrderBilling
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? TaxId { get; set; }
        public decimal? TaxAmount { get; set; }
        public bool? IsTaxInPercent { get; set; }
        public decimal? TotalBillAmount { get; set; }
        public decimal? TotalAmountPaid { get; set; }
        public int? PaymentType { get; set; }
        public decimal? CashReceived { get; set; }
        public decimal? CashReturn { get; set; }
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
