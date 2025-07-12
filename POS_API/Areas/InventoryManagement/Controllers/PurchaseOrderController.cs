using System;
using Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTO.InventoryManagement;
using POS_API.Utilities.Authentication;
using Microsoft.AspNetCore.Authorization;
using StatusCodesEnums = Models.Enums.StatusCodes;
using POS_API.Services.InventoryManagement.PurchaseOrderServices;

namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class PurchaseOrderController : BaseController
    {
        private readonly IPOService _pOService;
        public PurchaseOrderController(ILogger<PurchaseOrderController> logger, IAuthenticationUtilities authenticationService, IPOService pOService) 
            : base(logger, authenticationService) 
            => _pOService = pOService;

        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            try
            {
                var model = new InvPoMasterDto { Id = id, Status = status, DisplayDeleted = getDeleted != null && (bool) getDeleted, CompanyId = COMPANY_ID };
                var response = await _pOService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting POs."));
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new InvPoMasterDto { Id = id, CompanyId = COMPANY_ID };
                var response = await _pOService.GetDetails(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting PO data."));
            }
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(InvPoMasterDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _pOService.Create(model);
                //model = (InvPoMasterDto)response.Model;
                return Ok(response);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating PO."));
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(InvPoMasterDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _pOService.Edit(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating PO."));
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new InvPoMasterDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _pOService.Delete(model));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting PO.", model: false));
            }
        }

        [HttpPost(nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(InvPoMasterDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _pOService.GetSelectList(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting POs.", model: model));
            }
        }
    }
}
