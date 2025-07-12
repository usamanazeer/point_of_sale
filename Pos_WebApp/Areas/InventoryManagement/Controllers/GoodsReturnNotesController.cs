using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.GoodsReturnNoteServices;
using Pos_WebApp.Services.InventoryManagement.VendorServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), Route("[controller]")]
    public class GoodsReturnNotesController : BaseController
    {
        private readonly IGrrnService _grrnService;
        private readonly IVendorService _vendorService;
        public GoodsReturnNotesController(IGrrnService grrnService, IVendorService vendorService) : base("/GoodsReturnNotes")
        {
            _grrnService = grrnService;
            _vendorService = vendorService;
        }

        [RightAuthorization]
        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            
            try
            {
                var model = new InvGrrnMasterDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false };
                model = await _grrnService.Get(TOKEN, model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception )
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading GRRNs."), IndexUrl);
            }
        }


        [RightAuthorization, HttpGet(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            ViewBag.Vendors = await _vendorService.GetSelectList(TOKEN);
            return View(new InvGrrnMasterDto());
        }

        [JsonResponseAction, RightAuthorization, HttpPost, Route(nameof(Create), Name = "CreateGRRN")]
        public async Task<IActionResult> Create(InvGrrnMasterDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _grrnService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating GRRN."));
            }
        }

        [RightAuthorization, HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _grrnService.Details(TOKEN, id);
                ViewBag.Vendors = await _vendorService.GetSelectList(TOKEN);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading GRRN data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(InvGrrnMasterDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _grrnService.Edit(TOKEN, model));
                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating GRRN data."));
            }
        }

        [RightAuthorization, HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _grrnService.Details(TOKEN, id);

                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading GRRN."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _grrnService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting GRRN."));
            }
        }

        [JsonResponseAction, RightAuthorization(RightName = "GoodsReturnNotes"), HttpGet(nameof(AddGrrn_GrrnDetailsRow))]
        public async Task<IActionResult> AddGrrn_GrrnDetailsRow(int rowNo) => await Task.FromResult(ViewComponent("AddGrrn_GrrnDetailsRow", new Tuple<int, InvGrrnDetailsDto>(rowNo, null)));
    }
}
