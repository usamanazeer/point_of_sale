using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Models.DTO.InventoryManagement;
using Models.Enums;

namespace Models.DTO.Accounts
{
    public class BillDto : ExtendedBaseModel
    {
        public BillDto()
        {
            InvPurchaseDetail = new List<InvPurchaseDetailDto>();
        }


        [DisplayName("Bill Date")] public DateTime? BillDate { get; set; }
        [DisplayName("Due Date")] public DateTime? BillDueDate { get; set; }
        public DateTime? FromDueDate { get; set; }
        public DateTime? ToDueDate { get; set; }
        public decimal BillAmount { get; set; }//=> InvPurchaseDetail.Sum(x => x.Quantity * x.PurchaseRate);
        public decimal AmountPaid { get; set; }//=> BillPayments?.Where(x=>x.Status == StatusTypes.Active.ToInt()).Sum(x => x.TotalAmount) ?? 0;
        public decimal RemainingAmount => BillAmount - AmountPaid;
        public string BillNo { get; set; }
        public int? VendorId { get; set; }
        public int BillStatusId { get; set; }

        public string BillStatusText => 
            BillStatusId == AccBillStatus.Paid.ToInt() ? "Paid" : 
            BillStatusId == AccBillStatus.Unpaid.ToInt() ? "Unpaid" :
            BillStatusId == AccBillStatus.PartiallyPaid.ToInt() ? "Partially Paid" : "";
        public bool ExcludePaidBills { get; set; }
        
        public InvVendorDto Vendor { get; set; }
        public IList<InvPurchaseDetailDto> InvPurchaseDetail { get; set; }
        public List<BillDto> Bills { get; set; }

        public IList<AccBillPaymentDto> BillPayments { get; set; }
        public AccBillPaymentDto BillPayment { get; set; }
    }
}