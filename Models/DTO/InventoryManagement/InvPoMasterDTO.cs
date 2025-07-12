using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvPoMasterDto : ExtendedBaseModel
    {
        public InvPoMasterDto()
        {
            InvPoDetails = new List<InvPoDetailsDto>();

            PurchaseOrders = new List<InvPoMasterDto>();
        }

        [DisplayName("PO No")]
        public string PoNo { get; set; }

        [DisplayName("PO Date")]

        public DateTime? PoDate { get; set; }

        [DisplayName("Delivery Date")]
        public DateTime? DeliveryDate { get; set; }

        [DisplayName("Supplier")]
        public int? VendorId { get; set; }
        public string Description { get; set; }

        public InvVendorDto Vendor { get; set; }
        public IList<InvPoDetailsDto> InvPoDetails { get; set; }
        public IList<InvGrnDetailsDto> InvGrnDetails { get; set; }

        //DTO props
        public IList<InvPoMasterDto> PurchaseOrders { get; set; }

    }
}
