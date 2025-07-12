using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvPhysicalInventoryView
    {
        public int BillId { get; set; }
        public DateTime BillDate { get; set; }
        public string BillNo { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public int? ModifiedByUserId { get; set; }
        public string ModifiedByUserName { get; set; }
        public int BillItemId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public double? Quantity { get; set; }
        public double? RemainingQuantity { get; set; }
        public double? PurchaseRate { get; set; }
        public double? SalesRate { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemFullName { get; set; }
        public string ItemMeasurement { get; set; }
        public bool? ItemDisplayOnPos { get; set; }
        public bool? ItemIsDeal { get; set; }
        public double? ItemMinimumQuantity { get; set; }
        public double? DefaultPurchaseRate { get; set; }
        public double? DefaultSalesRate { get; set; }
        public double? DefaultDiscountAmount { get; set; }
        public bool? DefaultDiscountIsInPercent { get; set; }
        public string ItemImageUrl { get; set; }
        public bool? ItemAllowBackOrder { get; set; }
        public bool ManageStock { get; set; }
        public int? ItemType { get; set; }
        public string ItemTypeName { get; set; }
        public int? ItemBarCodeId { get; set; }
        public string ItemBarCode { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public int? TaxId { get; set; }
        public string TaxName { get; set; }
        public double? TaxAmount { get; set; }
        public bool? TaxIsInPercent { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryImageUrl { get; set; }
        public int? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategoryImageUrl { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        public int? SizeId { get; set; }
        public string SizeName { get; set; }
        public int? ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public string UnitDescription { get; set; }
        public int CompanyId { get; set; }
    }
}
