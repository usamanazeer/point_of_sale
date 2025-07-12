using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.Accounts;
using POS_API.Services.AccountsManagement.FiscalYearServices;
using POS_API.Utilities.Authentication;

namespace POS_API.Areas.AccountsManagement.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class FiscalYearController : BaseController
    {
        private readonly IFiscalYearService _fiscalYearService;
        public FiscalYearController(ILogger<BaseController> logger,
                 IAuthenticationUtilities authenticationService
        , IFiscalYearService fiscalYearService) : base(logger,authenticationService) =>
            _fiscalYearService = fiscalYearService;

        [HttpGet(template: nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status)
        {
            var response = new Response();
            var model = new AccFiscalYearDto();
            try
            {
                model.Id = id;
                model.Status = status;
                model.CompanyId = COMPANY_ID;

                response = await _fiscalYearService.GetAll(model: model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError($"Api Error while Getting Fiscal Years.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(Create))]
        public async Task<ActionResult> Create(AccFiscalYearDto fiscalYearDto)
        {
            var response = new Response();
            try
            {
                fiscalYearDto.CompanyId = COMPANY_ID;
                fiscalYearDto.CreatedBy = USER_ID;
                fiscalYearDto.CreatedOn = DateTime.Now;
                response = await _fiscalYearService.Create(fiscalYearDto: fiscalYearDto);
                return StatusCode(statusCode: response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Creating Fiscal Year.");
                return BadRequest(error: response);
            }
        }
    }
}
