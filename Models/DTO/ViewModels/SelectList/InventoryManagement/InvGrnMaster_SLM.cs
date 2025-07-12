using System;

namespace Models.DTO.ViewModels.SelectList.InventoryManagement
{    // ReSharper disable once InconsistentNaming
    public class InvGrnMaster_SLM : SelectListModel
    {
        public string GrnNo { get; set; }
        public DateTime GrnDate { get; set; }
        public string InvoiceNo { get; set; }
        public int VendorId { get; set; }
    }
}
