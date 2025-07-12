using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.DeliveryService;
using POS_API.Services.DeliveryService.DeliveryServiceVendorServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.DeliveryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class DeliveryServiceVendorController : BaseController
    {
        private readonly IDeliveryServiceVendorService _deliveryServiceVendorService;
        public DeliveryServiceVendorController
            (ILogger<DeliveryServiceVendorController> logger, IAuthenticationUtilities authenticationService, IDeliveryServiceVendorService deliveryServiceVendorService) 
            : base(logger, authenticationService) => _deliveryServiceVendorService = deliveryServiceVendorService;

        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            var response = new Response();
            
            try
            {
                var model = new DeliDeliveryServiceVendorDto{ Id = id, Status = status, DisplayDeleted = getDeleted.HasValue && getDeleted.Value, CompanyId = COMPANY_ID };
                response = await _deliveryServiceVendorService.GetAll(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Delivery Service Vendors.");
                return BadRequest(response);
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            var response = new Response();
            var model = new DeliDeliveryServiceVendorDto();
            try
            {
                model.Id = id;
                model.CompanyId = COMPANY_ID;

                response = await _deliveryServiceVendorService.GetDetails(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Delivery Service Vendor.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(DeliDeliveryServiceVendorDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                response = await _deliveryServiceVendorService.Create(model);
                //model = (DeliDeliveryServiceVendorDTO)response.Model;
                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
                
            }
            catch (Exception)
            {
                response.SetError("Api Error while Creating Delivery Service Vendor.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(DeliDeliveryServiceVendorDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                response = await _deliveryServiceVendorService.Edit(model);
                return Ok(response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Updating Delivery Service Vendor.");
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = new Response();
            try
            {
                var model = new DeliDeliveryServiceVendorDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                if (await _deliveryServiceVendorService.Delete(model))
                    response.SetMessage("Delivery Service Vendor Deleted Successfully.", model: true);
                else
                    response.SetMessage("Delivery Service Vendor Not Found.", StatusCodesEnums.Not_Found, false);
                return Ok(value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while deleting Delivery Service Vendor.", model:false);
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }

        [HttpPost(nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(DeliDeliveryServiceVendorDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                response = await _deliveryServiceVendorService.GetSelectList(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Delivery Service Vendors.");
                return BadRequest(response);
            }
        }

        [HttpGet(nameof(IsSelfExist))]
        public async Task<ActionResult> IsSelfExist()
        {
            var response = new Response();
            try
            {
                if (await _deliveryServiceVendorService.IsSelfExist(COMPANY_ID))
                    response.SetMessage("Self Delivery Service Vendor Exists.", model: true);
                else
                    response.SetMessage("Self Delivery Service Vendor Not Found.", StatusCodesEnums.Not_Found, false);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while looking for Self Delivery Service Vendor.", model:false);
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }
    }
}