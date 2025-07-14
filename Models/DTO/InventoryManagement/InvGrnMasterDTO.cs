using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvGrnMasterDto : ExtendedBaseModel
    {
        public InvGrnMasterDto()
        {
            InvGrnDetails = new List<InvGrnDetailsDto>();
        }

        [DisplayName("Grn No")]
        public string GrnNo { get; set; }

        [DisplayName("Grn Date")]
        public DateTime? GrnDate { get; set; }

        [DisplayName("Invoice No")]
        public string InvoiceNo { get; set; }

        [DisplayName("Supplier")]
        public int VendorId { get; set; }
        public string Description { get; set; }

        public InvVendorDto Vendor { get; set; }
        public IList<InvGrnDetailsDto> InvGrnDetails { get; set; }

        public IList<InvGrnMasterDto> GoodsReceivedNotes { get; set; }

        public decimal GetTotalAmount()
        {
            decimal total = 0.0M;
            foreach (var item in InvGrnDetails)
            {
                total += item.SubTotal;
            }
            return total;
        }
    }
}
