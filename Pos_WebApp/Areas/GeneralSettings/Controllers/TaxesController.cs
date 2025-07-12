using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.GeneralSettings;
using Models.DTO.Reporting.Sales;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.GeneralSettings.TaxServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.GeneralSettings.Controllers
{
    [Area("GeneralSettings"), RightAuthorization, Route("[controller]")]
    public class TaxesController : BaseController
    {
        private readonly ITaxService _taxService;
        public TaxesController(ITaxService taxService):base("/Taxes") => _taxService = taxService;

        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            var model = new TaxDto();
            try
            {
                //filters
                model.Id = id;
                model.Status = status;
                if (getDeleted.HasValue) model.DisplayDeleted = getDeleted.Value;
                model = await _taxService.Get(TOKEN, model);
                
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading taxes."), IndexUrl);
            }
            
        }

        [HttpGet(nameof(Create))]
        public IActionResult Create() => View(new TaxDto());


        [JsonResponseAction, HttpPost, Route(nameof(Create), Name = "CreateTax")]
        public async Task<IActionResult> Create(TaxDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _taxService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating tax."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _taxService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading tax data."), IndexUrl);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = (await _taxService.Details(TOKEN, id));
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading tax data."), IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(TaxDto model)
        {
            
            try
            {

                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _taxService.Edit(token: TOKEN, model: model));
                
                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating tax data."));
            }
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _taxService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting tax."));
            }
        }

        [JsonResponseAction, HttpGet(nameof(GetEnabledForPos))]
        public async Task<JsonResult> GetEnabledForPos()
        {
            try
            {
                return Json(await _taxService.GetEnabledForPos(TOKEN));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while getting tax."));
            }
        }

        [HttpGet(nameof(TaxCollectionReport))]
        public async Task<IActionResult> TaxCollectionReport()
        {
            var rptTaxCollectionDto = new RptTaxCollectionDto { FromDate = DateTime.Now.Date, ToDate = DateTime.Now.Date };
            try
            {
                rptTaxCollectionDto = await _taxService.GetTaxCollectionReport(TOKEN, rptTaxCollectionDto);
                return View(rptTaxCollectionDto);
            }
            catch (Exception)
            {
                return Error(response: global::Models.Response.Error("An Error Occurred, while loading tax collection report."), backUrl: IndexUrl);
            }
        }

        [HttpPost(nameof(TaxCollectionReport))]
        public async Task<IActionResult> TaxCollectionReport(RptTaxCollectionDto rptTaxCollectionDto)
        {
            try
            {
                rptTaxCollectionDto = await _taxService.GetTaxCollectionReport(TOKEN , rptTaxCollectionDto);
                return View(rptTaxCollectionDto);
            }
            catch (Exception)
            {
                return Error(response: global::Models.Response.Error("An Error Occurred, while loading tax collection report."), backUrl: IndexUrl);
            }
            
        }
    }
}
