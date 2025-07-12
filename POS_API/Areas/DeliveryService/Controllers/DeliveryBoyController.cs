using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.DeliveryService;
using POS_API.Services.DeliveryService.DeliveryBoyServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.DeliveryService.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class DeliveryBoyController : BaseController
    {
        private readonly IDeliveryBoyService _deliveryBoyService;


        public DeliveryBoyController(ILogger<BaseController> logger, IAuthenticationUtilities authenticationService, IDeliveryBoyService deliveryBoyService) 
            : base(logger: logger, authenticationService: authenticationService) 
            => _deliveryBoyService = deliveryBoyService;

        [HttpGet(template: nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            var response = new Response();
            try
            {
                var model = new DeliDeliveryBoyDto { Id = id, Status = status, DisplayDeleted = getDeleted.HasValue && getDeleted.Value, CompanyId = COMPANY_ID };
                response = await _deliveryBoyService.GetAll(model: model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError($"Api Error while Getting Delivery Boy.");
                return BadRequest(error: response);
            }
        }

        [HttpGet(template: nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            var response = new Response();
            try
            {
                var model = new DeliDeliveryBoyDto { Id = id, CompanyId = COMPANY_ID };
                response = await _deliveryBoyService.GetDetails(model: model);
                return !response.ErrorOccured? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting Delivery Boy.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template:nameof(Create))]
        public async Task<ActionResult> Create(DeliDeliveryBoyDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                response = await _deliveryBoyService.Create(model: model);
                return StatusCode(statusCode: response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode,value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Creating Delivery Boy.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(Edit))]
        public async Task<ActionResult> Edit(DeliDeliveryBoyDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;

                response = await _deliveryBoyService.Edit(model: model);
                return Ok(value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Updating Delivery Boy.");
                return StatusCode(statusCode: StatusCodesEnums.Error_Occured.ToInt(), value: response);
            }
        }

        [HttpGet(template: "Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = new Response();
            
            try
            {
                var model = new DeliDeliveryBoyDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                if (await _deliveryBoyService.Delete(model: model))
                    response.SetMessage("Delivery Boy Deleted Successfully.", model: true);
                else
                    response.SetMessage("Delivery Boy Not Found.", StatusCodesEnums.Not_Found, false);
                return Ok(value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while deleting Delivery Boy.", model:false);
                return StatusCode(statusCode: StatusCodesEnums.Error_Occured.ToInt(), value: response);
            }
        }

        [HttpPost(template: nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(DeliDeliveryBoyDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                response = await _deliveryBoyService.GetSelectList(model: model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Delivery Boys."); 
                return BadRequest(error: response);
            }
        }
    }
}