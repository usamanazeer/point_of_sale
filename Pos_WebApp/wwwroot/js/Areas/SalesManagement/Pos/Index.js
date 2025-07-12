//var OrderItemsList = [];

//var spinner = `<div class="text-center">
//                <div class="spinner-border" role="status">
//                <span class="sr-only">Loading...</span>
//                </div>
//            </div>`;
//var searchTextFlag = true;
//var orderServeType = ORDER_SERVE_TYPE.TakeAway;
//var ItemsList = [];
//function HeightWidth(time) {
//    setTimeout(function () {
//        var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
//        var height = (window.innerHeight > 0) ? window.innerHeight : screen.height;
//        alert('w: ' + width + ", h: " + height);
//    }, time);
//}
//$(document).ready(function () {
    
//    //$.ajax({
//    //    url: '/pos/ApplyCategoryFilter',
//    //    type: 'GET',
//    //    success: function (res) {
//    //        //SUCCESS
//    //        var _response = res;
//    //        if (_response.responseCode != undefined) {
//    //            if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
//    //                var subCategories = _response.model[0];
//    //                var items = _response.model[1];
//    //                //$(".sub-cat-ul").html('');
//    //                //var subCatListHTML = `<li class="sub-cat-li active"><button class="sub-category" data-subcat-id="">All</button></li>`;
//    //                //if (subCategories != null) {
//    //                //    for (var i in subCategories) {
//    //                //        subCatListHTML += `<li class="sub-cat-li"><button class="sub-category" data-subcat-id="${subCategories[i].value}">${subCategories[i].text}</button></li>`;
//    //                //    }
//    //                //}
//    //                //$(".sub-cat-ul").html(subCatListHTML);
//    //                $(".items-row").html('');
//    //                var itemsListHTML = ``;
//    //                if (items != null) {
//    //                    for (var i in items) {
//    //                        ItemsList[items[i].id] = items[i];
//    //                        itemsListHTML += getItemTemplate(items[i]);
//    //                    }
//    //                }
//    //                //$(".items-row").html(itemsListHTML);
//    //                setTimeout(function () {
//    //                    customNumberInput_INIT();
//    //                    $('.input-item-qty').change(itemQuantityChangeEvent);
//    //                    $('.input-item-qty').keyup(itemQuantityChangeEvent);
//    //                }, 50);
//    //                setTimeout(function () {
//    //                    $('.sub-category').click(subCategoryClickEvent);
//    //                }, 300);
//    //            }
//    //            else if (_response.errorCode == CODES.RESPONSE_CODE_ERROR) {
//    //                sweetAlert.error({ text: _response.errorMessage });
//    //            }
//    //            else if (_response.responseCode == CODES.RESPONSE_CODE_NOTFOUND) {
//    //                sweetAlert.error({ text: _response.responseMessage });
//    //            }
//    //        }
//    //        else {
//    //            sweetAlert.error({ text: 'An error Occured.' });
//    //        }
//    //    },
//    //    error: function (error) {
//    //        console.log(error);
//    //        sweetAlert.error({ text: 'An error Occured.' });
//    //    },
//    //});
//    //HeightWidth(2000);
//    $('.btn-order-type').removeClass('bg-custom-yellow');
//    if (orderServeType == ORDER_SERVE_TYPE.DineIn) {
//        $('.btn-dinein').addClass('bg-custom-yellow');
//        $('.container-table-no').css('visibility', 'visible');
//    }
//    else if (orderServeType == ORDER_SERVE_TYPE.TakeAway) {
//        $('.btn-takeaway').addClass('bg-custom-yellow');
//    }
//    else {
//        $('.btn-takeaway').addClass('bg-custom-yellow');
//    }
//    $('.select').select2();
//});


//$('.category').click(CategoryClickEvent);
//$('.sub-category').click(subCategoryClickEvent);
////$('.input-item-qty').change(itemQuantityChangeEvent);
////$('.input-item-qty').keyup(itemQuantityChangeEvent);
//$('.btn-dinein').click(function (e) {
//    $('.btn-order-type').removeClass('bg-custom-yellow');
//    $('.btn-dinein').addClass('bg-custom-yellow');
//    $('.container-table-no').css('visibility', 'visible');
//    $('.select').select2();
//});
//$('.btn-takeaway').click(function (e) {
//    $('.btn-order-type').removeClass('bg-custom-yellow');
//    $('.btn-takeaway').addClass('bg-custom-yellow');

//    $('.container-table-no').css('visibility', 'hidden');
//});
//$('.btn-delivery').click(function (e) {
//    $('.btn-order-type').removeClass('bg-custom-yellow');
//    $('.btn-delivery').addClass('bg-custom-yellow');
//    $('.container-table-no').css('visibility', 'hidden');
//});
//$('.item-search-box').keyup(function (e) {
//    if (e.target.value == '') {
//        $('.sub-cat-li.active>button.sub-category').trigger('click');
//        return;
//    }
//    if (searchTextFlag) {
//        searchTextFlag = false;
//        var searchText = e.target.value;
//        setTimeout(function () {

//            $.ajax({
//                url: '/pos/ApplySearchTextFilter?searchText=' + searchText,
//                type: 'GET',
//                success: function (res) {
//                    //SUCCESS
//                    var _response = res;
//                    if (_response.responseCode != undefined) {
//                        if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
//                            var items = _response.model;
//                            $(".items-row").html('');
//                            var itemsListHTML = ``;
//                            if (items != null) {
//                                for (var i in items) {
//                                    itemsListHTML += getItemTemplate(items[i]);
//                                }
//                            }
//                            $(".items-row").html(itemsListHTML);
//                            //$(".sub-cat-li").removeClass('active');
//                            //e.target.parentNode.classList.add("active");
//                            setTimeout(function () {
//                                customNumberInput_INIT();
//                                $('.input-item-qty').change(itemQuantityChangeEvent);
//                            }, 50);
//                            searchTextFlag = true;
//                        }
//                        else if (_response.errorCode == CODES.RESPONSE_CODE_ERROR) {
//                            sweetAlert.error({ text: _response.errorMessage });
//                        }
//                        else if (_response.responseCode == CODES.RESPONSE_CODE_NOTFOUND) {
//                            sweetAlert.error({ text: _response.responseMessage });
//                        }
//                    }
//                    else {
//                        sweetAlert.error({ text: 'An error Occured.' });
//                    }
//                },
//                error: function (error) {
//                    console.log(error);
//                    sweetAlert.error({ text: 'An error Occured.' });
//                },
//            });
//        }, 200);
//    }
//});
//function itemQuantityChangeEvent(e) {
//    var min = parseFloat(e.target.getAttribute("min"));
//    var max = parseFloat(e.target.getAttribute("max"));
//    var value = parseFloat(e.target.value);
//    if (isNaN(value) || value < min || value > max) {
//        e.target.value = (isNaN(value) || value < min) == true ? min : max;
//        value = (isNaN(value) || value < min) == true ? min : max;
//    }
//    var itemQuantity = value;
//    var itemSalesRate = parseFloat(e.target.getAttribute("data-item-salesrate"));
//    var itemId = parseInt(e.target.getAttribute("data-item-id"));
//    var itemName = e.target.getAttribute("data-item-name");
//    var itemType = parseInt(e.target.getAttribute("data-item-type"));
//    var itemCategoryId = parseInt(e.target.getAttribute("data-item-categoryid"));
//    var itemData = ItemsList[itemId];
//    itemData.quantity = itemQuantity;
//    //var itemData = {
//    //    Id: itemId,
//    //    Name: itemName,
//    //    Type: itemType,
//    //    CategoryId: itemCategoryId,
//    //    SalesRate: itemSalesRate,
//    //    Quantity: itemQuantity
//    //};
//    AddItemToOrder(itemData);
//}
//function CategoryClickEvent(e) {
//    $('.item-search-box').val('');
//    var catName = e.target.attributes['data-cat-name'].value;
//    var itemType = e.target.attributes['data-item-type'].value;
//    var URL = '';
//    if (itemType == "" || itemType == ITEM_TYPES.NormalItem) {
//        var categoryId = e.target.attributes['data-cat-id'].value;
//        URL = '/pos/ApplyCategoryFilter?categoryId=' + categoryId;
//    }
//    else {
//        URL = '/pos/AllDealsFilter';
//    }
//    $.ajax({
//        url: URL,
//        type: 'GET',
//        success: function (res) {
//            //SUCCESS
//            var _response = res;
//            if (_response.responseCode != undefined) {
//                if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
//                    var subCategories = _response.model[0];
//                    var items = _response.model[1];
//                    //$(".sub-cat-ul").html('');
//                    //var subCatListHTML = `<li class="sub-cat-li active"><button class="sub-category" data-subcat-id="">All</button></li>`;
//                    //if (subCategories != null) {
//                    //    for (var i in subCategories) {
//                    //        subCatListHTML += `<li class="sub-cat-li"><button class="sub-category" data-subcat-id="${subCategories[i].value}">${subCategories[i].text}</button></li>`;
//                    //    }
//                    //}
//                    //$(".sub-cat-ul").html(subCatListHTML);
//                    //$(".cat-ul li").removeClass('active');
//                    //$(".category").removeClass('active');
//                    //e.target.classList.add("active");
//                    //e.target.parentNode.classList.add("active");
//                    //$('.main-cat-title').html(catName);
//                    $(".items-row").html('');
//                    var itemsListHTML = ``;
//                    if (items != null) {
//                        for (var i in items) {
//                            itemsListHTML += getItemTemplate(items[i]);
//                        }
//                    }
//                    $(".items-row").html(itemsListHTML);
//                    setTimeout(function () {
//                        customNumberInput_INIT();
//                        $('.input-item-qty').change(itemQuantityChangeEvent);
//                        $('.input-item-qty').keyup(itemQuantityChangeEvent);
//                    }, 50);
//                    setTimeout(function () {
//                        $('.sub-category').click(subCategoryClickEvent);
//                    }, 300);
//                }
//                else if (_response.errorCode == CODES.RESPONSE_CODE_ERROR) {
//                    sweetAlert.error({ text: _response.errorMessage });
//                }
//                else if (_response.responseCode == CODES.RESPONSE_CODE_NOTFOUND) {
//                    sweetAlert.error({ text: _response.responseMessage });
//                }
//            }
//            else {
//                sweetAlert.error({ text: 'An error Occured.' });
//            }
//        },
//        error: function (error) {
//            console.log(error);
//            sweetAlert.error({ text: 'An error Occured.' });
//        },
//    });
//}
//function subCategoryClickEvent(e) {

//    $('.item-search-box').val('');
//    var itemType = $('ul.cat-ul .active>.category').data('item-type');
//    var URL = '';
//    if (itemType == "" || itemType == ITEM_TYPES.NormalItem) {
//        var categoryId = $('ul.cat-ul .active>.category').data('cat-id');
//        var subcategoryId = e.target.attributes['data-subcat-id'].value;
//        URL = '/pos/ApplySubCategoryFilter?categoryId=' + categoryId + '&subcategoryId=' + subcategoryId;
//    }
//    else {
//        var subcategoryId = e.target.attributes['data-subcat-id'].value;
//        URL = '/pos/SubCategoryDealsFilter?subcategoryId=' + subcategoryId;
//    }
//    $.ajax({
//        url: URL,
//        type: 'GET',
//        success: function (res) {
//            //SUCCESS
//            var _response = res;
//            if (_response.responseCode != undefined) {
//                if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
//                    var items = _response.model;
//                    $(".items-row").html('');
//                    var itemsListHTML = ``;
//                    if (items != null) {
//                        for (var i in items) {
//                            itemsListHTML += getItemTemplate(items[i]);
//                        }
//                    }
//                    $(".items-row").html(itemsListHTML);
//                    //$(".sub-cat-li").removeClass('active');
//                    //e.target.parentNode.classList.add("active");
//                    setTimeout(function () {
//                        customNumberInput_INIT();
//                        $('.input-item-qty').change(itemQuantityChangeEvent);
//                        $('.input-item-qty').keyup(itemQuantityChangeEvent);
//                    }, 50);
//                }
//                else if (_response.errorCode == CODES.RESPONSE_CODE_ERROR) {
//                    sweetAlert.error({ text: _response.errorMessage });
//                }
//                else if (_response.responseCode == CODES.RESPONSE_CODE_NOTFOUND) {
//                    sweetAlert.error({ text: _response.responseMessage });
//                }
//            }
//            else {
//                sweetAlert.error({ text: 'An error Occured.' });
//            }
//        },
//        error: function (error) {
//            console.log(error);
//            sweetAlert.error({ text: 'An error Occured.' });
//        },
//    });
//}
//function getItemTemplate(item) {
    
//    var itemTemplate = `<div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-xs-12 mt-3">
//                                        <div class="card item-card mb-1">
//                                        <i class="fa fa-plus-circle green" title="ad-ons" style="float: right; position: absolute; display: block; right: -1px; font-size: x-large; cursor: pointer; top: 11px; "></i>
//                                            <div class="card-header text-center">
//                                                <img src="${item.imageUrl}" alt="Brek" width="100" height="100" class="rounded-circle gradient-mint">
                                                
//                                            </div>
//                                            <div class="card-content">
//                                                <div class="card-body text-center item-card-body">
//                                                    <h6 class="card-title">${item.name}</h6>
//                                                    <p class="category text-gray font-small-5"> ${item.fullName}</p>
                                                    
//                                                    <div class="row">
//                                                        <div class="col-12 mt-1 pl-4">
//                                                            <div class="row grey justify-content-center">`;

//    if (item.itemType == ITEM_TYPES.NormalItem) {
//        if (item.manageStock == true && item.allowBackOrder == false) {

//            itemTemplate += `<div class="custom-number-input">
//                <input id="input-item-qty-${item.id}" type="number" min="0" max="${item.remainingInventory}" step="1" value="${(OrderItemsList[item.id] == undefined ? 0 : OrderItemsList[item.id].quantity)}" class="input-item-qty" data-item-id="${item.id}" data-item-name="${item.fullName} " data-item-salesrate="${item.finalSalesRate}" data-item-type="${item.itemType}" data-item-categoryid="${item.categoryId}" />
//            </div>`;
//        }
//        else {
//            itemTemplate += `<div class="custom-number-input">
//                <input id="input-item-qty-${item.id}" type="number" min="0" max="999999" step="1" value="${(OrderItemsList[item.id] == undefined ? 0 : OrderItemsList[item.id].quantity)}" class="input-item-qty" data-item-id="${item.id}" data-item-name="${item.fullName}" data-item-salesrate="${item.finalSalesRate}" data-item-type="${item.itemType}" data-item-categoryid="${item.categoryId}" />
//            </div>`;
//        }
//    }
//    else {
//        itemTemplate += `<div class="custom-number-input">
//            <input id="input-item-qty-${item.id}" type="number" min="0" max="999999" step="1" value="${(OrderItemsList[item.id] == undefined ? 0 : OrderItemsList[item.id].quantity)}" class="input-item-qty" data-item-id="${item.id}" data-item-name="${item.fullName}" data-item-salesrate="${item.finalSalesRate} " data-item-type="${item.itemType} " data-item-categoryid="${item.categoryId}" />
//        </div>`;
//    }
//    itemTemplate += `</div>
//                        </div>
//                    </div>
//                </div>
//            </div>
//        </div>
//        <div class="text-center">
//            <div class=" badge bg-custom-yellow round rate-badge">
//                ${item.finalSalesRate}
//            </div>
//        </div>
//    </div>`;





//    return '';
//}
//function AddItemToOrder(item) {
//    console.log(item.itemModifiers);
//    item.subTotal = Math.round((item.quantity * item.finalSalesRate) * 100 + Number.EPSILON) / 100;
//    item.quantity = Math.round((item.quantity) * 100 + Number.EPSILON) / 100;
//    if (item.quantity == 0) {
//        RemoveOrderItem(item.id);
//    }
//    else {
//        if ($(`#order-item-${item.id}`)[0] == undefined) {
//            var orderItemLi = `<li id="order-item-${item.id}" class="list-group-item ordered-item-li">
                                    
//                                        <div class="col-6 h-ft-content d-inline-block">${item.fullName}</div>
//                                        <div class="col-2 h-ft-content d-inline-block text-center order-item-quantity">x<span>${item.quantity}</span></div>
//                                        <div class="col-3 h-ft-content d-inline-block text-center order-item-subtotal"><span class="badge bg-secondary">${(item.subTotal)}</span></div>
//                                        <div class="col-1 h-ft-content d-inline-block text-right"><button class="btn btn-raised btn-outline-danger btn-min-width mr-1 btn-remove-order-item" onclick="RemoveOrderItem('${item.id}')">x</button></div>
                                        
//                                     <div class="col-12 addons-ul adons-item-${item.id}">`;

//            for (var i = 0; i < item.itemModifiers.length; i++) {
                
//                var itemModifier = item.itemModifiers[i];
//                if (!itemModifier.isMandatory) {
//                    var totalCharges = Math.round((itemModifier.quantity * item.quantity * itemModifier.modifier.modifierCharges) * 100 + Number.EPSILON) / 100;
//                    console.log('itemModifiers', itemModifier);
//                    orderItemLi += `<div class="row modifier-${itemModifier.id}">
//                     <div class="col-6 h-ft-content d-inline-block">${itemModifier.modifier.name}</div>
//                                        <div class="col-2 h-ft-content d-inline-block text-center order-item-quantity">x<span>${itemModifier.quantity}</span></div>
//                                        <div class="col-3 h-ft-content d-inline-block text-center order-item-subtotal"><span class="badge bg-secondary">${totalCharges}</span></div>
//                         <!--<div class="col-1 h-ft-content d-inline-block text-right"><button class="btn btn-raised btn-outline-danger btn-min-width mr-1 btn-remove-order-item" onclick="RemoveOrderItem('${item.id}')">x</button></div>-->
//                                </div>`;
//                }
                
//            }
//            orderItemLi += `</div></li>`;
//            if (item.itemType == ITEM_TYPES.DealItem) {
//                $('#order-item-deals').append(orderItemLi);
//                $(`#order-category-container-deals`).css('display', 'block');
//            }
//            else {
//                $(`#order-item-category-${item.categoryId}`).append(orderItemLi);
//                $(`#order-category-container-${item.categoryId}`).css('display', 'block');
//            }
//        }
//        else {
//            $(`#order-item-${item.id}>div.order-item-quantity>span`).html(item.quantity);
//            $(`#order-item-${item.id}>div.order-item-subtotal>span`).html(item.subTotal);
//        }
//        OrderItemsList[item.id] = item;
//    }
//    TotalOrderAmount();
//}
//function RemoveOrderItem(itemId) {

//    var Item = OrderItemsList[itemId];
//    if (Item != undefined) {
//        delete OrderItemsList[itemId];
//        $(`#order-item-${Item.id}`).remove();
//        if (Item.itemType == ITEM_TYPES.DealItem) {
//            if ($(`#order-item-deals>li`).length == 0) {
//                $(`#order-category-container-deals`).css('display', 'none');
//            }
//        }
//        else {
//            if ($(`#order-item-category-${Item.categoryId}>li`).length == 0) {
//                $(`#order-category-container-${Item.categoryId}`).css('display', 'none');
//            }
//        }

//        if ($(`#input-item-qty-${Item.id}`)[0] != undefined) {
//            $(`#input-item-qty-${Item.id}`).val($(`#input-item-qty-${Item.id}`)[0].attributes['min'].value);
//        }
//        TotalOrderAmount();
//    }
//}
//function TotalOrderAmount() {
//    var total = 0;
//    var itemCount = 0;
//    for (var i in OrderItemsList) {
//        var item = OrderItemsList[i];
//        total += item.subTotal;
//        itemCount++;
//    }
//    $('#total-amount').html(parseFloat(total).toFixed(2));
//    if (itemCount > 0) {
//        $('.btn-checkout').removeAttr('disabled');
//        $('.btn-place-order').removeAttr('disabled');
//    }
//    else {
//        $('.btn-checkout').attr('disabled', 'disabled');
//        $('.btn-place-order').attr('disabled', 'disabled');
//    }
//}