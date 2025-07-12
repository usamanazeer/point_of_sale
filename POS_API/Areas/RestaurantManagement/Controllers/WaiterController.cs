using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.RestaurantManagement;
using Models.Enums;
using POS_API.Services.RestaurantManagement.WaitersServices;
using POS_API.Utilities.Authentication;

namespace POS_API.Areas.RestaurantManagement.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class WaiterController : BaseController
    {
        private readonly IWaitersService _waitersService;


        public WaiterController(ILogger<WaiterController> logger, IAuthenticationUtilities authenticationService, IWaitersService waitersService) 
            : base(logger: logger, authenticationService: authenticationService) => _waitersService = waitersService;


        [HttpGet(template: nameof(Get))]
        public async Task<ActionResult> Get(int? id,int? status,bool? getDeleted = null)
        {
            try
            {
                var model = new RestWaiterDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var response = await _waitersService.GetAll(model: model);
                return Ok(value: response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Waiters."));
            }
        }


        [HttpGet(template: nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new RestWaiterDto { Id = id, CompanyId = COMPANY_ID };
                var response = await _waitersService.GetDetails(model: model);
                return Ok(value: response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Waiter."));
            }
        }

        [HttpPost(template: nameof(Create))]
        public async Task<ActionResult> Create(RestWaiterDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _waitersService.Create(model: model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Waiter."));
            }
        }


        [HttpPost(template: nameof(Edit))]
        public async Task<ActionResult> Edit(RestWaiterDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _waitersService.Edit(model: model);
                return Ok(value: response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating Waiter."));
            }
        }


        [HttpGet(template: "Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new RestWaiterDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _waitersService.Delete(model));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting Waiter.", model: false));
            }
        }

        [HttpPost(template: nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(RestWaiterDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _waitersService.GetSelectList(model: model);
                return Ok(value: response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Waiters List."));
            }
        }
    }
}