using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.Accounts;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.AccountsManagement.JournalServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.AccountsManagement.Controllers
{
    [Area(areaName: "AccountsManagement"), RightAuthorization, Route(template: "[controller]")]
    public class JournalController : BaseController
    {
        private readonly IJournalService _journalService;
        //private readonly IAccountsService _accountsService;
        public JournalController(IJournalService journalService) => _journalService = journalService;
        
        [HttpGet]
        public IActionResult Index() => View(new AccTransactionMasterDto());

        [HttpPost]
        public async Task<IActionResult> Index(AccTransactionMasterDto transactionMasterDto)
        {
            transactionMasterDto.IncludeDetails = true;
            transactionMasterDto.SelectVerifiedOnly = true;
            var data = await _journalService.Get(TOKEN, transactionMasterDto);
            return View(data);
        }

        [HttpGet(nameof(Add))]
        public IActionResult Add() => View(new AccTransactionMasterDto());


        [JsonResponseAction, HttpPost(template: nameof(Add), Name = "AddTransaction")]
        public async Task<IActionResult> Add(AccTransactionMasterDto transactionMasterDto)
        {
            var response = new Response();
            try
            {
                response = await _journalService.AddTransaction(TOKEN, transactionMasterDto);
            }
            catch
            {
                response.SetError("An Error Occurred, during transaction.", StatusCodesEnums.Error_Occured);
            }
            return Json(response);
        }

        [JsonResponseAction, RightAuthorization(RightName = "AddNewJournalEntry"), HttpGet(template: nameof(GetJournalEntryRow))]
        public IActionResult GetJournalEntryRow(int rowNo) => ViewComponent("JournalEntryRow", new Tuple<int, AccTransactionDetailDto>(rowNo, null));


        [HttpGet(nameof(UnverifiedEntries))]
        public async Task<IActionResult> UnverifiedEntries() => View(await _journalService.Get(TOKEN, new AccTransactionMasterDto { SelectUnverifiedOnly = true, IncludeDetails = true }));


        [JsonResponseAction, HttpGet(template: "VerifyJournalEntry/{id}")]
        public async Task<JsonResult> VerifyJournalEntry(int id)
        {
            var response = new Response();
            try
            {
                response = await _journalService.VerifyJournalEntry(token: TOKEN, id: id);
                return Json(data: response);
            }
            catch (Exception)
            {
                response.SetError("An error Occurred while verifying journal Entry.");
                return Json(data: response);
            }
        }
    }
}