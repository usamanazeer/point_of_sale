namespace Models.Enums
{
    public enum StatusTypes
    {
        Active = 1, 
        InActive = 2, 
        Delete = 3, 
        Secret = 4
    }
    public enum StatusCodes
    {
        OK = 200, Created = 201, Not_Found = 404,
        Bad_Request = 400, UnAuthorized = 401, Updated = 600, Not_Modified = 304, Internal_Server_Error = 500,
        Error_Occured = 900, Invalid_State = 800, Conflict = 409, Session_Expired = 440,
        Quantity_Not_Available = 950, Method_NotAllowed = 405
    }
    public enum NotificationTypes
    {
        Low_Inventory = 1, 
        Welcome_New_User = 2,
    }
    public enum ItemTypes
    {
        NormalItem = 0, 
        RawItem = 1, 
        RecipeItem = 2, 
        DealItem = 3
    }
    public enum OrderTypes
    {
        DineIn = 0,
        TakeAway = 1,
        Delivery = 2
    }
    public enum OrderStatus
    {
        Placed = 1,
        Served = 2,
        Delivered = 3,
        Billed = 4,
        Cancelled = 5,
        Returned = 6
    }

    public enum PaymentMode
    {
        Cash = 0,
        Card = 1,
    }


    public enum AccountType
    {
        Asset = 1, 
        Liability = 2, 
        Equity = 3, 
        Expenses = 4, 
        Revenues = 5
    }
    public enum Account
    {
        Cash = 1, 
        AccountReceivable = 2,
        Inventory = 3,
        NotesPayable = 4,
        AccountPayable = 5,
        OwnersEquity = 6,
        Vendors = 7,
        DeliveryServicesReceivables = 8,
        DeliveryBoys = 9,
        DeliveryServicesExpenses = 10,
        SalesRevenue = 11,
        TaxCollected = 12,
        InventoryExpense = 13,
        Bank = 14
    }

    public enum AccBillStatus
    {
        Unpaid = 1,
        PartiallyPaid = 2,
        Paid = 3
    }
    public enum AccBillPaymentType
    {
        Cash = 1,
        Cheque = 2,
        Split = 3
    }
}