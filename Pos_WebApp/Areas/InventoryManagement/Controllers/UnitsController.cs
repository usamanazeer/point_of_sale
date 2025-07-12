using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.UnitServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), RightAuthorization, Route("[controller]")]
    public class UnitsController : BaseController
    {
        private readonly IUnitService _unitService;
        public UnitsController(IUnitService unitService):base("/Units") => _unitService = unitService;

        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = new InvUnitDto { Id = id, Status = status,  DisplayDeleted = getDeleted ?? false };
                model = await _unitService.Get(TOKEN, model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading units."), IndexUrl);
            }
        }

        [HttpGet(nameof(Create))]
        public IActionResult Create() => View(new InvUnitDto());

        [JsonResponseAction, HttpPost, Route(nameof(Create), Name = "CreateUnit")]
        public async Task<IActionResult> Create(InvUnitDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _unitService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating unit."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _unitService.Details(token: TOKEN, id: id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading unit data."), backUrl: IndexUrl);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _unitService.Details(token: TOKEN, id: id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading unit data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost, Route(nameof(Edit))]
        public async Task<IActionResult> Edit(InvUnitDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _unitService.Edit(TOKEN, model));

                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating unit data."));
            }
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _unitService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting unit."));
            }
        }
    }
}