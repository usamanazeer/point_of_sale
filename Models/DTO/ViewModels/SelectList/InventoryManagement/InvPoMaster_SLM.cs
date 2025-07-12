using System;

namespace Models.DTO.ViewModels.SelectList.InventoryManagement
{
    // ReSharper disable once InconsistentNaming
    public class InvPoMaster_SLM : SelectListModel
    {
        public string PoNo { get; set; }
        public DateTime PoDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int VendorId { get; set; }
        public string Description { get; set; }
    }
}
