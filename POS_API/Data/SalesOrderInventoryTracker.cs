using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class SalesOrderInventoryTracker
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? InventoryItemId { get; set; }
        public int? ItemId { get; set; }
        public double Quantity { get; set; }
    }
}
