using Models;
using System;
using System.Linq;
using Models.Enums;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using POS_API.Utilities.Authentication;
using Microsoft.AspNetCore.Authorization;
using StatusCodesEnums = Models.Enums.StatusCodes;
using POS_API.Services.UserManagement.CompanyServices;

namespace POS_API.Areas.UserManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ILogger<CompanyController> logger, IAuthenticationUtilities authenticationService, ICompanyService companyService)
            :base(logger, authenticationService) => _companyService = companyService;

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(CompanyDto model)
        {
            var response = new Response();
            try
            {
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                model = await _companyService.Edit(model);
                if (model != null) 
                {
                    response.ResponseName = "Success";
                    response.SetMessage("Updated Successfully.", StatusCodesEnums.Updated, model:model);
                    return Ok(response);
                }
                response.SetError("Company Not Found.", StatusCodesEnums.Not_Found);
                return NotFound(response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred! Failed to Update Company.", StatusCodesEnums.Not_Modified);
                return BadRequest(model);
            }
        }

        [HttpPost(nameof(SaveLogo))]
        public async Task<ActionResult> SaveLogo([FromForm] BaseModel model, [FromForm] IFormFile file = null)
        {
            var response = new Response();
            try
            {
                var model1 = new CompanyDto { Id = model.Id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                response = await _companyService.SaveLogo(model1, file);
                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Updating Company.",model:false);
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Get))]
        public async Task<ActionResult> Get(SearchModel model)
        {
            try
            {
                var data = await _companyService.Get(model);
                return data.Any()
                    // ReSharper disable once RedundantCast
                    ? (ActionResult) Ok((await _companyService.Get(model)).ToList()) : NotFound("No Company Found!");
            }
            catch (Exception)
            {
                return BadRequest(model);
            }
        }
        [HttpGet(nameof(GetByUserId))]
        public async Task<ActionResult> GetByUserId(int userId)
        {
            
            try
            {
                var data  = await _companyService.GetByUserId(userId);
                if (data!=null)
                    return Ok(Models.Response.Message("OK", StatusCodesEnums.OK,data));

                return NotFound(Models.Response.Error("No Company Found!", StatusCodesEnums.Not_Found));
            }
            catch (Exception ex)
            {
                return BadRequest(Models.Response.Error(ex.Message, StatusCodesEnums.Bad_Request));
            }
        }
        [HttpGet(nameof(GetCompanyById))]
        public async Task<ActionResult> GetCompanyById()
        {
            var response = new Response();
            try
            {
                var data  = await _companyService.GetCompanyById(COMPANY_ID);
                if (data is not null)
                {
                    response.SetMessage("OK",model: data);
                    return Ok(response);
                }
                response.SetError("Company Not Found!", StatusCodesEnums.Not_Found);
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.SetError(ex.Message,StatusCodesEnums.Bad_Request);
                return BadRequest(response);
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var res = await _companyService.ChangeStatus(new CompanyDto { Id = id, ModifiedBy = USER_ID, Status = StatusTypes.Delete.ToInt() });
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                // ReSharper disable once RedundantCast
                return res ? (ActionResult) Ok(res) : BadRequest("Company Not Found!");
            }
            catch (Exception)
            {
                return BadRequest("An Error Occurred!");
            }
        }
        [HttpPost("Active/{id}")]
        public async Task<ActionResult> Active(int id)
        {
            try
            {
                var res = await _companyService.ChangeStatus(new CompanyDto { Id = id, ModifiedBy = USER_ID, Status = StatusTypes.Active.ToInt() });
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                // ReSharper disable once RedundantCast
                return res ? (ActionResult) Ok(res) : BadRequest("Company Not Found!");
            }
            catch (Exception)
            {
                return BadRequest("An Error Occurred!");
            }
        }
        [HttpPost("Inactive/{id}")]
        public async Task<ActionResult> InActive(int id)
        {
            try
            {
                var res = await _companyService.ChangeStatus(new CompanyDto { Id = id, ModifiedBy = USER_ID, Status = StatusTypes.InActive.ToInt() });
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                // ReSharper disable once RedundantCast
                return res ? (ActionResult) Ok(res) : BadRequest("Company Not Found!");
            }
            catch (Exception)
            {
                return BadRequest("An Error Occurred!");
            }
        }
        [HttpPost(nameof(SetupPrinters))]
        public async Task<ActionResult> SetupPrinters(CompanyDto companyDto)
        {
            try
            {
                var res = await _companyService.SetupPrinters(companyDto);
                var response = new Response
                {
                   Model = res, 
                   ResponseCode = res? StatusCodesEnums.OK.ToInt():0,
                   ResponseMessage = res? "Printer(s) Saved Successfully": "",
                   ErrorCode = res == false ? StatusCodesEnums.Error_Occured.ToInt():0, 
                   ErrorMessage = res == false ? "An Error Occurred": ""
                };
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                // ReSharper disable once RedundantCast
                return res ? (ActionResult) Ok(response) : BadRequest(response);
            }
            catch (Exception)
            {
                return BadRequest(Models.Response.Error("An Error Occurred"));
            }
        }
    }
}