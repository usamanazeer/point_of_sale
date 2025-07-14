using System.ComponentModel;
using Models.DTO.GeneralSettings;
using Models.DTO.SalesManagement;
using System.Collections.Generic;
using Models.DTO.InventoryManagement.ViewDTO;
using ItemTypesEnum = Models.Enums.ItemTypes;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvItemDto : ExtendedBaseModel
    {
        public InvItemDto()
        {
            InvGrnDetails = new List<InvGrnDetailsDto>();
            InvItemBarCode = new List<InvItemBarCodeDto>();
            //InvItemRecipeItem = new HashSet<InvItemRecipeDTO>();
            InvItemRecipeChild = new List<InvItemRecipeDto>();
            InvPhysicalInventoryItem = new List<InvPhysicalInventoryItemDto>();
            InvItemModifiers = new List<InvItemModifierDto>();
            SalesOrderDetails = new List<SalesOrderDetailsDto>();
            //dto prop
            Items = new List<InvItemDto>();
            Response = new Response();
        }

        [DisplayName("Item Code")]
        public string ItemCode { get; set; }

        public string Name { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        public string Measurement { get; set; }

        [DisplayName("Unit")]
        public int? UnitId { get; set; }

        [DisplayName("Brand")]
        public int? BrandId { get; set; }

        [DisplayName("Size")]
        public int? SizeId { get; set; }

        [DisplayName("Color")]
        public int? ColorId { get; set; }

        [DisplayName("Sub-Category")]
        public int? SubCategoryId { get; set; }

        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [DisplayName("Display On POS")]
        public bool DisplayOnPos { get; set; }
        public string DisplayOnPosText => DisplayOnPos == false ? "No" : "Yes";

        [DisplayName("Allow Back Order")]
        public bool ManageStock { get; set; }
        public string ManageStockText => ManageStock == false ? "No" : "Yes";
        public int ItemType { get; set; }
        public string ItemTypeText => GetTypeName();

        [DisplayName("Is Returnable")]
        public bool IsReturnable { get; set; }
        public string IsReturnableText => IsReturnable == false ? "No" : "Yes";

        [DisplayName("Is Deal")]
        public bool IsDeal { get; set; }
        public string IsDealText => IsDeal == false ? "No" : "Yes";

        [DisplayName("Is Deal")]
        public bool IsRecipe { get; set; }
        public string IsRecipeText => IsRecipe == false ? "No" : "Yes";

        [DisplayName("Is Raw Item")]
        public bool IsRawItem { get; set; }
        public string IsRawItemText => IsRawItem == false ? "No" : "Yes";

        public int? ParentItemId { get; set; }

        [DisplayName("Minimum Quantity")]
        public decimal? MinimumQuantity { get; set; }

        [DisplayName("Purchase Rate")]
        public decimal? PurchaseRate { get; set; }

        [DisplayName("Sales Rate")]
        public decimal? SalesRate { get; set; }

        //[DisplayName("Final Sales Rate")]
        //public decimal? FinalSalesRate => CalculateFinalSalesRate();

        [DisplayName("Final Sales Rate")]
        public decimal? FinalSalesRate { get; set; }

        [DisplayName("Discount Amount")]
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountedAmount => GetDiscountedAmount();

        [DisplayName("Discount In Percent")]
        public bool IsDiscountInPercent { get; set; }
        public string IsDiscountInPercentText => IsDiscountInPercent == false ? "No" : "Yes";

        [DisplayName("Tax")]
        public int? TaxId { get; set; }

        [DisplayName("Display Image")]
        public string ImageUrl { get; set; }

        //[DisplayName("Allow Back Order")]
        //public bool AllowBackOrder { get; set; }
        //public string AllowBackOrderText => AllowBackOrder == false ? "No" : "Yes";

        [DisplayName(displayName: "Account No")]
        public string AccountNo { get; set; }
        public int AssAccountId { get; set; }
        public int RevAccountId { get; set; }

        public InvBrandDto Brand { get; set; }

        public InvCategoryDto Category { get; set; }

        public InvColorDto Color { get; set; }

        public InvSizeDto Size { get; set; }

        public InvSubCategoryDto SubCategory { get; set; }

        public InvUnitDto Unit { get; set; }

        public TaxDto Tax { get; set; }
        public IList<InvGrnDetailsDto> InvGrnDetails { get; set; }
        public IList<InvItemBarCodeDto> InvItemBarCode { get; set; }
        //public virtual IList<InvItemRecipeDTO> InvItemRecipeItem { get; set; }
        public IList<InvItemRecipeDto> InvItemRecipeChild { get; set; }
        public IList<InvItemModifierDto> InvItemModifiers { get; set; }

        public IList<InvPhysicalInventoryItemDto> InvPhysicalInventoryItem { get; set; }
        public IList<InvPoDetailsDto> InvPoDetails { get; set; }
        public ICollection<SalesOrderDetailsDto> SalesOrderDetails { get; set; }


        //DTO prop
        [DisplayName("Bar Code")]
        public string ItemBarCode { get; set; }
        public List<InvItemDto> Items { get; set; }
        public IList<InvItemViewDto> ViewList { get; set; }

        public IList<int> ExceptIDs { get; set; }
        public bool ExceptDealItems { get; set; }
        public bool ExceptRecipeItems { get; set; }
        public bool ExceptRawItems { get; set; }
        public int[] ItemTypesFilter { get; set; }

        public string SearchText { get; set; }
        public decimal? GetDiscountedAmount()
        {
            var result = DiscountAmount;
            if (IsDiscountInPercent)
            {
                var discountInPercent = (result * DiscountAmount) / 100;
                result = discountInPercent;
            }
            return result;
        }
        public string GetTypeName()
        {
            if (ItemType == (int)ItemTypesEnum.DealItem)
            {
                return "Deal";
            }
            if (ItemType == (int)ItemTypesEnum.DealItem)
            {
                return "Recipe";
            }
            if (ItemType == (int)ItemTypesEnum.RawItem)
            {
                return "Raw Item";
            }
            return "Item";
        }
    }
}