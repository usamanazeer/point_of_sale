using System;
using Models;
using System.Linq;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.SalesManagement;
using StatusCodesEnums = Models.Enums.StatusCodes;
using Pos_WebApp.Services.SalesManagement.OrderServices;


namespace Pos_WebApp.Areas.SalesManagement.Controllers
{
    [Area("SalesManagement"), Route("[controller]")]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService) => _orderService = orderService;


        [RightAuthorization(RightName = "Orders"), HttpGet(nameof(GetAll))]
        public async Task<JsonResult> GetAll(SalesOrderMasterDto model)
        {
            var res = await _orderService.Get(TOKEN, model);
            var items = res.Orders;

            var totalRecords = 0;
            if (items.Any())
                totalRecords = items[0].totalRecords;
            return Json(new { model.sEcho, iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords, aaData = items });
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new SalesOrderMasterDto();
            try
            {
                model.FromDate = DateTime.Now.Date;
                model.ToDate = DateTime.Now.Date;
                try
                {
                    ViewBag.OrdersStatusList = await _orderService.GetOrderStatusSelectList(TOKEN);
                }
                catch (Exception)
                {
                    //ignore
                }
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while placing order.");
            }
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(SalesOrderMasterDto model)
        {
            try
            {
                model = await _orderService.Get(TOKEN, model);
                try
                {
                    ViewBag.OrdersStatusList = await _orderService.GetOrderStatusSelectList(TOKEN);
                }
                catch (Exception )
                {
                    //ignore
                }
                ViewBag.OrdersStatusList = await _orderService.GetOrderStatusSelectList(TOKEN);
            }
            catch (Exception )
            {
                model.Response.SetError("An Error Occurred, while placing order.");
            }
            return View(model);
        }

        [JsonResponseAction, HttpGet("PrintReceipt/{orderId}")]
        public async Task<IActionResult> PrintReceipt(int orderId)
        {
            var response = new Response();
            try
            {
                response = await _orderService.PrintReceipt(TOKEN, orderId);
                return Json(response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while printing receipt.");
                return Json(response);
            }
        }
        [HttpGet("PlaceOrder")]
        public IActionResult PlaceOrder() => View(new SalesOrderMasterDto());


        [HttpPost, Route("PlaceOrder", Name = "PlaceOrder")]
        public async Task<IActionResult> PlaceOrder(SalesOrderMasterDto model)
        {
            try
            {
                //Save Vendor
                if (ModelState.IsValid)
                    model = await _orderService.PlaceOrder(TOKEN, model);
                else
                    model.Response.SetError("Please Fill the form carefully.", StatusCodesEnums.Invalid_State);
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while placing order.");
            }
            return View(model);
        }

        [HttpGet("SalesReturn/{id}")]
        public async Task<IActionResult> SalesReturn(int id) 
        {
            var model = new SalesOrderMasterDto();
            try
            {
                //filters
                model.Id = id;
                var responseModel = await _orderService.Details(TOKEN, id);
                if (responseModel != null)
                {
                    model = responseModel;
                    model.Response = responseModel.Response;
                }
                else
                {
                    if (model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                    {
                        model.Response.ResponseMessage = "Order Not Found.";
                        return NotFound(model.Response, "/Orders");
                    }
                    return Error(model.Response, "/Orders");
                }
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while Returning Sales.");
                return Error(model.Response, "/Orders");
            }
            return View(model);
        }

        [JsonResponseAction]
        [HttpGet("Cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = new Response();
            try
            {
                response = await _orderService.CancelOrder(TOKEN, orderId);
                if (response.ErrorOccured)
                    response.ErrorMessage = "An Error Occurred, while Cancelling Order.";
                else
                    response.ResponseMessage = "Order Successfully Cancelled.";
                return Json(response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while Cancelling Order.");
                return Json(response);
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var model = new SalesOrderMasterDto();
            try
            {
                //filters
                model.Id = id;
                var responseModel = await _orderService.Details(TOKEN, id);
                if (responseModel != null)
                {
                    model = responseModel;
                    model.Response = responseModel.Response;
                }
                else
                {
                    if (model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                    {
                        model.Response.ResponseMessage = "Order Not Found.";
                        return NotFound(model.Response, "/Orders");
                    }
                    return Error(model.Response, "/Orders");
                }
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading Order data.");
                return Error(model.Response, "/Orders");
            }
            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new SalesOrderMasterDto();
            try
            {
                model.Id = id;
                var responseModel = await _orderService.Details(TOKEN, id);
                model = responseModel;
                if (responseModel.Id == null)
                {
                    if(model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                    {
                        model.Response.ResponseMessage = "Order Not Found.";
                        return NotFound(model.Response, "/Orders");
                    }
                    return Error(model.Response, "/Orders");
                }
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading Order data.");
                return Error(model.Response, "/Orders");
            }
            return View(model);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(SalesOrderMasterDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                {
                    model = await _orderService.UpdateOrder(TOKEN, model);
                    if (model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                    {
                        model.Response.ResponseMessage = "Order Not Found.";
                        return NotFound(model.Response, "/Orders");
                    }
                    if (model.Response.ErrorCode == StatusCodesEnums.Error_Occured.ToInt())
                        return Error(model.Response, "/Orders");
                }
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while updating Order data.");
            }
            return View(model);
        }

        [Obsolete, JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            var response = new Response();
            try
            {
                response = await _orderService.Delete(TOKEN, id);
                return Json(response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while deleting Order.");
                return Json(response);
            }
        }

        [JsonResponseAction, HttpGet(nameof(ChangeOrderStatus))]
        public async Task<JsonResult> ChangeOrderStatus(int id, int status)
        {
            var response = new Response();
            try
            {
                response = await _orderService.ChangeOrderStatus(TOKEN, id, status);
                return Json(response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while Changing Order Status.");
                return Json(response);
            }
        }

        [JsonResponseAction, RightAuthorization(RightName = "Orders"), HttpPost(nameof(GetOrders))]
        public async Task<JsonResult> GetOrders(SalesOrderMasterDto filters)
        {
            var response = new Response();
            try
            {
                var data = await _orderService.GetResponse(TOKEN, filters);
                return Json(data);
            }
            catch (Exception)   
            {
                response.SetError("An Error Occurred, while Changing Order Status.");
                return Json(response);
            }
        }
    }
}
