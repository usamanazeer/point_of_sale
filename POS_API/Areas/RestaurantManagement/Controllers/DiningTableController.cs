using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.RestaurantManagement;
using POS_API.Services.RestaurantManagement.DiningTableServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.RestaurantManagement.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class DiningTableController : BaseController
    {
        private readonly IDiningTableService _diningTableService;

        public DiningTableController(ILogger<DiningTableController> logger, IAuthenticationUtilities authenticationService, IDiningTableService diningTableService) 
            : base(logger: logger, authenticationService: authenticationService) => _diningTableService = diningTableService;

        [HttpGet(template: nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            try
            {
                var model = new RestDiningTableDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var response = await _diningTableService.GetAll(model: model);
                return Ok(value: response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Dining Tables."));
            }
        }

        [HttpGet(template: nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new RestDiningTableDto { Id = id, CompanyId = COMPANY_ID };
                var response = await _diningTableService.GetDetails(model: model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Dining Table."));
            }
        }

        [HttpPost(template: nameof(Create))]
        public async Task<ActionResult> Create(RestDiningTableDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _diningTableService.Create(model: model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Dining Table."));
            }
        }

        [HttpPost(template: nameof(Edit))]
        public async Task<ActionResult> Edit(RestDiningTableDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _diningTableService.Edit(model: model);
                return Ok(value: response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating Dining Table."));
            }
        }
        [HttpPost(template: nameof(ReleaseOrOccupy))]
        public async Task<ActionResult> ReleaseOrOccupy(RestDiningTableDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                return Ok(await _diningTableService.ReleaseOrOccupy(model));
            }
            catch (Exception)
            {
                return StatusCode(statusCode: StatusCodesEnums.Error_Occured.ToInt(), value: Models.Response.Error("Api Error while Releasing/Occupying Dining Table.", model: false));
            }
        }

        [HttpGet(template: "Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new RestDiningTableDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _diningTableService.Delete(model));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting Dining Table.", model: false));
            }
        }

        [HttpPost(template: nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(RestDiningTableDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _diningTableService.GetSelectList(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while getting Dining Tables.", model: model));
            }
        }
    }
}