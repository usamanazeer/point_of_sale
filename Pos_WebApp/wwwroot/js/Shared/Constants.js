const CODES = {
    RESPONSE_CODE_OK : 200,
    RESPONSE_CODE_CREATED: 201,
    RESPONSE_CODE_UPDATED: 600,
    RESPONSE_CODE_NOTMODIFIED: 304,

    //errorscodes
    RESPONSE_CODE_ERROR: 900,
    RESPONSE_CODE_NOTFOUND: 404,
    RESPONSE_CODE_INVALIDSTATE: 800,
    //extras
    Quantity_Not_Available : 950
}
const STATUSES = {
    ACTIVE: 1,
    INACTIVE: 2,
    DELETED: 3,
    SECRET: 4
}
const ITEM_TYPES = {
    NormalItem: 0,
    RawItem: 1,
    RecipeItem: 2,
    DealItem: 3
}
const ORDER_SERVE_TYPE = {
    DineIn: 0,
    TakeAway: 1,
    Delivery: 2
}
const ORDER_STATUS = {
    Placed: 1,
    Served: 2,
    Delivered: 3,
    Billed: 4,
    Cancelled: 5
}
const Payment_Mode = {
    Cash: 0,
    Card: 1
}
const BILL_PAYMENT_TYPE = {
    CASH : 1,
    CHEQUE : 2,
    SPLIT : 3
}