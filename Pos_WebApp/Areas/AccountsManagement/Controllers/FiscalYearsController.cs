using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.Accounts;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.AccountsManagement.FiscalYearServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.AccountsManagement.Controllers
{
    [Area(areaName: "AccountsManagement"), RightAuthorization, Route(template: "[controller]")]
    public class FiscalYearsController : BaseController
    {
        private readonly IFiscalYearService _fiscalYearService;

        public FiscalYearsController( IFiscalYearService fiscalYearService )=> _fiscalYearService = fiscalYearService;
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new AccFiscalYearDto();
            try
            {
                model = await _fiscalYearService.Get(TOKEN, model);
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading Fiscal Years.");
            }
            return View(model);
        }

        [HttpGet(template: "Create")]
        public IActionResult Create() => View(model: new AccFiscalYearDto());


        [HttpPost, Route(template: "Create", Name = "CreateFiscalYear")]
        public async Task<IActionResult> Create(AccFiscalYearDto fiscalYearDto)
        {
            try
            {
                //Save Vendor
                if (ModelState.IsValid)
                    fiscalYearDto = await _fiscalYearService.Create(token: TOKEN, model: fiscalYearDto);
                else
                    fiscalYearDto.Response.SetError("Please Fill the form carefully.", StatusCodesEnums.Invalid_State.ToInt());
            }
            catch (Exception)
            {
                fiscalYearDto.Response.SetError("An Error Occurred, while creating Fiscal Year.");
            }
            return View(model: fiscalYearDto);
        }
    }
}