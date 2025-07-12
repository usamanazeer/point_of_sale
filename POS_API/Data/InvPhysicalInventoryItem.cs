using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvPhysicalInventoryItem
    {
        public int Id { get; set; }
        public int? PhysicalInventoryId { get; set; }
        public int? ItemId { get; set; }
        public int? BarCodeId { get; set; }
        public int? VendorId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public double? Quantity { get; set; }
        public double? RemainingQuantity { get; set; }
        public int? TaxId { get; set; }
        public double? PurchaseRate { get; set; }
        public double? SalesRate { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvItemBarCode BarCode { get; set; }
        public virtual InvItem Item { get; set; }
        public virtual InvPhysicalInventory PhysicalInventory { get; set; }
        public virtual Tax Tax { get; set; }
        public virtual InvVendor Vendor { get; set; }
    }
}
