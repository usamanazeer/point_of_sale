using Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTO.Reporting.Accounts;
using POS_API.Utilities.Authentication;
using Microsoft.AspNetCore.Authorization;
using POS_API.Services.Reporting.AccountsReportingServices;

namespace POS_API.Areas.Reporting.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class AccountReportingController : BaseController
    {
        private readonly IAccountsReportingService _accountsReportingService;
        public AccountReportingController(ILogger<BaseController> logger, IAuthenticationUtilities authenticationService, IAccountsReportingService accountsReportingService) 
            : base(logger,authenticationService) => _accountsReportingService = accountsReportingService;


        [HttpPost(template: nameof(GetLedger))]
        public async Task<ActionResult> GetLedger(RptAccountsLedgerDto accLedgerPosting)
        {
            var response = new Response();
            try
            {
                accLedgerPosting.CompanyId = COMPANY_ID;
                response = await _accountsReportingService.GetLedger(accLedgerPosting);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting Account Ledger.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(GetTrialBalance))]
        public async Task<ActionResult> GetTrialBalance(RptAccountsTrialBalanceDto rptTrialBalanceDto)
        {
            var response = new Response();
            try
            {
                rptTrialBalanceDto.CompanyId = COMPANY_ID;
                response = await _accountsReportingService.GetTrialBalance(rptTrialBalanceDto);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting Trial Balance Data.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(GetIncomeStatement))]
        public async Task<ActionResult> GetIncomeStatement(RptAccountsIncomeStatementDto incomeStatementDto)
        {
            var response = new Response();
            try
            {
                incomeStatementDto.CompanyId = COMPANY_ID;
                response = await _accountsReportingService.GetIncomeStatement(incomeStatementDto);
                return !response.ErrorOccured? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting Income Statement Data.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(GetBalanceSheet))]
        public async Task<ActionResult> GetBalanceSheet(RptAccountBalanceSheetDto  rptAccountBalanceSheetDto)
        {
            var response = new Response();
            try
            {
                rptAccountBalanceSheetDto.CompanyId = COMPANY_ID;
                response = await _accountsReportingService.GetBalanceSheet(rptAccountBalanceSheetDto);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting Balance Sheet.");
                return BadRequest(error: response);
            }
        }
    }
}
