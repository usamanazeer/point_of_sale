using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_API.Data.TVPs
{
    // ReSharper disable once InconsistentNaming
    public class OrderItemTVP
    {
        public int ItemId { get; set; }
        public int? BarCodeId { get; set; }
        public decimal Quantity { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
    }
}