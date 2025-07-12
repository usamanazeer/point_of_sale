
using System;
using Models.DTO.InventoryManagement;
using Models.Enums;

namespace Models.DTO.Accounts
{
    public class AccBillPaymentDto:ExtendedBaseModel
    {
        public int BillId { get; set; }
        public int PaymentTypeId { get; set; }
        public double TotalAmount => (CashAmount??0) + (ChequeAmount??0);
        public int? BankAccountId { get; set; }
        public string ChequeNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Remarks { get; set; }
        public double? CashAmount { get; set; }
        public double? ChequeAmount { get; set; }
        public InvPurchaseMasterDto Bill { get; set; }

        public string PaymentTypeText =>
            PaymentTypeId == AccBillPaymentType.Cash.ToInt() ? "Cash" :
            PaymentTypeId == AccBillPaymentType.Cheque.ToInt() ? "Cheque" :
            PaymentTypeId == AccBillPaymentType.Split.ToInt() ? "Split" : "";



    }
}
