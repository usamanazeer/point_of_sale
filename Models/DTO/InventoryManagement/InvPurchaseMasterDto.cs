using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Models.DTO.Accounts;

namespace Models.DTO.InventoryManagement
{
    public class InvPurchaseMasterDto:ExtendedBaseModel
    {
        public InvPurchaseMasterDto()
        {
            InvPurchaseDetail = new List<InvPurchaseDetailDto>();
            Purchases = new List<InvPurchaseMasterDto>();
        }
        [DisplayName("Purchase Date")]
        public DateTime? PurchaseDate { get; set; }
        [DisplayName("Bill Due Date")]
        public DateTime? BillDueDate { get; set; }
        public DateTime? FromDueDate { get; set; }
        public DateTime? ToDueDate { get; set; }
        public decimal BillAmount { get; set; } //=> InvPurchaseDetail.Sum(x => x.Quantity * x.PurchaseRate);
        public decimal AmountPaid { get; set; }
        public string BillNo { get; set; }
        public int? VendorId { get; set; }
        public int BillStatusId { get; set; }
        public InvVendorDto Vendor { get; set; }
        public IList<InvPurchaseDetailDto> InvPurchaseDetail { get; set; }
        public IList<InvPurchaseMasterDto> Purchases { get; set; }
        public IList<AccBillPaymentDto> AccBillPayment { get; set; }
    }
}
