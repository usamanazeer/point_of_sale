using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.DTO.Reporting.Accounts;
using Models.DTO.ViewModels.SelectList.AccountsManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.AccountsManagement.AccountsServices;
using Pos_WebApp.Services.Reporting.AccountsReportingServices;

namespace Pos_WebApp.Areas.Reporting.Controllers
{
    [Area(areaName: "Reporting"), Route(template: "[controller]")]
    public class AccountsReportsController : BaseController
    {
        private readonly IAccountsReportingService _accountsReportingService;
        private readonly IAccountsService _accountsService;


        public AccountsReportsController(IAccountsReportingService accountsReportingService,
                                         IAccountsService accountsService)
        {
            _accountsReportingService = accountsReportingService;
            _accountsService = accountsService;
        }


        [RightAuthorization, HttpGet(template: "Ledger")]
        public async Task<IActionResult> Ledger()
        {
            var accountsList = await _accountsService.GetAccountsSelectList(token: TOKEN);
            var accountsSelectList = new SelectList(items: accountsList ?? new List<Account_SLM>(),
                                                    dataValueField: "Value",
                                                    dataTextField: "Text");
            ViewBag.AccountsSelectList = accountsSelectList;
            return View(model: new RptAccountsLedgerDto());
        }


        [RightAuthorization, HttpPost(template: "Ledger")]
        public async Task<IActionResult> Ledger(RptAccountsLedgerDto rptLedgerDto)
        {
            var getAccountsListTask = _accountsService.GetAccountsSelectList(token: TOKEN);
            var getLedgerDataTask = _accountsReportingService.GetLedger(token: TOKEN,
                                                                        rptLedgerDto: rptLedgerDto);

            await Task.WhenAll(getLedgerDataTask,
                               getAccountsListTask);
            var accountsSelectList = new SelectList(items: getAccountsListTask.Result ?? new List<Account_SLM>(),
                                                    dataValueField: "Value",
                                                    dataTextField: "Text");
            ViewBag.AccountsSelectList = accountsSelectList;

            var ledgerData = getLedgerDataTask.Result;

            return View(model: ledgerData);
        }


        [RightAuthorization(RightName = "Ledger"), JsonResponseAction, HttpPost(template: "GetLedger")]
        public async Task<IActionResult> GetLedger(RptAccountsLedgerDto rptLedgerDto)
        {
            var res = await _accountsReportingService.GetLedgerResponse(token: TOKEN,
                                                                        rptLedgerDto: rptLedgerDto);
            return Json(data: res);
        }


        [RightAuthorization, HttpGet(template: "TrialBalance")]
        public async Task<IActionResult> TrialBalance()
        {
            var rptTrialBalanceDto = new RptAccountsTrialBalanceDto
                                     {
                                         OnDate = DateTime.Now
                                     };
            rptTrialBalanceDto = await _accountsReportingService.GetTrialBalance(token: TOKEN,
                                                                                 rptTrialBalanceDto:
                                                                                 rptTrialBalanceDto);
            return View(model: rptTrialBalanceDto);
        }


        [RightAuthorization, HttpPost(template: "TrialBalance")]
        public async Task<IActionResult> TrialBalance(RptAccountsTrialBalanceDto rptTrialBalanceDto)
        {
            rptTrialBalanceDto = await _accountsReportingService.GetTrialBalance(token: TOKEN,
                                                                                 rptTrialBalanceDto:
                                                                                 rptTrialBalanceDto);
            return View(model: rptTrialBalanceDto);
        }


        [RightAuthorization(RightName = "TrialBalance"), JsonResponseAction, HttpPost(template: "GetTrialBalance")]
        public async Task<IActionResult> GetTrialBalance(RptAccountsTrialBalanceDto rptTrialBalanceDto)
        {
            var res = await _accountsReportingService.GetTrialBalanceResponse(token: TOKEN,
                                                                              rptTrialBalanceDto: rptTrialBalanceDto);
            return Json(data: res);
        }


        [RightAuthorization, HttpGet(template: "IncomeStatement")]
        public async Task<IActionResult> IncomeStatement()
        {
            var rptTrialBalanceDto = new RptAccountsIncomeStatementDto
            {
                                         FromDate = DateTime.Now.Date,
                                         ToDate = DateTime.Now.Date
            };
            rptTrialBalanceDto = await _accountsReportingService.GetIncomeStatement(token: TOKEN,
                                                                                    rptIncomeStatementDto:
                                                                                 rptTrialBalanceDto);
            return View(model: rptTrialBalanceDto);
        }


        [RightAuthorization, HttpPost(template: "IncomeStatement")]
        public async Task<IActionResult> IncomeStatement(RptAccountsIncomeStatementDto rptIncomeStatementDto)
        {
            rptIncomeStatementDto = await _accountsReportingService.GetIncomeStatement(token: TOKEN,
                                                                                       rptIncomeStatementDto:
                                                                                 rptIncomeStatementDto);
            return View(model: rptIncomeStatementDto);
        }


        [RightAuthorization(RightName = "IncomeStatement"), JsonResponseAction,
         HttpPost(template: "GetIncomeStatement")]
        public async Task<IActionResult> GetIncomeStatement(RptAccountsIncomeStatementDto rptIncomeStatementDto)
        {
            var res = await _accountsReportingService.GetIncomeStatementResponse(token: TOKEN,
                                                                                 rptIncomeStatementDto: rptIncomeStatementDto);
            return Json(data: res);
        }


        [RightAuthorization, HttpGet(template: "BalanceSheet")]
        public async Task<IActionResult> BalanceSheet()
        {
            var rptAccountBalanceSheetDto = new RptAccountBalanceSheetDto
            {
                                         OnDate = DateTime.Now.Date
            };
            rptAccountBalanceSheetDto = await _accountsReportingService.GetBalanceSheet(token: TOKEN,
                                                                                 rptAccountBalanceSheetDto:
                                                                                 rptAccountBalanceSheetDto);
            return View(model: rptAccountBalanceSheetDto);
        }


        [RightAuthorization, HttpPost(template: "BalanceSheet")]
        public async Task<IActionResult> BalanceSheet(RptAccountBalanceSheetDto rptAccountBalanceSheetDto)
        {
            rptAccountBalanceSheetDto = await _accountsReportingService.GetBalanceSheet(token: TOKEN,
                                                                                 rptAccountBalanceSheetDto:
                                                                                 rptAccountBalanceSheetDto);
            return View(model: rptAccountBalanceSheetDto);
        }


        [RightAuthorization(RightName = "BalanceSheet"), JsonResponseAction, HttpPost(template: "GetBalanceSheet")]
        public async Task<IActionResult> GetBalanceSheet(RptAccountBalanceSheetDto rptAccountBalanceSheetDto)
        {
            var res = await _accountsReportingService.GetBalanceSheetResponse(token: TOKEN, rptAccountBalanceSheetDto: rptAccountBalanceSheetDto);
            return Json(data: res);
        }
    }
}