using System;
using System.Collections.Generic;

namespace Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory
{
    public class InvPhysicalInventoryViewDto
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
        public decimal? Quantity { get; set; }
        public decimal? RemainingQuantity { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? SalesRate { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemFullName { get; set; }
        public string ItemMeasurement { get; set; }
        public bool? ItemDisplayOnPos { get; set; }
        public bool? ItemIsDeal { get; set; }
        public decimal? ItemMinimumQuantity { get; set; }
        public decimal? DefaultPurchaseRate { get; set; }
        public decimal? DefaultSalesRate { get; set; }
        public decimal? DefaultDiscountAmount { get; set; }
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
        public decimal? TaxAmount { get; set; }
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
        //public int BranchId { get; set; }
        //public string BranchName { get; set; }
        public int CompanyId { get; set; }

        //Dto prop
        public IList<InvPhysicalInventoryViewDto> PhysicalInventoryViews { get; set; }
        public Response Response { get; set; }

        public InvPhysicalInventoryViewDto Clone()
        {
            return (InvPhysicalInventoryViewDto)this.MemberwiseClone();
        }
    }
}
