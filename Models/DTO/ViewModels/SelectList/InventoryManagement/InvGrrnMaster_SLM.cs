using System;

namespace Models.DTO.ViewModels.SelectList.InventoryManagement
{
    // ReSharper disable once InconsistentNaming
    public class InvGrrnMaster_SLM : SelectListModel
    {
        public string GrrnNo { get; set; }
        public DateTime GrrnDate { get; set; }
        public string InvoiceNo { get; set; }
        public int VendorId { get; set; }
    }
}
