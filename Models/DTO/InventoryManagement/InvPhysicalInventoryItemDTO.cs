﻿using Models.DTO.GeneralSettings;
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
        public double? Quantity { get; set; }
        public double? RemainingQuantity { get; set; }
        public int? TaxId { get; set; }
        public double? PurchaseRate { get; set; }
        public double? SalesRate { get; set; }

        public virtual InvItemDto Item { get; set; }
        public virtual InvItemBarCodeDto BarCode { get; set; }
        public virtual InvPhysicalInventoryDto PhysicalInventory { get; set; }
        public virtual TaxDto Tax { get; set; }
        public virtual InvVendorDto Vendor { get; set; }
    }
}