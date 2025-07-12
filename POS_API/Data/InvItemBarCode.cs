using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvItemBarCode
    {
        public InvItemBarCode()
        {
            InvItemRecipe = new HashSet<InvItemRecipe>();
            InvModifierItems = new HashSet<InvModifierItems>();
            InvPhysicalInventoryItem = new HashSet<InvPhysicalInventoryItem>();
            InvPurchaseDetail = new HashSet<InvPurchaseDetail>();
        }

        public int Id { get; set; }
        public string BarCode { get; set; }
        public int? ItemId { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual InvItem Item { get; set; }
        public virtual ICollection<InvItemRecipe> InvItemRecipe { get; set; }
        public virtual ICollection<InvModifierItems> InvModifierItems { get; set; }
        public virtual ICollection<InvPhysicalInventoryItem> InvPhysicalInventoryItem { get; set; }
        public virtual ICollection<InvPurchaseDetail> InvPurchaseDetail { get; set; }
    }
}
