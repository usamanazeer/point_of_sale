using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.GoodsReceivedNoteServices;
using Pos_WebApp.Services.InventoryManagement.PurchaseOrderServices;
using Pos_WebApp.Services.InventoryManagement.VendorServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), Route("[controller]")]
    public class GoodsReceivedNotesController : BaseController
    {
        private readonly IGrnService _grnService;
        private readonly IVendorService _vendorService;
        private readonly IPOService _pOService;
        public GoodsReceivedNotesController(IGrnService grnService, IVendorService vendorService, IPOService pOService):base("/GoodsReceivedNotes")
        {
            _grnService = grnService;
            _vendorService = vendorService;
            _pOService = pOService;
        }

        [RightAuthorization]
        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = new InvGrnMasterDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false };
                model = await _grnService.Get(TOKEN, model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading GRNs."), IndexUrl);
            }
        }

        [RightAuthorization, HttpGet(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            ViewBag.Vendors = await _vendorService.GetSelectList(TOKEN);
            return View(new InvGrnMasterDto());
        }

        [JsonResponseAction, RightAuthorization, HttpPost, Route(nameof(Create), Name = "CreateGRN")]
        public async Task<IActionResult> Create(InvGrnMasterDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _grnService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating GRN."));
            }
        }

        [RightAuthorization, HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = (await _grnService.Details(TOKEN, id));
                ViewBag.Vendors = await _vendorService.GetSelectList(TOKEN);
                ViewBag.PurchseOrders = await _pOService.GetSelectList(TOKEN, new InvPoMasterDto { VendorId = model.VendorId });
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading GRN data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(InvGrnMasterDto model)
        {
            
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                {
                    model.Status = StatusTypes.Active.ToInt();
                    return Json(await _grnService.Edit(TOKEN, model));
                }
                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading GRN data."), backUrl: IndexUrl);
            }
        }
        [RightAuthorization, HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _grnService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading GRN data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _grnService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting GRN."));
            }
        }

        [RightAuthorization(RightName = "GoodsReturnNotes"), JsonResponseAction, HttpPost(nameof(AddGrn_GrnDetailsRow))]
        public async Task<IActionResult> AddGrn_GrnDetailsRow(InvGrnDetailsDto model) => await Task.FromResult(ViewComponent("AddGrn_GrnDetailsRow", new Tuple<int, InvGrnDetailsDto>(model.Id ?? 0, model)));
    }
}
