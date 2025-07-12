using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class AccBillPayment
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int PaymentTypeId { get; set; }
        public double? CashAmount { get; set; }
        public double? ChequeAmount { get; set; }
        public int? BankAccountId { get; set; }
        public string ChequeNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvPurchaseMaster Bill { get; set; }
        public virtual AccBillPaymentType PaymentType { get; set; }
    }
}
