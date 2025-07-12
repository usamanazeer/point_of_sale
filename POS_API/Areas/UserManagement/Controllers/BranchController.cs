using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.UserManagement;
using POS_API.Services.UserManagement.BranchServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.UserManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class BranchController : BaseController
    {
        
        private readonly IBranchService _branchService;
        public BranchController(ILogger<BranchController> logger, IAuthenticationUtilities authenticationService ,IBranchService branchService): 
            base(logger, authenticationService) => _branchService = branchService;

        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            var response = new Response();
            var model = new BranchDto();
            try
            {
                model.Id = id;
                model.Status = status;
                model.DisplayDeleted = getDeleted??false;
                model.CompanyId = COMPANY_ID;
                response = await _branchService.GetAll(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
               response.SetError("Api Error while Getting Branches.");
               return BadRequest(response);
            }
        }
        
        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            var response = new Response();
            var model = new BranchDto();
            try
            {
                model.Id = id;
                model.CompanyId = COMPANY_ID;
                response = await _branchService.GetDetails(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting branches.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(BranchDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                response = await _branchService.Create(model);
                //model = (BranchDto)response.Model;
                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Creating Branch.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(BranchDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                response = await _branchService.Edit(model);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Updating Branch.");
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }
        
        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = new Response();
            var model = new BranchDto();
            try
            {
                model.Id = id;
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                if (await _branchService.Delete(model))
                    response.SetMessage("Branch Deleted Successfully.", StatusCodesEnums.OK, true);
                else
                    response.SetMessage("Branch Not Found.", StatusCodesEnums.Not_Found, false);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while deleting Branch.", model:false);
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }

        [HttpPost(nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(BranchDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                response = await _branchService.GetSelectList(model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Branches.");
                return BadRequest(response);
            }
        }
    }
}