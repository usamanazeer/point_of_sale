using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvVendor
    {
        public InvVendor()
        {
            InvGrnMaster = new HashSet<InvGrnMaster>();
            InvGrrnMaster = new HashSet<InvGrrnMaster>();
            InvPhysicalInventoryItem = new HashSet<InvPhysicalInventoryItem>();
            InvPoMaster = new HashSet<InvPoMaster>();
            InvPurchaseMaster = new HashSet<InvPurchaseMaster>();
        }

        public int Id { get; set; }
        public string VendorCode { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public int? AccountId { get; set; }
        public string PrimaryEmail { get; set; }
        public string OtherEmail { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<InvGrnMaster> InvGrnMaster { get; set; }
        public virtual ICollection<InvGrrnMaster> InvGrrnMaster { get; set; }
        public virtual ICollection<InvPhysicalInventoryItem> InvPhysicalInventoryItem { get; set; }
        public virtual ICollection<InvPoMaster> InvPoMaster { get; set; }
        public virtual ICollection<InvPurchaseMaster> InvPurchaseMaster { get; set; }
    }
}
