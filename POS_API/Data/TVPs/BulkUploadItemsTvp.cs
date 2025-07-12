namespace POS_API.Data.TVPs
{
    public class BulkUploadItemsTvp
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string BarCode { get; set; }
        public string Measurement { get; set; }
        public string UnitName { get; set; }
        public double PurchaseRate { get; set; }
        public double SalesRate { get; set; }
        public double DiscountAmount { get; set; }
        public bool IsDiscountInPercentage { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public double MinimumQuantity { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public bool IsReturnable { get; set; }
        public bool DisplayOnPos { get; set; }
        public bool IsRawItem { get; set; }
        public bool AllowBackOrder { get; set; }
    }
}
