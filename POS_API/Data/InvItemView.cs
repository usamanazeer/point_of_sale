using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvItemView
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Measurement { get; set; }
        public int CompanyId { get; set; }
        public bool? DisplayOnPos { get; set; }
        public bool ManageStock { get; set; }
        public int? ItemType { get; set; }
        public string ItemTypeName { get; set; }
        public bool IsReturnable { get; set; }
        public bool? IsDeal { get; set; }
        public bool? IsRecipe { get; set; }
        public bool? IsRawItem { get; set; }
        public double? MinimumQuantity { get; set; }
        public double? PurchaseRate { get; set; }
        public double? SalesRate { get; set; }
        public double? DiscountAmount { get; set; }
        public bool? IsDiscountInPercent { get; set; }
        public double? FinalSalesRate { get; set; }
        public string ImageUrl { get; set; }
        public bool? AllowBackOrder { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? BarCodeId { get; set; }
        public string BarCode { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImageUrl { get; set; }
        public bool? CategoryDisplayOnPos { get; set; }
        public int? CategoryStatus { get; set; }
        public int? SubCategoryId { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryImageUrl { get; set; }
        public bool? SubCategoryDisplayOnPos { get; set; }
        public int? SubCategoryStatus { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public string UnitDescription { get; set; }
        public int? UnitStatus { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        public int? BrandStatus { get; set; }
        public int? ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
        public int? ColorStatus { get; set; }
        public int? SizeId { get; set; }
        public string SizeName { get; set; }
        public int? SizeStatus { get; set; }
        public int? TaxId { get; set; }
        public string TaxName { get; set; }
        public double? TaxAmount { get; set; }
        public bool? TaxIsInPercent { get; set; }
        public int? TaxStatus { get; set; }
        public int? CreatedById { get; set; }
        public string CreatedByFirstName { get; set; }
        public string CreatedByLastName { get; set; }
        public int? ModifiedById { get; set; }
        public string ModifiedByFirstName { get; set; }
        public string ModifiedByLastName { get; set; }
        public double RemainingInventory { get; set; }
    }
}
