using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvModifier
    {
        public InvModifier()
        {
            InvItemModifiers = new HashSet<InvItemModifiers>();
            InvModifierItems = new HashSet<InvModifierItems>();
            SalesOrderItemModifiers = new HashSet<SalesOrderItemModifiers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double ModifierCharges { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<InvItemModifiers> InvItemModifiers { get; set; }
        public virtual ICollection<InvModifierItems> InvModifierItems { get; set; }
        public virtual ICollection<SalesOrderItemModifiers> SalesOrderItemModifiers { get; set; }
    }
}
