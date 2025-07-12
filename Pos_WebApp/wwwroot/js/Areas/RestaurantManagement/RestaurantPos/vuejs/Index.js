//function _HeightWidth(seconds = 5) {
//    setTimeout(function() {
//            const width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
//            const height = (window.innerHeight > 0) ? window.innerHeight : screen.height;
//            alert(`w: ${width}, h: ${height}`);
//        },
//        seconds * 1000);
//}

//function _SearchListItemByKeyValue(list, value, key = "id") {
//    var item = null;
//    var index = null;
//    for (let i = 0; i < list.length; i++) {
//        if (list[i][key] == value) {
//            index = i;
//            item = list[i];
//            break;
//        }
//    }
//    return { index: index, item: item };
//}

//function _CopyObject(obj) {
//    if (obj != undefined || obj != null) {
//        return JSON.parse(JSON.stringify(obj));
//    }
//    return null;
//}

// register modal component
//Vue.component("modal",
//    {
//        template: "#paymentModal-template"
//    });




var posapp = new Vue({
    el: "#posapp",
    data() {
        return {
            ErrorOnLoad: false,
            EditMode: false,
            OrderToEdit: null,
            AntiForgeryToken: null,
            BaseURL: "/RestaurantPos/",
            TaxToApply: {},
            TaxAmount: 0,
            OnOrderDiscount: 0,
            DiscountInPercent: false,
            TablesList: [],
            WaitersList: [],
            CategoriesList: [],
            SubCategoriesList: [],
            ItemsList: [],
            CartList: [],
            OrderTotal: 0.00,
            NetTotal: 0,
            //OrdetrTotalAfterDiscount:0.00,
            ItemToAddModifiers: {},
            ORDER_TYPES: ORDER_SERVE_TYPE,
            OrderType: ORDER_SERVE_TYPE.TakeAway,
            SearchItemText: "",
            AllowSearchFlag: true,
            MainCategoryTitle: "All Categories",
            Active_Category: null,
            Active_SubCategory: null,
            TableNo: "",
            WaiterNo: "",
            DeliveryService: "",
            //DeliveryCharges: 0,
            DeliveryServiceNo: '',
            DeliveryServiceReferenceNo: "",
            IsSelfDeliveryService: '',
            DeliveryBoyId: '',
            PendingOrders: [],
            CashReceived: 0,
            CashBack: 0,
            CardCashReceived: 0,
            PaymentMode: Payment_Mode.Cash,
            CheckoutOrderId: null
        };
    },
    methods: {
        Init() {
            //floatKeypad();
            //console.log('Init called!');
            var _this = this;
            this.AntiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            this.CategoriesList = window.Categories;
            this.TablesList = window.Tables;
            this.WaitersList = window.Waiters;
            this.DeliveryServicesList = window.DeliveryServices;
            this.DeliveryBoysList = window.DeliveryBoys;
            //console.log(this.DeliveryBoysList);
            this.GetPlacedOrders();
            const URL = this.BaseURL + "ApplyCategoryFilter";
            $.ajax({
                url: URL,
                type: "GET",
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    _this.SubCategoriesList = res.model[0];
                    _this.PopulateItemsList(res.model[1]);
                },
                error: function (error) {

                }
            });
            $.ajax({
                url: URL,
                type: "GET",
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    _this.SubCategoriesList = res.model[0];
                    _this.PopulateItemsList(res.model[1]);
                },
                error: function (error) {

                }
            });
            this.GetTaxToApply();
            setTimeout(function () {
                $(".select").select2();
            },
                1000);
        },
        GetTaxToApply() {
            //console.log('GetTaxToApply called!');
            var _this = this;
            const urlForTax = "/taxes/getenabledforpos";
            $.ajax({
                url: urlForTax,
                type: "GET",
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    if (res.responseCode === CODES.RESPONSE_CODE_OK) {
                        //console.log(res.model);
                        _this.TaxToApply = res.model;
                    } else if (res.errorOccured) {
                        this.ErrorOnLoad = true;
                    }
                },
                error: function (error) {

                }
            });
        },
        PopulateItemsList(list) {
            //console.log('PopulateItemsList called!');
            _this = this;
            if (list != null) {
                for (let i in list) {
                    const item = _SearchListItemByKeyValue(_this.CartList, list[i].id).item;
                    list[i].quantity = item != null ? item.quantity : 0;
                }
                _this.ItemsList = list;
            } else {
                _this.ItemsList = [];
            }

            setTimeout(function () {
                $('.openModalInput').on('focus', function () {
                    $('.floatingKeyboard').show();
                });
                $('.openModalInput').on('blur', function () {
                    $('.floatingKeyboard').hide();
                });
            }, 200);
        },
        GetPlacedOrders() {
            //console.log('GetPlacedOrders called!');
            var _this = this;
            const URL = this.BaseURL +
                "GetOrders?orderStatus=" +
                ORDER_STATUS.Placed;
            $.ajax({
                url: URL,
                type: "GET",
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    if (res.responseCode == CODES.RESPONSE_CODE_OK) {
                        //console.log('PlacedOrders', res.model);
                        _this.PendingOrders = res.model;
                        //console.log(_this.PendingOrders);
                    }
                },
                error: function (error) { }
            });

        },
        CategoryClick(categoryId) {
            //console.log('CategoryClick called!');
            this.SearchItemText = "";
            this.Active_Category = categoryId;
            var _this = this;
            const URL = this.BaseURL + "ApplyCategoryFilter?categoryId=" + categoryId;
            $.ajax({
                url: URL,
                type: "GET",
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    if (res.responseCode == CODES.RESPONSE_CODE_OK) {
                        _this.SubCategoriesList = res.model[0];
                        if (categoryId) {
                            var currentCategory =
                                _SearchListItemByKeyValue(_this.CategoriesList, categoryId, "Id").item;
                            if (currentCategory != null) {
                                _this.MainCategoryTitle = currentCategory.Name;
                            }
                        } else {
                            _this.MainCategoryTitle = "All Categories";
                        }
                        _this.PopulateItemsList(res.model[1]);
                    }
                },
                error: function (error) { }
            });
        },
        DealsClick() {
            //console.log('DealsClick called!');
            this.SearchItemText = "";
            this.Active_Category = "deals";
            var _this = this;
            const URL = this.BaseURL + "AllDealsFilter";
            $.ajax({
                url: URL,
                type: "GET",
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    if (res.responseCode == CODES.RESPONSE_CODE_OK) {
                        _this.SubCategoriesList = res.model[0];
                        _this.PopulateItemsList(res.model[1]);
                        _this.MainCategoryTitle = "Deals";
                    }
                },
                error: function (error) { },
            });
        },
        SubCategoryClick(subcategoryId) {
            //console.log('SubCategoryClick called!');
            _this = this;
            _this.SearchItemText = "";
            let URL = this.BaseURL +
                "ApplySubCategoryFilter?categoryId=" +
                _this.Active_Category +
                "&subcategoryId=" +
                subcategoryId;
            if (_this.Active_Category == "deals") {
                URL = this.BaseURL + "SubCategoryDealsFilter?subcategoryId=" + subcategoryId;
            }
            $.ajax({
                url: URL,
                type: "GET",
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    if (res.responseCode == CODES.RESPONSE_CODE_OK) {
                        _this.PopulateItemsList(res.model);
                        _this.Active_SubCategory = subcategoryId == undefined ? null : subcategoryId;
                    }
                },
                error: function (error) { }
            });
        },
        SearchItems(e) {
            //console.log('SearchItems called!');
            var _this = this;
            if (_this.SearchItemText == "") {

                _this.SubCategoryClick(this.Active_SubCategory);
                return;
            };
            if (_this.AllowSearchFlag) {
                _this.AllowSearchFlag = false;
                const URL = this.BaseURL + "ApplySearchTextFilter?searchText=" + _this.SearchItemText;
                $.ajax({
                    url: URL,
                    type: "GET",
                    headers: {
                        RequestVerificationToken: _this.AntiForgeryToken
                    },
                    success: function (res) {
                        if (res.responseCode == CODES.RESPONSE_CODE_OK) {
                            _this.PopulateItemsList(res.model);
                            _this.AllowSearchFlag = true;
                        };
                    },
                    error: function (error) { }
                });
            }
        },
        ItemQuantityChange(event) {
            //console.log('ItemQuantityChange called!');
            const index = $(event.target).attr("data-index");
            this.AddToOrderList(index);
        },
        AddToOrderList(index) {
            //console.log('AddToOrderList called!');
            var item = this.ItemsList[index];
            //if quantity is null;
            if (String(item.quantity).trim() == "" || parseFloat(item.quantity) == 0) {
                item.quantity = 0;
            }
            item.quantity = parseFloat(item.quantity);
            //if quantity is less than 0;
            if (item.quantity < 0) {
                item.quantity = 0;
            }
            if (item.itemType == ITEM_TYPES.NormalItem && item.manageStock == true && item.allowBackOrder == false) {
                item.quantity = item.quantity > item.remainingInventory ? item.remainingInventory : item.quantity;
            }
            var itemInCart = _SearchListItemByKeyValue(this.CartList, item.id);
            if (item.quantity > 0) {
                //add to cart
                if (itemInCart.item == null) {
                    //if item is not already in cart.
                    itemInCart.item = _CopyObject(item);
                    itemInCart.item.itemModifiers = [];
                    itemInCart.item.subTotal = 0;
                    //add mendatory modifiers
                    if (item.itemModifiers != null) {
                        var modifiers = item.itemModifiers.filter(m => {
                            return m.isMandatory == true;
                        });
                        for (var i in modifiers) {
                            modifiers[i].totalQuantity = modifiers[i].quantity * itemInCart.item.quantity;
                            modifiers[i].totalCharges =
                                modifiers[i].totalQuantity * modifiers[i].modifier.modifierCharges;
                        }
                        itemInCart.item.itemModifiers = modifiers;
                    } else {
                        itemInCart.item.itemModifiers = [];
                    }
                    this.CartList.push(itemInCart.item);
                    itemInCart.index = this.CartList.length - 1;
                } else {
                    //if item is already in cart.
                    itemInCart.item.quantity = item.quantity;
                    for (var i in itemInCart.item.itemModifiers) {
                        var modifier = itemInCart.item.itemModifiers[i];
                        modifier.totalQuantity = modifier.quantity * itemInCart.item.quantity;
                        modifier.totalCharges = modifier.totalQuantity * modifier.modifier.modifierCharges;
                    }
                }
                itemInCart.item.quantity = Math.round((itemInCart.item.quantity) * 100 + Number.EPSILON) / 100;
                itemInCart.item.subTotal =
                    Math.round((itemInCart.item.quantity * itemInCart.item.finalSalesRate) * 100 + Number.EPSILON) /
                    100;
                //console.log(itemInCart);
            } else {
                //remove from cart
                if (itemInCart.index != null) {
                    this.RemoveFromCart(itemInCart.item.id, itemInCart.index);
                }
            }
            this.TotalOrderAmount();
        },
        FilterCartItemsByCategory(categoryId) {
            //console.log('FilterCartItemsByCategory called!');
            return this.CartList.filter(item => {
                return item.categoryId == categoryId && item.itemType != ITEM_TYPES.DealItem;
            });
        },
        FilterCartItemsByDeals() {
            //console.log('FilterCartItemsByDeals called!');
            return this.CartList.filter(item => {
                return item.itemType == ITEM_TYPES.DealItem;
            });
        },
        RemoveFromCart(key, index) {
            //console.log('RemoveFromCart called!');
            //console.log(key);
            if (index == undefined || index == null) {
                index = _SearchListItemByKeyValue(this.CartList, key).index;
            }
            this.CartList.splice(index, 1);
            const item = _SearchListItemByKeyValue(this.ItemsList, key).item;
            item.quantity = 0;
            this.TotalOrderAmount();
        },
        RemoveModifier(itemKey, modiferkey) {
            //console.log('RemoveModifier called!');
            const item = _SearchListItemByKeyValue(this.CartList, itemKey).item;
            if (item != null) {
                const modifierIndex = _SearchListItemByKeyValue(item.itemModifiers, modiferkey).index;
                item.itemModifiers.splice(modifierIndex, 1);
                this.TotalOrderAmount();
            }
        },
        TotalOrderAmount() {

            this.OrderTotal = this.CartList.reduce(function (prev, cur) {
                return prev +
                    cur.subTotal +
                    cur.itemModifiers.reduce(function (_prev, _cur) {
                        return _prev + _cur.totalCharges;
                    },
                        0);
            },
                0);
            if (this.TaxToApply.name) {
                if (!this.TaxToApply.isInPercent) {
                    this.TaxAmount = this.TaxToApply.amount.roundOff(2);
                } else {
                    this.TaxAmount = ((this.TaxToApply.amount * this.OrdetrTotalAfterDiscount) / 100).roundOff(2);
                }
            }
            this.NetTotal = (this.OrdetrTotalAfterDiscount + this.DeliveryCharges + this.TaxAmount).roundOff(2);
        },
        //Add Modifier Modal events
        OpenAddModifierModal(itemkey) {
            //console.log('OpenAddModifierModal called!');
            const item = _CopyObject(_SearchListItemByKeyValue(this.ItemsList, itemkey).item);
            const orderedItem = _CopyObject(_SearchListItemByKeyValue(this.CartList, itemkey).item);
            for (let i in item.itemModifiers) {
                const modifier = item.itemModifiers[i];
                const orderItemModifier = _SearchListItemByKeyValue(orderedItem.itemModifiers, modifier.id).item;
                if (orderItemModifier != null) {
                    modifier.totalQuantity = orderItemModifier.totalQuantity;
                } else {
                    modifier.totalQuantity = 0;
                    modifier.totalCharges = 0;
                }
            }
            this.ItemToAddModifiers = item;
            customNumberInput_INIT();
        },
        AddModifier(id) {
            //console.log('AddModifier called!');
            //alert('called');
            //modifier to add/update/remove.
            const modifier = _SearchListItemByKeyValue(this.ItemToAddModifiers.itemModifiers, id).item;

            //if modifier quantity is null or <= 0, set it to 0.
            if (String(modifier.totalQuantity).trim() == "" || parseFloat(modifier.totalQuantity) <= 0) {
                modifier.totalQuantity = 0;
            }
            //cart item in which modifier is to be added/updated/remove.
            const cartItem = _SearchListItemByKeyValue(this.CartList, this.ItemToAddModifiers.id).item;
            //modifier in cart which has to be added/updated/remove.
            var cartModifier = _SearchListItemByKeyValue(cartItem.itemModifiers, id).item;
            //if modifier is mandatory, then modifier quantity in cart must be >= modifer quantity in itemslit.
            if (modifier.isMandatory && modifier.totalQuantity < modifier.quantity) {
                modifier.totalQuantity = modifier.quantity;
            }
            //if modifier quantity is 0 and modifer in cart exists, remove modifier from cartlist. and return.
            if (modifier.totalQuantity == 0) {
                if (cartModifier != null) {
                    this.RemoveModifier(cartItem.id, cartModifier.id);
                }
                return;
            }
            //from here, if quantity is > 0.
            if (cartModifier != null) {
                //if modifier exits in cartlist, update modifier quantity, and charges in cartlist.
                cartModifier.totalQuantity = modifier.totalQuantity;
                cartModifier.totalCharges = modifier.totalQuantity * modifier.modifier.modifierCharges;
            } else {
                //if modifier does not exit in cartlist, add modifier in cartlist.
                var cartModifier = _CopyObject(modifier);
                cartModifier.totalCharges = modifier.totalQuantity * modifier.modifier.modifierCharges;
                cartItem.itemModifiers.push(cartModifier);
            }
            //total
            this.TotalOrderAmount();
        },
        PlaceOrder() {
            //console.log('PlaceOrder called!');
            const _this = this;
            const Order = {
                OrderTypeId: _this.OrderType,
                DiscountAmount: _this.OnOrderDiscount,
                IsDiscountInPercent: _this.DiscountInPercent,
                TaxId: _this.TaxToApply.id,
                TaxAmount: _this.TaxAmount,
                IsTaxInPercent: _this.TaxToApply.isInPercent,
                DiningTableId: parseInt(_this.TableNo === "" ? null : _this.TableNo),
                WaiterId: parseInt(_this.WaiterNo === "" ? null : _this.WaiterNo),
                DeliveryServiceVendorId: parseInt(_this.DeliveryServiceNo === "" ? null : _this.DeliveryServiceNo),
                IsSelfDelivery: _this.IsSelfDeliveryService,
                DeliveryServiceReferenceNo: _this.DeliveryServiceReferenceNo,
                OrderStatusId: [_this.ORDER_TYPES.DineIn, _this.ORDER_TYPES.Delivery].includes(_this.OrderType) ? ORDER_STATUS.Placed : ORDER_STATUS.Billed,
                SalesOrderDetails: [],
                SalesOrderBilling: null
            };
            if (_this.IsSelfDeliveryService) {
                Order.DeliveryCharges = _this.DeliveryService.ServiceCharges;
                Order.IsChargesInPercent = _this.DeliveryService.ChargesInPercent;
            }
            const CartItems = _CopyObject(_this.CartList);
            for (let i in CartItems) {
                const Item = CartItems[i];
                //console.log(Item);
                const OrderItem = {
                    ItemId: Item.id,
                    Quantity: Item.quantity,
                    PurchaseRate: Item.purchaseRate,
                    SalesRate: Item.finalSalesRate,
                    DiscountAmount: Item.discountAmount,
                    IsDiscountInPercent: Item.isDiscountInPercent,
                    TaxId: Item.taxId,
                    SalesOrderItemModifiers: [],
                };
                for (let j in Item.itemModifiers) {
                    const modifier = Item.itemModifiers[j];
                    const ItemModifier = {
                        OrderItemId: modifier.itemId,
                        ModifierId: modifier.modifierId,
                        Quantity: modifier.totalQuantity,
                        Charges: modifier.totalCharges
                    };
                    OrderItem.SalesOrderItemModifiers.push(ItemModifier);
                }
                Order.SalesOrderDetails.push(OrderItem);
            }
            Order.SalesOrderBilling = {
                TaxId: _this.TaxToApply.id,
                TaxAmount: _this.TaxAmount,
                PaymentType: _this.PaymentMode,
                IsTaxInPercent: _this.TaxToApply.isInPercent,
                CashReceived: parseFloat(_this.CashReceived),
                CashReturn: parseFloat(_this.CashBack),
                TotalBillAmount: parseFloat(_this.OrderTotal) + parseFloat(_this.TaxAmount) - parseFloat(_this.DiscountAmount)
            };
            if (!_this.EditMode) {
                //create order mode.....
                //console.log(Order);
                return _this.__CreateOrder(Order);
            } else {
                //when EditMode is true
                //edit order mode.....
                return _this.__UpdateOrder(Order);
            }
        },
        __CreateOrder(Order) {
            //console.log('__CreateOrder called!');
            //console.log(Order);
            var _this = this;
            $.ajax({
                url: _this.BaseURL + "placeorder",
                type: "POST",
                data: Order,
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    //console.log("response:", res);
                    if (res.responseCode == CODES.RESPONSE_CODE_CREATED) {
                        AlertManager.AlertSweetly(res);

                        if (_this.OrderType == _this.ORDER_TYPES.DineIn) {
                            _this.PendingOrders.push(res.model);
                            //mark table reserved.
                            const table = _SearchListItemByKeyValue(_this.TablesList, _this.TableNo, "Value").item;
                            if (table != null) {
                                table.IsOccupied = true;
                            }
                        } else if (_this.OrderType == _this.ORDER_TYPES.Delivery) {
                            _this.PendingOrders.push(res.model);
                            //console.log(_this.PendingOrders);
                        }
                        _this.ResetPage();
                        return true;
                    } else if (res.errorCode == CODES.Quantity_Not_Available) {
                        //in case requested quantity is not available.
                        const responseModel = res.model.response;
                        let messageString = "";
                        //console.log("response:", responseModel.model);
                        for (let i = 0; i < responseModel.model.length; i++) {
                            messageString += `${(i + 1)}. ${responseModel.model[i].itemName
                                } is not available in required quantity (${responseModel.model[i].requestedQuantity
                                })\n`;
                        }
                        sweetAlert.error({ title: "Quantity Exceeded!", text: messageString });
                    }
                },
                error: function (error) {
                    console.error(error);
                }
            });
        },
        __UpdateOrder(Order) {
            //console.log('__UpdateOrder called!');
            var _this = this;
            //console.log(Order);
            //console.log(_this.OrderToEdit);
            Order.Id = _this.OrderToEdit.id;
            Order.Status = STATUSES.ACTIVE;

            $.ajax({
                url: _this.BaseURL + "updateorder",
                type: "POST",
                data: Order,
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    AlertManager.AlertSweetly(res);
                    if (res.responseCode == CODES.RESPONSE_CODE_UPDATED) {
                        if (_this.OrderType == _this.ORDER_TYPES.DineIn) {
                            let oldItem = _SearchListItemByKeyValue(_this.PendingOrders, res.model.id).item;
                            if (oldItem != null) {
                                oldItem = res.model;
                                //console.log(oldItem);
                            }
                            _this.ResetPage();
                        }
                        return true;
                    } else if (res.errorCode == CODES.Quantity_Not_Available) {
                        //in case requested quantity is not available.
                        const responseModel = res.model.response;
                        let messageString = "";
                        //console.log("response:", responseModel.model);
                        for (let i = 0; i < responseModel.model.length; i++) {
                            messageString += `${(i + 1)}.\t\t${responseModel.model[i].message}\n`;
                        }
                        sweetAlert.error({ title: "Quantity Exceeded!", text: messageString });
                    }
                },
                error: function (error) {
                    //console.log(error);
                    return false;
                }
            });
        },
        Checkout(orderId) {
            //console.log('Checkout called!');
            var _this = this;
            $.ajax({
                url: _this.BaseURL + "Checkout",
                type: "POST",
                data: {
                    Id: orderId,
                    DeliveryBoyId: _this.DeliveryBoyId,
                    DiscountAmount: _this.OnOrderDiscount,
                    IsDiscountInPercent: _this.DiscountInPercent,
                    SalesOrderBilling:
                    {
                        TaxId: _this.TaxToApply.id,
                        TaxAmount: _this.TaxAmount,
                        PaymentType: _this.PaymentMode,
                        IsTaxInPercent: _this.TaxToApply.isInPercent,
                        CashReceived: parseFloat(_this.CashReceived),
                        CashReturn: parseFloat(_this.CashBack),
                        TotalBillAmount: parseFloat(_this.OrderTotal) + parseFloat(_this.TaxAmount) - parseFloat(_this.DiscountAmount)
                    }
                },
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    //console.log(res);
                    if (res.responseCode == CODES.RESPONSE_CODE_OK) {
                        const diningTableIndex =
                            _SearchListItemByKeyValue(_this.TablesList, res.model.diningTableId, "Value").index;
                        if (diningTableIndex != null) {
                            _this.TablesList[diningTableIndex].IsOccupied = false;
                        }
                        var orderToRemove = _SearchListItemByKeyValue(_this.PendingOrders, orderId, "id");
                        if (orderToRemove != null) {
                            _this.PendingOrders.splice(orderToRemove.index, 1);
                        }

                        AlertManager.AlertSweetly(res, window.location.href);
                        this.CheckoutOrderIndex = null;
                    }
                },
                error: function (error) {
                    //console.log(error);
                    this.CheckoutOrderIndex = null;
                    return false;
                }
            });
        },
        ResetPage() {
            //console.log('ResetPage called!');
            const _this = this;
            _this.EditMode = false;
            _this.OrderToEdit = null;
            _this.CartList = [];
            _this.CategoryClick();
            $("#select-table-no").val("");
            _this.OrderTotal = 0.00;
            _this.TaxAmount = 0.00;
            _this.NetTotal = 0.00;
            //_this.OrdetrTotalAfterDiscount = 0.00
            _this.TableNo = "";
            _this.CashReceived = 0;
            _this.OnOrderDiscount = 0;
            _this.DiscountInPercent = false;
            _this.CashBack = 0;
            $("#select-delivery-service").val("");
            _this.DeliveryCharges = 0;
            _this.DeliveryService = "";
            _this.DeliveryServiceNo = '';
            _this.DeliveryServiceReferenceNo = "";
            _this.IsSelfDeliveryService = '';
            $("#select-delivery-boy-no").val("");
        },
        LoadOrderToEdit(orderId) {
            //console.log('LoadOrderToEdit called!');
            var _this = this;

            $.ajax({
                url: `${_this.BaseURL}GetOrderDetails/${orderId}`,
                type: "GET",
                headers: {
                    RequestVerificationToken: _this.AntiForgeryToken
                },
                success: function (res) {
                    if (res.responseCode == CODES.RESPONSE_CODE_OK) {
                        var resData = res.model;
                        //console.log(resData);
                        _this.OrderToEdit = resData;
                        _this.CartList = [];
                        for (var i in _this.OrderToEdit.salesOrderDetails) {
                            var orderItem = _this.OrderToEdit.salesOrderDetails[i];

                            orderItem.id = _this.OrderToEdit.salesOrderDetails[i].itemId;
                            orderItem.name = _this.OrderToEdit.salesOrderDetails[i].item.name;
                            orderItem.fullName = _this.OrderToEdit.salesOrderDetails[i].item.fullName;
                            orderItem.categoryId = _this.OrderToEdit.salesOrderDetails[i].item.categoryId;
                            orderItem.subCategoryId = _this.OrderToEdit.salesOrderDetails[i].item.subCategoryId;
                            orderItem.finalSalesRate = orderItem.finalSalesRate;
                            orderItem.quantity = Math.round((orderItem.quantity) * 100 + Number.EPSILON) / 100;
                            orderItem.subTotal =
                                Math.round((orderItem.quantity * orderItem.finalSalesRate) * 100 + Number.EPSILON) /
                                100;
                            orderItem.itemModifiers = [];
                            var listItemObj = _SearchListItemByKeyValue(_this.ItemsList, orderItem.id);
                            if (listItemObj.item != null) {
                                listItemObj.item.quantity = orderItem.quantity;
                            }
                            for (var j in orderItem.salesOrderItemModifiers) {
                                var orderModifier = orderItem.salesOrderItemModifiers[j];
                                orderModifier.id = orderModifier.modifier.id;
                                //check if isMandatory?
                                orderModifier.totalQuantity = orderModifier.quantity;
                                orderModifier.totalCharges = orderModifier.charges * orderModifier.quantity;
                                if (listItemObj.item != null) {
                                    var itemModifierObj = _SearchListItemByKeyValue(listItemObj.item.itemModifiers,
                                        orderModifier.modifier.id,
                                        "modifierId");
                                    orderModifier.id = itemModifierObj.item.id;
                                    orderModifier.isMandatory = itemModifierObj.item.isMandatory;
                                    itemModifierObj.item.totalQuantity = orderModifier.totalQuantity;
                                }
                                orderItem.itemModifiers.push(orderModifier);
                            }
                            _this.CartList.push(orderItem);
                        }
                        _this.EditMode = true;
                        _this.OrderType = _this.OrderToEdit.orderTypeId;
                        _this.TableNo = _this.OrderToEdit.diningTableId;

                        setTimeout(function () {
                            $("#select-table-no").val(_this.TableNo);
                            $("#select-table-no").select2();
                        },
                            100);
                        //console.log(_this.TableNo);
                        _this.WaiterNo = _this.OrderToEdit.waiterId;
                        _this.TotalOrderAmount();
                        $(".notification-sidebar").removeClass("open");
                    }
                },
                error: function (error) { }
            });
        },
        TableName(tableId) {
            //console.log('TableName called!');
            const table = _SearchListItemByKeyValue(this.TablesList, tableId, "Value").item;
            if (table == null) {
                return "";
            }
            return table.Text;
        },
        TableNameWithFloor(tableId) {
            //console.log('TableNameWithFloor called!');
            const table = _SearchListItemByKeyValue(this.TablesList, tableId, "Value").item;
            if (table == null) {
                return "";
            }
            return table.Text + " (" + table.FloorName + ")";
        },
        WaiterName(waiterId) {
            //console.log('WaiterName called!');
            const waiter = _SearchListItemByKeyValue(this.WaitersList, waiterId, "Value").item;
            if (waiter == null) {
                return "";
            }
            return waiter.Text;
        },
        DeliveryServiceName(deliveryServiceId) {
            //console.log('DeliveryServiceName called!');
            const deliveryService = _SearchListItemByKeyValue(this.DeliveryServicesList, deliveryServiceId, "Value").item;
            if (!this.DeliveryService) {
                return "";
            }
            return deliveryService.Text;
        },
        FreeTables(isForOrderSelect = false) {
            //console.log('FreeTables called!');
            var _this = this;
            return this.TablesList.filter(table => {
                if (isForOrderSelect) {
                    //on edit mode, return order table also.
                    return (table.IsOccupied != true ||
                        ((_this.OrderToEdit != null) &&
                            (_this.OrderToEdit.diningTableId == table.Value && this.EditMode == true)));
                } else {
                    return (table.IsOccupied != true);
                }
            });
        },
        OccupiedTables() {
            //console.log('OccupiedTables called!');
            return this.TablesList.filter(table => {
                return table.IsOccupied == true;
            });
        },
        DineInPendingOrders() {
            //console.log('DineInPendingOrders called!');
            return this.PendingOrders.filter(order => {
                return order.orderTypeId == ORDER_SERVE_TYPE.DineIn;
            });
        },
        DeliveryPendingOrders() {
            //console.log('DeliveryPendingOrders called!');
            return this.PendingOrders.filter(order => {
                return order.orderTypeId == ORDER_SERVE_TYPE.Delivery;
            });
        },
        CheckoutBtnClick(orderId, orderTypeId, isSelfDeliveryOrder) {
            //console.log('CheckoutBtnClick called!');
            var _this = this;
            this.CheckoutOrderId = orderId;
            if (orderTypeId == this.ORDER_TYPES.DineIn) {
                this.OpenPaymentModal();
                return;
            }
            if (orderTypeId == this.ORDER_TYPES.Delivery && isSelfDeliveryOrder) {
                this.OpenDeliveryBoyModal();
                return;
            }
            this.CheckoutConfirmModal();
            return;
        },
        PlaceOrderOrCheckout() {
            //console.log('PlaceOrderOrCheckout called!');
            if (this.CheckoutOrderId != null) {
                this.Checkout(this.CheckoutOrderId);
            }
            else {
                if (this.OrderType == this.ORDER_TYPES.TakeAway || this.OrderType == this.ORDER_TYPES.Delivery) {
                    this.PlaceOrder();
                    return;
                }
            }
        },
        OrderBtnClick() {
            //console.log('OrderBtnClick called!');
            if (!this.EditMode && (this.OrderType == this.ORDER_TYPES.TakeAway || (this.OrderType == this.ORDER_TYPES.Delivery /*&& !this.IsSelfDeliveryService*/))) {
                if (this.OrderType == this.ORDER_TYPES.TakeAway) {
                    this.OpenPaymentModal();
                }
                if (this.OrderType == this.ORDER_TYPES.Delivery/* && !this.IsSelfDeliveryService*/) {
                    this.DeliveryServiceReferenceModal();
                }
            } else {
                this.PlaceOrder();
            }
        },
        OpenPaymentModal() {
            //console.log('OpenPaymentModal called!');
            this.PaymentMode = Payment_Mode.Cash;
            $("#cashReceivingModal").modal("show");
        },
        OpenDeliveryBoyModal() {
            //console.log('OpenDeliveryBoyModal called!');
            $("#deliveryBoyModal").modal("show");
        },
        CheckoutConfirmModal() {
            //console.log('CheckoutConfirmModalModal called!');
            $("#checkoutConfirmModal").modal("show");
        },
        DeliveryServiceReferenceModal() {
            //console.log('DeliveryServiceReferenceModal called!');
            $("#deliveryServiceReferenceModal").modal("show");
        },
        CashReceivedInput() {
            //console.log('CashReceivedInput called!');
            if (isNaN(this.CashReceived)) {
                this.CashReceived = 0;
            }
            if (isNaN(this.OnOrderDiscount)) {
                this.OnOrderDiscount = 0;
            }

            this.CashReceived = parseFloat(this.CashReceived);
            this.DiscountAmount = parseFloat(this.DiscountAmount);
            if (this.PaymentMode == Payment_Mode.Card) {
                this.CardCashReceived = (this.OrderTotal + this.TaxAmount - this.DiscountAmount);
            } else {
                if (this.PaymentMode == Payment_Mode.Cash) {
                    this.CashBack = (this.CashReceived - this.OrderTotal - this.TaxAmount + this.DiscountAmount).roundOff(2);
                } else {
                    this.CashBack = 0;
                }
            }
        },
        TestEvent(e) {
            //console.log('TestEvent called!');
            //console.log("test event:", e.target);
            alert("test event occured!");
        },


        ///OnScreenKeyPadEvents
        PressOnScreenKeypadBtn(inputVal) {
            if (document.activeElement.type == 'text') {
                var modelName = $(document.activeElement).data("vue-model");

                if (inputVal == 'ok') {
                    //press tab
                    var nextItem = $(document.activeElement).next('input').focus();
                }
                else {
                    if (inputVal == 'x') {
                        var currentVal = $(document.activeElement).val();
                        if (currentVal.length >= 0) {
                            this._data[modelName] = currentVal.slice(0, -1)
                        }
                    }
                    else {
                        this._data[modelName] = Number($(document.activeElement).val() + inputVal);
                    }
                }
            }
        },
        PressOnScreenKeypadBtn1(inputVal) {
            if (document.activeElement.type == 'number') {
                var modelName = "ItemsList";
                var index = $(document.activeElement).data("index");
                var propName = "quantity";
                if (inputVal == 'ok') {
                    //press tab
                    var nextItem = $(document.activeElement).next('input').focus();
                }
                else {
                    if (inputVal == 'x') {
                        var currentVal = $(document.activeElement).val();
                        if (currentVal.length >= 0) {
                            if (index >= 0) {
                                this._data[modelName][index][propName] = currentVal.slice(0, -1)
                            }
                        }
                    }
                    else {
                        if (index >= 0) {
                            this._data[modelName][index][propName] = Number($(document.activeElement).val() + inputVal);
                        }
                    }
                    if (index >= 0) {
                        this.AddToOrderList(index);
                    }

                }
            }
        },
        PressOnScreenKeypadBtn2(inputVal) {
            if (document.activeElement.type == 'number') {
                var index = $(document.activeElement).data("index");
                var id = $(document.activeElement).data("id");
                if (inputVal == 'ok') {
                    //press tab
                    var nextItem = $(document.activeElement).next('input').focus();
                }
                else {
                    if (inputVal == 'x') {
                        var currentVal = $(document.activeElement).val();
                        if (currentVal.length >= 0) {
                            if (index >= 0) {
                                this.ItemToAddModifiers.itemModifiers[index].totalQuantity = currentVal.slice(0, -1);
                            }
                        }
                    }
                    else {
                        if (index >= 0) {
                            this.ItemToAddModifiers.itemModifiers[index].totalQuantity = $(document.activeElement).val() + inputVal;
                        }
                    }
                    if (index >= 0) {
                        this.AddModifier(id);
                    }
                }
            }
        },
        HideKeyPadModal() {
            $('.floatingKeyboard').hide();
        }
    },
    created() {
        this.Init();
    },
    watch: {
        OrderTotal: function (newVal, oldVal) {
            if (oldVal > 0 && newVal == 0) {
                //console.log('OrderTotal changed to 0');
            }
        },
        OrderType: function (newVal, oldVal) {
            //console.log('OrderType called!');
            if (newVal != this.ORDER_TYPES.DineIn) {
                $("#select-table-no").val("");
                $("#select-waiter-no").val("");
                this.TableNo = "";
            }

            if (newVal != this.ORDER_TYPES.Delivery) {
                $("#select-delivery-service").val("");
                this.DeliveryServiceNo = "";
            }
        },
        TableNo: function (newVal, oldVal) {
            //console.log('TableNo called!');
            this.TableNo = newVal;
            if (newVal == "") {
                $("#select-table-no").val();
                $("#select-waiter-no").val("");
                this.WaiterNo = "";
                $("#select-table-no").select2();
                $("#select-waiter-no").select2();
            } else {
                $("#select-table-no").val(newVal);
                $("#select-table-no").select2();
            }
        },
        WaiterNo: function (newVal, oldVal) {
            //console.log('WaiterNo called!');
            this.WaiterNo = newVal;
            $("#select-waiter-no").val(newVal);
            $("#select-waiter-no").select2();
        },
        DeliveryServiceNo: function (newVal, oldVal) {
            //console.log('DeliveryServiceNo called!');
            $("#select-delivery-service").select2();
        }
    },
    computed: {
        //EnablePlaceOrder: function () {
        //    return (this.CartList.length > 0 && this.TableNo != "" && this.WaiterNo != "");
        //},
        EnableCheckout: function () {
            //console.log('EnableCheckout called!');
            return (
                (this.CartList.length > 0 && this.OrderType === this.ORDER_TYPES.DineIn && this.TableNo !== "")
                ||
                (this.CartList.length > 0 && this.OrderType === this.ORDER_TYPES.TakeAway)
                ||
                (this.CartList.length > 0 && this.OrderType === this.ORDER_TYPES.Delivery && this.DeliveryServiceNo !== "")
            ) && !this.ErrorOnLoad;
        },
        DiscountAmount: function () {
            if (isNaN(this.OnOrderDiscount)) {
                return 0;
            }
            this.OnOrderDiscount = parseFloat(this.OnOrderDiscount);
            if (!this.DiscountInPercent) {
                return this.OnOrderDiscount;
            }
            return (this.OrderTotal * this.OnOrderDiscount * 0.01);
        },
        OrdetrTotalAfterDiscount: function () {
            return this.OrderTotal - this.DiscountAmount;
        },
        DeliveryCharges: function () {
            var deliveryCharges = 0;
            if (this.DeliveryService && this.DeliveryService.IsSelf) {
                deliveryCharges = this.DeliveryService.ServiceCharges;
                if (this.DeliveryService.ChargesInPercent) {
                    deliveryCharges = (this.OrdetrTotalAfterDiscount * this.DeliveryService.ServiceCharges) * 0.01;
                }
                return deliveryCharges;
            }
            return deliveryCharges;
        }
    }
});

$("#select-table-no").on("change",
    function () {
        posapp.TableNo = $(this).val();
    });
$("#select-waiter-no").on("change",
    function () {
        posapp.WaiterNo = $(this).val();
    });
$("#select-delivery-service").on("change",
    function () {
        posapp.DeliveryServiceReferenceNo = '';
        posapp.DeliveryServiceNo = $(this).val();
        posapp.IsSelfDeliveryService = $(this).find(':selected').data("is-self");
        //console.log(posapp.IsSelfDeliveryService);
        var deliveryService = _SearchListItemByKeyValue(posapp.DeliveryServicesList, posapp.DeliveryServiceNo, "Value");
        if (deliveryService && deliveryService.index != null) {
            posapp.DeliveryService = deliveryService.item;
        }
        else {
            posapp.DeliveryService = "";
        }
    });
$('#select-delivery-boy-no').on('change', function () {
    posapp.DeliveryBoyId = $(this).val();
});
//OnScreenKeypad jquery
$(".onscreen-keypad-btn").mousedown(function (e) { // handle the mousedown event
    e.preventDefault(); // prevent the textarea to loose focus!
});

