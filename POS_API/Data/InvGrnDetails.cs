using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvGrnDetails
    {
        public int Id { get; set; }
        public int GrnId { get; set; }
        public int? PoId { get; set; }
        public int ItemId { get; set; }
        public string BatchNo { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal Rate { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvGrnMaster Grn { get; set; }
        public virtual InvItem Item { get; set; }
        public virtual InvPoMaster Po { get; set; }
    }
}
