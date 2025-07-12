using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvPurchaseMaster
    {
        public InvPurchaseMaster()
        {
            AccBillPayment = new HashSet<AccBillPayment>();
            InvPurchaseDetail = new HashSet<InvPurchaseDetail>();
        }

        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string BillNo { get; set; }
        public DateTime? BillDueDate { get; set; }
        public double BillAmount { get; set; }
        public double AmountPaid { get; set; }
        public int? VendorId { get; set; }
        public int BillStatusId { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual AccBillStatus BillStatus { get; set; }
        public virtual InvVendor Vendor { get; set; }
        public virtual ICollection<AccBillPayment> AccBillPayment { get; set; }
        public virtual ICollection<InvPurchaseDetail> InvPurchaseDetail { get; set; }
    }
}
