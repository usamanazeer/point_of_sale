using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Services.InventoryManagement.ColorServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class ColorController : BaseController
    {
        private readonly IColorService _colorService;
        public ColorController(ILogger<ColorController> logger, IAuthenticationUtilities authenticationService, IColorService colorService )
            : base(logger, authenticationService) => _colorService = colorService;

        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            try
            {
                var model = new InvColorDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var response = await _colorService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting colors."));
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new InvColorDto{ Id = id, CompanyId = COMPANY_ID };
                var response = await _colorService.GetDetails(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting color data."));
            }
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(InvColorDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _colorService.Create(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Color."));
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(InvColorDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _colorService.Edit(model);
                return Ok(response);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating Color."));
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new InvColorDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _colorService.Delete(model));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting Color.", model: false));
            }
        }
    }
}