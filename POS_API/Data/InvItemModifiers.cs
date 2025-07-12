using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvItemModifiers
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ModifierId { get; set; }
        public int Quantity { get; set; }
        public bool IsMandatory { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvItem Item { get; set; }
        public virtual InvModifier Modifier { get; set; }
    }
}
