using Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTO.InventoryManagement;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;
using POS_API.Services.InventoryManagement.UnitServices;

namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class UnitController : BaseController
    {
        private readonly IUnitService _unitService;
        public UnitController(
            ILogger<UnitController> logger, IAuthenticationUtilities authenticationService,
            IUnitService unitService
            )
            : base(logger, authenticationService) =>
            _unitService = unitService;


        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            try
            {
                var model = new InvUnitDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var response = await _unitService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting units."));
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new InvUnitDto { Id = id, CompanyId = COMPANY_ID };
                var response = await _unitService.GetDetails(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting unit data."));
            }
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(InvUnitDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _unitService.Create(model);
                //model = (InvUnitDto)response.Model;
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Unit."));
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(InvUnitDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _unitService.Edit(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating Unit."));
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new InvUnitDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _unitService.Delete(model));
            }
            catch (Exception)
            {   
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting Unit.", model: false));
            }
        }
    }
}
