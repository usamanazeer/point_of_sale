using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace POS_API.Data.TVPs
{
    // ReSharper disable once InconsistentNaming
    public class OrderItemModifierTVP
    {
        // ReSharper disable once InconsistentNaming
        public int OrderItemId { get; set; }
        public int ModifierId { get; set; }
        public float Quantity { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
    }
}
