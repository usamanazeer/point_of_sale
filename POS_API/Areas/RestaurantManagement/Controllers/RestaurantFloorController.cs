using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.RestaurantManagement;
using POS_API.Services.RestaurantManagement.RestaurantFloorsServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.RestaurantManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class RestaurantFloorController : BaseController
    {
        private readonly IRestaurantFloorsService _restaurantFloorsService;
        public RestaurantFloorController(ILogger<RestaurantFloorController> logger, IAuthenticationUtilities authenticationService
            , IRestaurantFloorsService restaurantFloorsService
            ) : base(logger, authenticationService) =>
            _restaurantFloorsService = restaurantFloorsService;


        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            try
            {
                var model = new RestRestaurantFloorsDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var response = await _restaurantFloorsService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Floors."));
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new RestRestaurantFloorsDto { Id = id, CompanyId = COMPANY_ID };
                var response = await _restaurantFloorsService.GetDetails(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Floor."));
            }
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(RestRestaurantFloorsDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _restaurantFloorsService.Create(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Floor."));
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(RestRestaurantFloorsDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _restaurantFloorsService.Edit(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating Floor."));
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new RestRestaurantFloorsDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _restaurantFloorsService.Delete(model));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting Floor.", model: false));
            }
        }

        [HttpPost(nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(RestRestaurantFloorsDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _restaurantFloorsService.GetSelectList(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting Floors.", model: model));
            }
        }
    }
}