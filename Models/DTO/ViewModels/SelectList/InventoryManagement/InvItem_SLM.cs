namespace Models.DTO.ViewModels.SelectList.InventoryManagement
{
    // ReSharper disable once InconsistentNaming
    public class InvItem_SLM : SelectListModel
    {
        public string BarCode { get; set; }
        public string FullName { get; set; }
        public string Measurement { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public string SalesRate { get; set; }
        public string PurchaseRate { get; set; }
        public string FinalSalesRate { get; set; }
    }
}
