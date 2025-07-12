using System;

namespace Models.DTO.InventoryManagement
{
    public class InvPurchaseDetailDto:BaseModel
    {
        public int PurchaseMasterId { get; set; }
        public int ItemId { get; set; }
        public int? BarCodeId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public double Quantity { get; set; }
        public double PurchaseRate { get; set; }
        public double? SalesRate { get; set; }

        public InvItemBarCodeDto BarCode { get; set; }
        public InvItemDto Item { get; set; }
        public InvPurchaseMasterDto PurchaseMaster { get; set; }
    }
}
