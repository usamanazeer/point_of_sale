using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.PurchaseOrderServices;
using Pos_WebApp.Services.InventoryManagement.VendorServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), Route("[controller]")]
    public class PurchaseOrdersController : BaseController
    {
        private readonly IPOService _pOService;
        private readonly IVendorService _vendorService;
        public PurchaseOrdersController(IPOService pOService, IVendorService vendorService) : base("/PurchaseOrders")
        {
            _pOService = pOService;
            _vendorService = vendorService;
        }

        [RightAuthorization]
        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = new InvPoMasterDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false };
                model = await _pOService.Get(TOKEN, model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading POs."), IndexUrl);
            }
        }

        [RightAuthorization, HttpGet(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            ViewBag.Vendors = await _vendorService.GetSelectList(TOKEN);
            return View(new InvPoMasterDto());
        }

        [JsonResponseAction, RightAuthorization, HttpPost, Route(nameof(Create), Name = "CreatePO")]
        public async Task<IActionResult> Create(InvPoMasterDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _pOService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating PO."));
            }
        }

        [RightAuthorization, HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _pOService.Details(TOKEN, id);
                ViewBag.Vendors = await _vendorService.GetSelectList(TOKEN);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading PO data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(InvPoMasterDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _pOService.Edit(TOKEN, model));

                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating PO."));
            }
        }

        [RightAuthorization, HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _pOService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading PO."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _pOService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting PO."));
            }
        }

        [JsonResponseAction, RightAuthorization(RightName = "PurchaseOrders"), HttpGet("AddPo_PoDetailsRow")]
        public async Task<IActionResult> AddPo_PoDetailsRow(int rowNo)=> await Task.FromResult(ViewComponent("AddPo_PoDetailsRow", new Tuple<int, InvPoDetailsDto>(rowNo, null)));

        [JsonResponseAction, HttpGet(nameof(GetSelectList))]
        public async Task<JsonResult> GetSelectList(int? vendorId) 
        {
            try
            {
                var filter = new InvPoMasterDto { VendorId = vendorId };
                var response = await _pOService.GetSelectListResponse(TOKEN, filter);
                return Json(response);
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while getting POs."));
            }
        }

        [JsonResponseAction, HttpGet("GetDetails/{id}")]
        public async Task<JsonResult> GetDetails(int id)
        {
            try
            {
                var res = await _pOService.GetDetailsResponse(TOKEN, id);
                return Json(res);
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while getting PO."));
            }
        }
    }
}
