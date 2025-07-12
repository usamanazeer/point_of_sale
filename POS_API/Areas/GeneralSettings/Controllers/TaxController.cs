using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.GeneralSettings;
using Models.DTO.Reporting.Sales;
using POS_API.Services.GeneralSettings.TaxServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.GeneralSettings.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class TaxController : BaseController
    {
        private readonly ITaxService _texService;


        public TaxController(ILogger<TaxController> logger, IAuthenticationUtilities authenticationService, ITaxService texService)
            : base(logger: logger, authenticationService: authenticationService) => _texService = texService;

        [HttpGet(template: nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            var response = new Response();
            try
            {
                var model = new TaxDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                response = await _texService.GetAll(model: model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting taxes.");
                return BadRequest(error: response);
            }
        }

        [HttpGet(template: nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            var response = new Response();
            try
            {
                var model = new TaxDto { Id = id, CompanyId = COMPANY_ID };
                response = await _texService.GetDetails(model: model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting taxes.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(GetTaxCollectionReport))]
        public async Task<ActionResult> GetTaxCollectionReport(RptTaxCollectionDto rptTaxCollectionDto)
        {
            var response = new Response();
            //var model = new TaxDto();
            try
            {
                //model.CompanyId = COMPANY_ID;
                //model.BranchId = BRANCH_ID;
                response = await _texService.GetTaxCollectionReport(rptTaxCollectionDto);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting tax report.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(Create))]
        public async Task<ActionResult> Create(TaxDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;

                response = await _texService.Create(model: model);
                //model = (TaxDTO)response.Model;

                return StatusCode(statusCode: response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode,value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Creating Tax.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(Edit))]
        public async Task<ActionResult> Edit(TaxDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                response = await _texService.Edit(model: model);
                return Ok(value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Updating Tax.");
                return StatusCode(statusCode:  StatusCodesEnums.Error_Occured.ToInt(), value: response);
            }
        }


        [HttpGet(template: "Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = new Response();
            
            try
            {
                var model = new TaxDto{ Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                if (await _texService.Delete(model: model))
                    response.SetMessage("Tax Deleted Successfully.", model: true);
                else
                    response.SetMessage("Tax Not Found.", StatusCodesEnums.Not_Found, false);
                return Ok(value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while deleting tex.",model:false);
                return StatusCode(statusCode:  StatusCodesEnums.Error_Occured.ToInt(),value: response);
            }
        }

        [HttpGet(template: nameof(GetEnabledForPos))]
        public async Task<ActionResult> GetEnabledForPos()
        {
            var response = new Response();
            try
            {
                response = await _texService.GetEnabledForPos(COMPANY_ID);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting taxes."); 
                return BadRequest(error: response);
            }
        }

    }
}