namespace POS_API.Data.TVPs
{
    public class BulkUploadItemsTvp
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string BarCode { get; set; }
        public string Measurement { get; set; }
        public string UnitName { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal SalesRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsDiscountInPercentage { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public decimal MinimumQuantity { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public bool IsReturnable { get; set; }
        public bool DisplayOnPos { get; set; }
        public bool IsRawItem { get; set; }
        public bool AllowBackOrder { get; set; }
    }
}
