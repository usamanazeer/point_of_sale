using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using POS_API.Services.InventoryManagement.VendorServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;


namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class VendorController : BaseController
    {
        private readonly IVendorService _vendorService;
        public VendorController(ILogger<VendorController> logger, IAuthenticationUtilities authenticationService, IVendorService vendorService) 
            : base(logger: logger, authenticationService: authenticationService) =>
            _vendorService = vendorService;


        [HttpGet(template: nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            
            try
            {
                var model = new InvVendorDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var response = await _vendorService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Vendors Data."));
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new InvVendorDto { Id = id, CompanyId = COMPANY_ID };
                var response = await _vendorService.GetDetails(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Vendor."));
            }
        }

        [HttpPost(template:nameof(Create))]
        public async Task<ActionResult> Create(InvVendorDto model) 
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _vendorService.Create(model);
                return Ok(response);
            }
            catch /*(Exception ex)*/
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(),Models.Response.Error("Api Error while Creating Vendor."));
            }
        }

        [HttpPost(template: nameof(Edit))]
        public async Task<ActionResult> Edit(InvVendorDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _vendorService.Edit(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating Vendor."));
            }
        }

        [HttpGet(template: "Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new InvVendorDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _vendorService.Delete(model));
            }
            catch /*(Exception)*/
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting user.", model: false));
            }
        }
    
        [HttpPost(template: "Active/{id}")]
        public async Task<ActionResult> Active(int id)
        {
            try
            {
                var model = new InvVendorDto { Id = id, ModifiedBy = USER_ID, Status = StatusTypes.Active.ToInt() };
                var res = await _vendorService.ChangeStatus(model);
                // ReSharper disable once RedundantCast
                return res ? (ActionResult) Ok(true) : BadRequest("Company Not Found!");
            }
            catch (Exception)
            {
                return BadRequest("An Error Occurred!");
            }
        }

        [HttpPost(template: "Inactive/{id}")]
        public async Task<ActionResult> InActive(int id)
        {
            try
            {
                var model = new InvVendorDto { Id = id, ModifiedBy = USER_ID, Status = StatusTypes.InActive.ToInt() };
                var res = await _vendorService.ChangeStatus(model);
                if (res) return Ok(true);

                return BadRequest("Company Not Found!");
            }
            catch (Exception)
            {
                return BadRequest("An Error Occurred!");
            }
        }

        [HttpPost(template: nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(InvVendorDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _vendorService.GetSelectList(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                return BadRequest(Models.Response.Error("Api Error while Getting Vendors."));
            }
        }
    }
}
