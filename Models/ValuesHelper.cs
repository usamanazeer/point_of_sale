using Models.Enums;
using StatusTypes = Models.Enums.StatusTypes;
namespace Models
{
    public class ValuesHelper
    {
        public static string Get_StatusValue(int key)
        {
            var value = key switch
            {
                (int) StatusTypes.Active => "Active",
                (int) StatusTypes.InActive => "InActive",
                (int) StatusTypes.Delete => "Deleted",
                (int) StatusTypes.Secret => "Secret",
                _ => ""
            };
            return value;
        }

        public const string OrderStatus_Placed = "Placed";
        public const string OrderStatus_Served = "Served";
        public const string OrderStatus_Delivered = "Delivered";
        public const string OrderStatus_Billed = "Billed";
        public const string OrderStatus_Cancelled = "Cancelled";
        public const string OrderStatus_Returned = "Returned";
        public static string Get_OrderStatusValue(int? key)
        {
            if (key is null) return "";

            var value = (int) key switch
            {
                (int) OrderStatus.Placed => OrderStatus_Placed,
                (int) OrderStatus.Served => OrderStatus_Served,
                (int) OrderStatus.Delivered => OrderStatus_Delivered,
                (int) OrderStatus.Billed => OrderStatus_Billed,
                (int) OrderStatus.Cancelled => OrderStatus_Cancelled,
                (int) OrderStatus.Returned => OrderStatus_Returned,
                _ => ""
            };
            return value;
        }
        public static int Get_OrderStatusId(string key)
        {
            if (key is null) return 0;

            var value = key switch
            {
                OrderStatus_Placed => OrderStatus.Placed.ToInt(),
                OrderStatus_Served => OrderStatus.Served.ToInt(),
                OrderStatus_Delivered => OrderStatus.Delivered.ToInt(),
                OrderStatus_Billed => OrderStatus.Billed.ToInt(),
                OrderStatus_Cancelled => OrderStatus.Cancelled.ToInt(),
                OrderStatus_Returned => OrderStatus.Returned.ToInt(),
                _ => 0
            };
            return value;
        }

        public const string OrderType_DineIn = "DineIn";
        public const string OrderType_TakeAway = "TakeAway";
        public const string OrderType_Delivery = "Delivery";

        public static string Get_OrderTypeValue(int? key)
        {
            if (key is null) return "";

            var value = (int) key switch
            {
                (int) OrderTypes.DineIn => OrderType_DineIn,
                (int) OrderTypes.TakeAway => OrderType_TakeAway,
                (int) OrderTypes.Delivery => OrderType_Delivery,
                _ => ""
            };
            return value;
        }
        public static int Get_OrderTypeId(string key)
        {
            if (key is null) return 0;

            var value = key switch
            {
                OrderType_DineIn => OrderTypes.DineIn.ToInt(),
                OrderType_TakeAway => OrderTypes.TakeAway.ToInt(),
                OrderType_Delivery => OrderTypes.Delivery.ToInt(),
                _ => 0
            };
            return value;
        }
    }
}
