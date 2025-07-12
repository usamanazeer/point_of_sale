namespace Models.DTO.ViewModels.SelectList.DeliveryService
{
    // ReSharper disable once InconsistentNaming
    public class DeliveryServiceVendor_SLM : SelectListModel
    {
        public bool IsSelf { get; set; }
        public double ServiceCharges { get; set; }
        public bool ChargesInPercent { get; set; }
    }
}
