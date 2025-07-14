using System;
using System.Collections.Generic;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvGrrnMasterDto : ExtendedBaseModel
    {
        public InvGrrnMasterDto()
        {
            InvGrrnDetails = new List<InvGrrnDetailsDto>();
        }

        public string GrrnNo { get; set; }
        public DateTime? GrrnDate { get; set; }
        public string InvoiceNo { get; set; }
        public int VendorId { get; set; }
        public string Description { get; set; }

        public InvVendorDto Vendor { get; set; }
        public IList<InvGrrnDetailsDto> InvGrrnDetails { get; set; }
        public IList<InvGrrnMasterDto> GoodsRetrunNotes { get; set; }

        public decimal GetTotalAmount()
        {
            decimal total = 0.0M;
            foreach (var item in InvGrrnDetails)
            {
                total += item.SubTotal;
            }
            return total;
        }
    }
}
