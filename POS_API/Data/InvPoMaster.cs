using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvPoMaster
    {
        public InvPoMaster()
        {
            InvGrnDetails = new HashSet<InvGrnDetails>();
            InvPodetails = new HashSet<InvPodetails>();
        }

        public int Id { get; set; }
        public string Pono { get; set; }
        public DateTime Podate { get; set; }
        public DateTime DeliveryDate { get; set; }
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
        public virtual ICollection<InvPodetails> InvPodetails { get; set; }
    }
}
