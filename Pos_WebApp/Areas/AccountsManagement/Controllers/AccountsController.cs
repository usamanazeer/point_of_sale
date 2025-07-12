using Models;
using System;
using Models.DTO.Accounts;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StatusCodesEnums = Models.Enums.StatusCodes;
using StatusCodes = Microsoft.AspNetCore.Http.StatusCodes;
using Pos_WebApp.Services.AccountsManagement.AccountsServices;

namespace Pos_WebApp.Areas.AccountsManagement.Controllers
{
    [Area(areaName: "AccountsManagement"), RightAuthorization, Route(template: "[controller]")]
    public class AccountsController : BaseController
    {
        private readonly IAccountsService _accountsService;
        public AccountsController(IAccountsService accountsService) => _accountsService = accountsService;
        public async Task<IActionResult> Index()
        {
            var chartOfAccounts = await _accountsService.GetChartOfAccounts(token: TOKEN);
            return View(model: chartOfAccounts);
        }

        [HttpGet(template: nameof(Create))]
        public async Task<IActionResult> Create()
        {
            var getAccountsTypesTask = _accountsService.GetAccountTypes(token: TOKEN);
            var getAccountsTask = _accountsService.GetAccountsSelectList(token: TOKEN, skipSystemMade: true, selectIfParent: true);

            await Task.WhenAll(getAccountsTypesTask, getAccountsTask);

            ViewBag.AccountTypes = getAccountsTypesTask.Result.AccountTypes;
            ViewBag.Accounts = getAccountsTask.Result;

            return View(model: new AccAccountDto());
        }

        [JsonResponseAction, HttpPost(template: nameof(Create), Name = "CreateAccount")]
        public async Task<IActionResult> Create(AccAccountDto accountDto)
        {
            var response = new Response();
            try
            {
                response = await _accountsService.Create(TOKEN, accountDto);
            }
            catch
            {
                response.SetError("An Error Occurred while creating Account.");
            }
            return Json(response);
        }

        [HttpGet(template: "Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var accountDto = new AccAccountDto();
            try
            {
                accountDto.Id = id;
                accountDto = await _accountsService.Details(token: TOKEN, id: id);
                if (!accountDto.Id.HasValue && accountDto.Response.ResponseCode ==  StatusCodesEnums.Not_Found.ToInt())
                {
                    accountDto.Response.ResponseMessage = "Account Not Found.";
                    return NotFound(response: accountDto.Response, backUrl: "/Accounts");
                }
                if (!accountDto.Id.HasValue)
                    return Error(response: accountDto.Response, backUrl: "/Accounts");
            }
            catch (Exception)
            {
                accountDto.Response.SetError($"An Error Occurred, while loading Account data.");
                return Error(response: accountDto.Response, backUrl: "/Accounts");
            }

            return View(model: accountDto);
        }

        [JsonResponseAction, HttpPost(template: nameof(Edit))]
        public async Task<IActionResult> Edit(AccAccountDto accountDto)
        {
            var response = new Response();

            try
            {
                if (ModelState.IsValid && accountDto.Id > 0)
                {
                    response = await _accountsService.Edit(token: TOKEN, accountDto: accountDto);
                    if (accountDto.Response.ResponseCode == StatusCodes.Status404NotFound)
                        response.SetError("Account Not Found.", StatusCodes.Status404NotFound);
                }
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while updating Account data.", StatusCodes.Status404NotFound);
            }
            return Json(response);
        }

        [HttpGet(template: "Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var accAccountDto = new AccAccountDto();
            try
            {
                //filters
                accAccountDto.Id = id;
                var responseModel = await _accountsService.Details(token: TOKEN, id: id);
                if (responseModel != null)
                {
                    accAccountDto = responseModel;
                    accAccountDto.Response = responseModel.Response;
                }
                else
                {
                    if (accAccountDto.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                    {
                        accAccountDto.Response.ResponseMessage = "Account Not Found.";
                        return NotFound(response: accAccountDto.Response, backUrl: "/Accounts");
                    }
                    return Error(response: accAccountDto.Response, backUrl: "/Accounts");
                }
            }
            catch (Exception)
            {
                accAccountDto.Response.SetError("An Error Occurred, while loading Account data.");
                return Error(response: accAccountDto.Response, backUrl: "/Accounts");
            }
            return View(model: accAccountDto);
        }
    }
}