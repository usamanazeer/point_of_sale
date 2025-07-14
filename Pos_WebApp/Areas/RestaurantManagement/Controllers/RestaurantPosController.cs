using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.SalesManagement;
using Newtonsoft.Json;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.DeliveryService.DeliveryBoyServices;
using Pos_WebApp.Services.DeliveryService.DeliveryServiceVendorServices;
using Pos_WebApp.Services.InventoryManagement.CategoryServices;
using Pos_WebApp.Services.RastaurantManagement.WaitersServices;
using Pos_WebApp.Services.RestaurantManagement.DiningTableServices;
using Pos_WebApp.Services.SalesManagement.OrderServices;
using Pos_WebApp.Services.SalesManagement.PosServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.RestaurantManagement.Controllers
{
    [Area(areaName: "RestaurantManagement"), SkipSideBar]
    [Route(template: "[controller]")]
    public class RestaurantPosController : BaseController
    {
        private readonly IDeliveryBoyService _deliveryBoyService;
        private readonly IDeliveryServiceVendorService _deliveryServiceVendor;
        private readonly IDiningTableService _diningTableService;
        private readonly IMainCategoryService _mainCategoryService;
        private readonly IOrderService _orderService;
        private readonly IPosService _posService;
        private readonly IWaitersService _waitersService;
        public RestaurantPosController(IPosService posService,
                                       IMainCategoryService mainCategoryService,
                                       IDiningTableService diningTableService,
                                       IWaitersService waitersService,
                                       IDeliveryServiceVendorService deliveryServiceVendor,
                                       IDeliveryBoyService deliveryBoyService,
                                       IOrderService orderService)
        {
            _posService = posService;
            _mainCategoryService = mainCategoryService;
            _diningTableService = diningTableService;
            _waitersService = waitersService;
            _orderService = orderService;
            _deliveryServiceVendor = deliveryServiceVendor;
            _deliveryBoyService = deliveryBoyService;
        }

        [RightAuthorization, HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var getMainCategoriesTask = _mainCategoryService.Get(token: TOKEN,
                                                                     model: new InvCategoryDto
                                                                            {
                                                                                DisplayOnPos = true
                                                                            });
                var getDiningTableTask = _diningTableService.GetSelectList(tOKEN: TOKEN);
                var getWaitersTask = _waitersService.GetSelectList(tOKEN: TOKEN);
                var getDeliveryServiceVendorsTask = _deliveryServiceVendor.GetSelectList(token: TOKEN);
                var getDeliveryBoysTask = _deliveryBoyService.GetSelectList(token: TOKEN);
                await Task.WhenAll(getMainCategoriesTask,
                                   getDiningTableTask,
                                   getWaitersTask,
                                   getDeliveryServiceVendorsTask,
                                   getDeliveryBoysTask);

                ViewBag.Categories = getMainCategoriesTask.Result.MainCategories;
                ViewBag.Ser_Categories = JsonConvert.SerializeObject(ViewBag.Categories);

                ViewBag.Ser_Tables = JsonConvert.SerializeObject(value: getDiningTableTask.Result);

                ViewBag.Ser_Waiters = JsonConvert.SerializeObject(value: getWaitersTask.Result);

                ViewBag.Ser_DeliveryServices = JsonConvert.SerializeObject(value: getDeliveryServiceVendorsTask.Result);
                ViewBag.Ser_DeliveryBoys = JsonConvert.SerializeObject(value: getDeliveryBoysTask.Result);
            }

            catch (Exception)
            {
                ViewBag.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                ViewBag.ErrorMessage = "An Error Occurred, while loading data.";
            }
            return View();
        }


        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpGet(template: "ApplyCategoryFilter")]
        public async Task<JsonResult> ApplyCategoryFilter(int? categoryId)
        {
            var response = new Response();
            try
            {
                response = await _posService.ApplyCategoryFilter(token: TOKEN,
                                                                 categoryId: categoryId);
                return Json(data: response);
            }
            catch (Exception)
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while getting data.";
                return Json(data: response);
            }
        }


        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpGet(template: "ApplySubCategoryFilter")]
        public async Task<JsonResult> ApplySubCategoryFilter(int? categoryId, int? subcategoryId)
        {
            var response = new Response();
            try
            {
                response = await _posService.ApplySubCategoryFilter(token: TOKEN,categoryId: categoryId,subcategoryId: subcategoryId);
                return Json(data: response);
            }
            catch (Exception)
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while getting data.";
                return Json(data: response);
            }
        }

        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpGet(template: "AllDealsFilter")]
        public async Task<JsonResult> AllDealsFilter()
        {
            var response = new Response();
            try
            {
                response = await _posService.AllDealsFilter(token: TOKEN);
                return Json(data: response);
            }
            catch (Exception)
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while getting data.";
                return Json(data: response);
            }
        }


        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpGet(template: "SubCategoryDealsFilter")]
        public async Task<JsonResult> SubCategoryDealsFilter(int? subcategoryId)
        {
            var response = new Response();
            try
            {
                response = await _posService.SubCategoryDealsFilter(token: TOKEN, subcategoryId: subcategoryId);
                return Json(data: response);
            }
            catch (Exception)
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while getting data.";
                return Json(data: response);
            }
        }


        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpGet(template: "ApplySearchTextFilter")]
        public async Task<JsonResult> ApplySearchTextFilter(string searchText)
        {
            var response = new Response();
            try
            {
                response = await _posService.ApplySearchTextFilter(token: TOKEN,
                                                                   searchText: searchText);
                return Json(data: response);
            }
            catch (Exception)
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while getting data.";
                return Json(data: response);
            }
        }


        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpGet(template: "GetOrders")]
        public async Task<JsonResult> GetOrders(int? orderStatus, int? orderType)
        {
            var response = new Response();
            try
            {
                response = await _orderService.GetResponse(token: TOKEN,
               model: new SalesOrderMasterDto
                      {
                          //OrderTypeId = orderType,
                          OrderStatusId = orderStatus
                      });
                return Json(data: response);
            }
            catch (Exception ex)
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while getting data.";
                return Json(data: response);
            }
        }


        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpGet(template: "GetOrderDetails/{id}")]
        public async Task<JsonResult> GetOrderDetails(int id)
        {
            var response = new Response();
            try
            {
                response = await _orderService.DetailsResponse(token: TOKEN,
                                                               id: id);
                return Json(data: response);
            }
            catch (Exception)
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while getting data.";
                return Json(data: response);
            }
        }


        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpPost(template: "PlaceOrder")]
        public async Task<JsonResult> PlaceOrder( /*[FromBody]*/ SalesOrderMasterDto model)
        {
            var response = new Response();
            try
            {
                response = await _orderService.PlaceOrderResponse(token: TOKEN,
                                                                  model: model);
                return Json(data: response);
            }
            catch
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while placing order.";
                return Json(data: response);
            }
        }


        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpPost(template: "UpdateOrder")]
        public async Task<JsonResult> UpdateOrder( /*[FromBody]*/ SalesOrderMasterDto model)
        {
            var response = new Response();
            try
            {
                response = await _orderService.UpdateOrderResponse(token: TOKEN,
                                                                   model: model);
                return Json(data: response);
            }
            catch
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while updating order.";
                return Json(data: response);
            }
        }


        [JsonResponseAction, RightAuthorization(RightName = "RestaurantPosFront"), HttpPost(template: "Checkout")]
        public async Task<JsonResult> Checkout( /*[FromBody] */ SalesOrderMasterDto model)
        {
            var response = new Response();
            try
            {
                response = await _orderService.Checkout(token: TOKEN,
                                                        model: model);
                return Json(data: response);
            }
            catch
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while Checking-out order.";
                return Json(data: response);
            }
        }
    }
}