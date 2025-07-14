using Models.DTO.GeneralSettings;
using System;

namespace Models.DTO.InventoryManagement
{
    public class InvPhysicalInventoryItemDto : BaseModel
    {

        public int? PhysicalInventoryId { get; set; }
        public int? ItemId { get; set; }
        public int? BarCodeId { get; set; }
        public int? VendorId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? RemainingQuantity { get; set; }
        public int? TaxId { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? SalesRate { get; set; }

        public virtual InvItemDto Item { get; set; }
        public virtual InvItemBarCodeDto BarCode { get; set; }
        public virtual InvPhysicalInventoryDto PhysicalInventory { get; set; }
        public virtual TaxDto Tax { get; set; }
        public virtual InvVendorDto Vendor { get; set; }
    }
}