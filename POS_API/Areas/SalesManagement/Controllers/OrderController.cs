using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.SalesManagement;
using POS_API.Services.SalesManagement.OrderServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

using POS_API.Utilities.SignalR.SalesHubs;
using Microsoft.AspNetCore.SignalR;

namespace POS_API.Areas.SalesManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    // ReSharper disable once InconsistentNaming
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IHubContext<SalesHub> _salesHubContext;
        public OrderController(ILogger<BaseController> logger, IAuthenticationUtilities authenticationService
            ,IOrderService orderService , IHubContext<SalesHub> salesHubContext
            ) : base(logger, authenticationService)
        {
            _orderService = orderService;
            _salesHubContext = salesHubContext;
        }
        
        [HttpPost(nameof(Get))]
        public async Task<ActionResult> Get(SalesOrderMasterDto filter)
        {
            var response = new Response();
            //SalesOrderMasterDTO model = new SalesOrderMasterDTO();
            try
            {
                //model.Id = filter.Id;
                //model.Status = filter.Status;
                //model.DisplayDeleted = filter.DisplayDeleted != null && (bool)filter.DisplayDeleted;
                //model.DisplayDeleted = filter.DisplayDeleted;
                //model.OrderTypeId = filter.OrderTypeId;
                //model.OrderStatus = filter.OrderStatus;
                filter.CompanyId = COMPANY_ID;
                response = await _orderService.GetAll(model: filter);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError ("Api Error while Getting Orders.");
                return BadRequest(response);
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            var response = new Response();
            var model = new SalesOrderMasterDto();
            try
            {
                model.Id = id;
                model.CompanyId = COMPANY_ID;
                response = await _orderService.GetDetails(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Order.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(PlaceOrder))]
        public async Task<ActionResult> PlaceOrder(SalesOrderMasterDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                response = await _orderService.PlaceOrder(model);
                return Ok(response);
            }
            catch (Exception  )
            {
                response.SetError("Api Error while Placing Order.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(SalesOrderMasterDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                response = await _orderService.Edit(model);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Updating Order.");
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }

        [Obsolete, HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = new Response();
            var model = new SalesOrderMasterDto();
            try
            {

                model.Id = id;
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                if (await _orderService.Delete(model))
                    response.SetMessage("Order Deleted Successfully.", StatusCodesEnums.OK, model: true);
                else
                    response.SetMessage("Order Not Found.", StatusCodesEnums.Not_Found, model: false);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while deleting Order.", model:false);
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }

        [HttpGet(nameof(ChangeOrderStatus))]
        public async Task<ActionResult> ChangeOrderStatus(int id, int status)
        {
            var response = new Response();
            var model = new SalesOrderMasterDto();
            try
            {
                model.Id = id;
                model.OrderStatusId = status;
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                if (await _orderService.ChangeOrderStatus(model))
                    response.SetError("Order Status Changed Successfully.", StatusCodesEnums.OK, model: true);
                else
                    response.SetError("Order Not Found.", StatusCodesEnums.Not_Found, model: false);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Changing Order Status.", model:false);
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }
        [HttpGet(nameof(CancelOrder))]
        public async Task<ActionResult> CancelOrder(int id)
        {
            var response = new Response();
            var model = new SalesOrderMasterDto();
            try
            {
                model.Id = id;
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                if (await _orderService.CancelOrder(model))
                    response.SetMessage("Order Cancelled Successfully.", StatusCodesEnums.OK, model: true);
                else
                    response.SetMessage("Order Not Found.", StatusCodesEnums.Not_Found, model: false);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Cancelling Order.", model:false);
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }

        [HttpPost(nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(SalesOrderMasterDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                response = await _orderService.GetSelectList(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting Orders.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Checkout))]
        public async Task<ActionResult> Checkout(SalesOrderMasterDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                response = await _orderService.Checkout(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Checking-out order.");
                return BadRequest(response);
            }
        }
        [HttpGet("PrintReceipt/{id}")]
        public async Task<ActionResult> PrintReceipt(int id)
        {
            var response = new Response();
            var model = new SalesOrderMasterDto();
            try
            {
                model.Id = id;
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                if (await _orderService.PrintReceipt(model))
                    response.SetMessage("Receipt Printed Successfully.", StatusCodesEnums.OK, model: true);
                else
                    response.SetMessage("Order Not Found.", StatusCodesEnums.Not_Found, model: false);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while printing receipt.", model: false);
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }

        [HttpPost(nameof(GetOrderStatusSelectList))]
        public async Task<IActionResult> GetOrderStatusSelectList(SalesOrderStatusDto filter)
        {
            var response = new Response();
            try
            {
                //filter.CompanyId = COMPANY_ID;
                response = await _orderService.GetOrderStatusSelectList(filter);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting OrdersStatus List.");
                return BadRequest(response);
            }
        }
    }
}