using System;

namespace Models.DTO.InventoryManagement
{
    public class InvPurchaseDetailDto:BaseModel
    {
        public int PurchaseMasterId { get; set; }
        public int ItemId { get; set; }
        public int? BarCodeId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal? SalesRate { get; set; }

        public InvItemBarCodeDto BarCode { get; set; }
        public InvItemDto Item { get; set; }
        public InvPurchaseMasterDto PurchaseMaster { get; set; }
    }
}
