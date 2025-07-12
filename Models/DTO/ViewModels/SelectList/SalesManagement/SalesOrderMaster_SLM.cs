namespace Models.DTO.ViewModels.SelectList.SalesManagement
{
    // ReSharper disable once InconsistentNaming
    public class SalesOrderMaster_SLM : SelectListModel
    {
        public double? DiscountAmount { get; set; }
        public bool? IsDiscountInPercent { get; set; }
        public int? OrderTypeId { get; set; }
        public int? DiningTableId { get; set; }
        public int? WaiterId { get; set; }
    }
}
