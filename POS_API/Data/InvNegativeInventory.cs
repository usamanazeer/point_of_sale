using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class InvNegativeInventory
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }

        public virtual InvItem Item { get; set; }
    }
}
