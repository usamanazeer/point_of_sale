using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvItem
    {
        public InvItem()
        {
            InvGrnDetails = new HashSet<InvGrnDetails>();
            InvGrrnDetails = new HashSet<InvGrrnDetails>();
            InvItemBarCode = new HashSet<InvItemBarCode>();
            InvItemModifiers = new HashSet<InvItemModifiers>();
            InvItemRecipeItem = new HashSet<InvItemRecipe>();
            InvItemRecipeParent = new HashSet<InvItemRecipe>();
            InvModifierItems = new HashSet<InvModifierItems>();
            InvPhysicalInventoryItem = new HashSet<InvPhysicalInventoryItem>();
            InvPodetails = new HashSet<InvPodetails>();
            InvPurchaseDetail = new HashSet<InvPurchaseDetail>();
            SalesOrderDetails = new HashSet<SalesOrderDetails>();
        }

        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Measurement { get; set; }
        public int? UnitId { get; set; }
        public int? BrandId { get; set; }
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? CategoryId { get; set; }
        public int CompanyId { get; set; }
        public bool? DisplayOnPos { get; set; }
        public bool? ManageStock { get; set; }
        public int? ItemType { get; set; }
        public bool IsReturnable { get; set; }
        public bool? IsDeal { get; set; }
        public bool? IsRecipe { get; set; }
        public bool? IsRawItem { get; set; }
        public double? MinimumQuantity { get; set; }
        public double? PurchaseRate { get; set; }
        public double? SalesRate { get; set; }
        public double? DiscountAmount { get; set; }
        public bool? IsDiscountInPercent { get; set; }
        public int? TaxId { get; set; }
        public string ImageUrl { get; set; }
        public bool? AllowBackOrder { get; set; }
        public string AccountNo { get; set; }
        public int AssAccountId { get; set; }
        public int ExpAccountId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public double? FinalSalesRate { get; set; }

        public virtual InvBrand Brand { get; set; }
        public virtual InvCategory Category { get; set; }
        public virtual InvColor Color { get; set; }
        public virtual InvSize Size { get; set; }
        public virtual InvSubCategory SubCategory { get; set; }
        public virtual Tax Tax { get; set; }
        public virtual InvUnit Unit { get; set; }
        public virtual InvNegativeInventory InvNegativeInventory { get; set; }
        public virtual ICollection<InvGrnDetails> InvGrnDetails { get; set; }
        public virtual ICollection<InvGrrnDetails> InvGrrnDetails { get; set; }
        public virtual ICollection<InvItemBarCode> InvItemBarCode { get; set; }
        public virtual ICollection<InvItemModifiers> InvItemModifiers { get; set; }
        public virtual ICollection<InvItemRecipe> InvItemRecipeItem { get; set; }
        public virtual ICollection<InvItemRecipe> InvItemRecipeParent { get; set; }
        public virtual ICollection<InvModifierItems> InvModifierItems { get; set; }
        public virtual ICollection<InvPhysicalInventoryItem> InvPhysicalInventoryItem { get; set; }
        public virtual ICollection<InvPodetails> InvPodetails { get; set; }
        public virtual ICollection<InvPurchaseDetail> InvPurchaseDetail { get; set; }
        public virtual ICollection<SalesOrderDetails> SalesOrderDetails { get; set; }
    }
}
