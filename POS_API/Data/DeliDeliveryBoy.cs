using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class DeliDeliveryBoy
    {
        public DeliDeliveryBoy()
        {
            SalesOrderMaster = new HashSet<SalesOrderMaster>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Cnic { get; set; }
        public string BikeNo { get; set; }
        public string Email { get; set; }
        public int AccountId { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<SalesOrderMaster> SalesOrderMaster { get; set; }
    }
}
