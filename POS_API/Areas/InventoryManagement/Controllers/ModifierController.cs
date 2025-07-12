using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Services.InventoryManagement.ModifierServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;


namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class ModifierController : BaseController
    {
        private readonly IModifierService _modifierService;
        public ModifierController( ILogger<ModifierController> logger, IAuthenticationUtilities authenticationService, IModifierService modifierService) 
            : base(logger, authenticationService) => _modifierService = modifierService;

        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            try
            {
                var model = new InvModifierDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var response = await _modifierService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Modifiers."));
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new InvModifierDto { Id = id, CompanyId = COMPANY_ID };
                var response = await _modifierService.GetDetails(model);
                return Ok(response);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting modifier data."));
            }
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(InvModifierDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _modifierService.Create(model);
                //model = (InvModifierDto)response.Model;
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Modifier."));
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(InvModifierDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _modifierService.Edit(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating Modifier."));
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new InvModifierDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _modifierService.Delete(model));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting modifier.", model: false));
            }
        }

        [HttpPost(nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(InvModifierDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _modifierService.GetSelectList(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting modifiers.", model: model));
            }
        }
    }
}
