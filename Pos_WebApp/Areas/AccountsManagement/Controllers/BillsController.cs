using System;
using Models;
using Models.DTO.Accounts;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StatusCodesEnums = Models.Enums.StatusCodes;
using Pos_WebApp.Services.AccountsManagement.BillsServices;
using Pos_WebApp.Services.AccountsManagement.AccountsServices;

namespace Pos_WebApp.Areas.AccountsManagement.Controllers
{
    [Area(areaName: "AccountsManagement"), RightAuthorization, Route(template: "[controller]")]
    public class BillsController : BaseController
    {
        private readonly IBillsService _billsService;
        private readonly IAccountsService _accountsService;

        public BillsController(IBillsService billsService, IAccountsService accountsService)
        {
            _billsService = billsService;
            _accountsService = accountsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? status, bool? excludePaidBills = false)
        {
            var model = new BillDto();
            try
            {
                //filters
                model.Id = id;
                model.Status = status;
                if (excludePaidBills != null) model.ExcludePaidBills = (bool)excludePaidBills;
                model = await _billsService.Get(token: TOKEN, model);
            }
            catch (Exception )
            {
                model.Response.SetError("An Error Occurred, while loading bills.");
            }
            return View( model);
        }

        [HttpGet(template: "PayBill/{id}")]
        public async Task<IActionResult> PayBill(int id)
        {
            var model = new BillDto();
            try
            {
                model.Id = id;
                var responseModel = await _billsService.Details(token: TOKEN,id: id);
                model = responseModel;
                if (model.Id is null)
                {
                    if (model.Response.ResponseCode != StatusCodesEnums.Not_Found.ToInt())
                        return Error(response: model.Response, backUrl: "/Bills");

                    model.Response.ResponseMessage = "Bill Not Found.";
                    return NotFound(response: model.Response, backUrl: "/Bills");
                }
                ViewBag.BankAccounts =  await _accountsService.GetAccountsSelectList(TOKEN, selectBankAccountsOnly: true);
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading bill data.");
                return Error(response: model.Response, backUrl: "/Bills");
            }
            if (model.Response.ResponseCode == StatusCodesEnums.OK.ToInt())
                model.Response = null;
            return View(model: model);
        }

        [HttpPost, Route(template: "PayBill/{id}", Name = "PayBillPost")]
        public async Task<IActionResult> PayBill(BillDto model)
        {
            if (model.Id.HasValue)
            {
                model.BillPayment.BillId = model.Id.Value;
                try
                {
                    if (ModelState.IsValid && model.Id > 0)
                    {
                        model = await _billsService.PayBill(token: TOKEN, model: model.BillPayment);
                        if (model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                        {
                            model.Response.ResponseMessage = "Bill Not Found.";
                            return NotFound(response: model.Response, backUrl: "/Bills");
                        }
                        if (model.Response.ErrorCode == StatusCodesEnums.Error_Occured.ToInt())
                            return Error(response: model.Response, backUrl: "/Bills");
                    }
                }
                catch (Exception)
                {
                    model.Response.SetError("An Error Occurred, while updating bill data.");
                }
            }
            else
            {
                throw new Exception("Bill Id is Null.");
            }
            return View(model);
        }

        [JsonResponseAction, RightAuthorization(RightName = nameof(UserRights.PayBill)), HttpPost(template: nameof(GetBillsByFilters))]
        public async Task<JsonResult> GetBillsByFilters(BillDto model)
        {
            var response = new Response();
            try
            {
                response = await _billsService.GetBillsByFilters(token: TOKEN, model);
                return Json(data: response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while getting data.");
                return Json(data: response);
            }
        }
    }
}