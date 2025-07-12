using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvGrrnDetails
    {
        public int Id { get; set; }
        public int GrrnId { get; set; }
        public int ItemId { get; set; }
        public string BatchNo { get; set; }
        public double ReturnQuantity { get; set; }
        public double Rate { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvGrrnMaster Grrn { get; set; }
        public virtual InvItem Item { get; set; }
    }
}
