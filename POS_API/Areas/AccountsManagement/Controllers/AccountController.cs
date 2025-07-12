using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.Accounts;
using POS_API.Services.AccountsManagement.AccountServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.AccountsManagement.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAuthenticationUtilities authenticationService, IAccountService accountService) 
            : base(logger: logger, authenticationService: authenticationService) => _accountService = accountService;

        [HttpGet(template: nameof(GetAccountTypes))]
        public async Task<ActionResult> GetAccountTypes()
        {
            var response = new Response();
            try
            {
                response = await _accountService.GetAccountTypes();
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Account Types.");
                return BadRequest(error: response);
            }
        }

        [Obsolete, HttpGet(template: nameof(GetAllAccounts))]
        public async Task<ActionResult> GetAllAccounts()
        {
            var response = new Response();
            try
            {
                response = await _accountService.GetAllAccounts(companyId: COMPANY_ID);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Account Types.");
                return BadRequest(error: response);
            }
        }


        [HttpGet(template: nameof(GetAccountsSelectList))]
        public async Task<IActionResult> GetAccountsSelectList(bool skipSystemMade = false, bool skipIfParent = false,
                                                               bool selectIfParent = false, bool selectForManualTransactions = false, 
                                                               bool selectBankAccountsOnly = false)
        {
            var response = new Response();
            try
            {
                response = await _accountService.GetAccountsSelectList(companyId: COMPANY_ID, skipSystemMade, skipIfParent, selectIfParent, selectForManualTransactions, selectBankAccountsOnly);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Accounts.");
                return BadRequest(error: response);
            }
        }


        [HttpGet(template: nameof(GetChartOfAccounts))]
        public async Task<ActionResult> GetChartOfAccounts()
        {
            var response = new Response();
            try
            {
                response = await _accountService.GetChartOfAccount(companyId: COMPANY_ID);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Chart Of Accounts.");
                return BadRequest(error: response);
            }
        }

        [HttpGet(template: "GetDetails")]
        public async Task<ActionResult> Details(int id)
        {
            var response = new Response();
            var accAccountDto = new AccAccountDto();
            try
            {
                accAccountDto.Id = id;
                accAccountDto.CompanyId = COMPANY_ID;
                response = await _accountService.GetDetails(accAccountDto: accAccountDto);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Account Details.");
                return BadRequest(error: response);
            }
        }


        [HttpPost(template: nameof(Create))]
        public async Task<ActionResult> Create(AccAccountDto accAccountDto)
        {
            var response = new Response();
            try
            {
                accAccountDto.CompanyId = COMPANY_ID;
                accAccountDto.CreatedBy = USER_ID;
                accAccountDto.CreatedOn = DateTime.Now;
                response = await _accountService.Create(accAccountDto: accAccountDto);
                return StatusCode(statusCode: response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Creating Account."); 
                return BadRequest(error: response);
            }
        }


        [HttpPost(template: nameof(Edit))]
        public async Task<ActionResult> Edit(AccAccountDto accAccountDto)
        {
            var response = new Response();
            try
            {
                accAccountDto.CompanyId = COMPANY_ID;
                accAccountDto.ModifiedBy = USER_ID;
                accAccountDto.ModifiedOn = DateTime.Now;
                response = await _accountService.Edit(accAccountDto: accAccountDto);
                return Ok(value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Updating Account.");
                return StatusCode(statusCode:  StatusCodesEnums.Error_Occured.ToInt(), value: response);
            }
        }
    }
}