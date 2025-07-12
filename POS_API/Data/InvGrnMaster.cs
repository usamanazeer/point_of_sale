using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvGrnMaster
    {
        public InvGrnMaster()
        {
            InvGrnDetails = new HashSet<InvGrnDetails>();
        }

        public int Id { get; set; }
        public string GrnNo { get; set; }
        public DateTime GrnDate { get; set; }
        public string InvoiceNo { get; set; }
        public int VendorId { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvVendor Vendor { get; set; }
        public virtual ICollection<InvGrnDetails> InvGrnDetails { get; set; }
    }
}
