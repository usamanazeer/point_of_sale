using Models.DTO.RestaurantManagement;
using System.Collections.Generic;
using System.Linq;
using Models.DTO.DeliveryService;
using Models.Enums;

namespace Models.DTO.SalesManagement
{
    public sealed class SalesOrderMasterDto : ExtendedBaseModel
    {

        public SalesOrderMasterDto()
        {
            SalesOrderDetails = new List<SalesOrderDetailsDto>();
        }
        public string OrderNo { get; set; }
        public double? DiscountAmount { get; set; }
        public bool? IsDiscountInPercent { get; set; }
        public int? TaxId { get; set; }
        public double? TaxAmount { get; set; }
        public bool? IsTaxInPercent { get; set; }
        public int? OrderTypeId { get; set; }
        public string OrderTypeText => ValuesHelper.Get_OrderTypeValue(OrderTypeId);
        public int? DiningTableId { get; set; }
        public int? WaiterId { get; set; }
        public int? DeliveryServiceVendorId { get; set; }
        public bool IsSelfDelivery { get; set; }
        public string DeliveryServiceReferenceNo { get; set; }
        public int? DeliveryBoyId { get; set; }
        public double? DeliveryCharges { get; set; }
        public bool? IsChargesInPercent { get; set; }
        public int? OrderStatusId { get; set; }
        public string OrderStatusText => ValuesHelper.Get_OrderStatusValue(OrderStatusId);


        public DeliDeliveryBoyDto DeliveryBoy { get; set; }
        public DeliDeliveryServiceVendorDto DeliveryServiceVendor { get; set; }
        public RestDiningTableDto DiningTable { get; set; }
        public SalesOrderStatusDto OrderStatus { get; set; }
        public SalesOrderTypeDto OrderType { get; set; }
        public RestWaiterDto Waiter { get; set; }
        public SalesOrderBillingDto SalesOrderBilling { get; set; }
        public IList<SalesOrderDetailsDto> SalesOrderDetails { get; set; }
        public IList<SalesOrderItemModifiersDto> OrderItemsModifiers => SalesOrderDetails.SelectMany(x => x.SalesOrderItemModifiers).ToList();

        //DTO PROPS
        public IList<SalesOrderMasterDto> Orders { get; set; }
        public double GetOrderAmount()
        {
            //if (SalesOrderBilling != null)
            //{
            //    return SalesOrderBilling.TotalBillAmount ?? 0;
            //}

            if (SalesOrderDetails == null || SalesOrderDetails.Count == 0)
            {
                return 0;
            }
            return SalesOrderDetails.Select(x =>
            {
                var itemTotalRates = (x.FinalSalesRate * x.Quantity);
                var modifiersTotalRates = x.SalesOrderItemModifiers.Select(y => (y.Quantity ?? 0) * (y.Charges ?? 0)).Sum();
                return itemTotalRates + modifiersTotalRates;
            }).Sum();
        }
        public double GetOrderAmountWithDiscount()
        {
            return GetOrderAmount() - GetDiscount();
        }
        public double GetOrderAmountAfterTax()
        {
            return GetOrderAmountWithDiscount() + (TaxAmount ?? 0);
        }
        public double GetOrderAmountPayable()
        {
            return GetOrderAmountAfterTax()+GetDeliveryCharges();
        }
        public double GetDeliveryCharges()
        {
            double deliveryCharges = 0;
            
            if (IsSelfDelivery && OrderTypeId == OrderTypes.Delivery.ToInt()) {
                deliveryCharges = DeliveryCharges ?? 0;
                if (IsChargesInPercent??false)
                {
                    deliveryCharges = deliveryCharges * GetOrderAmountWithDiscount() * 0.01;
                }
            }
            return deliveryCharges;
        }
        //public double GetOrderItemsTotal()
        //{
        //    if (SalesOrderDetails == null || SalesOrderDetails.Count == 0)
        //    {
        //        return 0;
        //    }
        //    return SalesOrderDetails.Select(x => x.FinalSalesRate * x.Quantity).Sum();
        //}
        //public double GetOrderModifiersTotal()
        //{
        //    if (OrderItemsModifiers == null || OrderItemsModifiers.Count == 0)
        //    {
        //        return 0;
        //    }
        //    return OrderItemsModifiers.Select(y => (y.Quantity ?? 0) * (y.Charges ?? 0)).Sum();
        //}
        public double GetDiscount()
        {
            if (!(IsDiscountInPercent??false))
            {
                return DiscountAmount??0;
            }
            return (DiscountAmount??0) * GetOrderAmount() * .01;
        }
    }
}