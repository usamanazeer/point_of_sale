using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class SalesOrderMaster
    {
        public SalesOrderMaster()
        {
            SalesOrderDetails = new HashSet<SalesOrderDetails>();
        }

        public int Id { get; set; }
        public string OrderNo { get; set; }
        public decimal? DiscountAmount { get; set; }
        public bool? IsDiscountInPercent { get; set; }
        public int? TaxId { get; set; }
        public decimal? TaxAmount { get; set; }
        public bool? IsTaxInPercent { get; set; }
        public int? OrderTypeId { get; set; }
        public int? DiningTableId { get; set; }
        public int? WaiterId { get; set; }
        public int? DeliveryServiceVendorId { get; set; }
        public bool IsSelfDelivery { get; set; }
        public string DeliveryServiceReferenceNo { get; set; }
        public int? DeliveryBoyId { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public bool? IsChargesInPercent { get; set; }
        public int? OrderStatusId { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual DeliDeliveryBoy DeliveryBoy { get; set; }
        public virtual DeliDeliveryServiceVendor DeliveryServiceVendor { get; set; }
        public virtual RestDiningTable DiningTable { get; set; }
        public virtual SalesOrderStatus OrderStatus { get; set; }
        public virtual SalesOrderType OrderType { get; set; }
        public virtual RestWaiter Waiter { get; set; }
        public virtual SalesOrderBilling SalesOrderBilling { get; set; }
        public virtual ICollection<SalesOrderDetails> SalesOrderDetails { get; set; }
    }
}
