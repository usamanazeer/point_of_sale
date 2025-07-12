using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.ModifierServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), Route("[controller]")]
    public class ModifiersController : BaseController
    {
        private readonly IModifierService _modifierService;
        public ModifiersController(IModifierService modifierService):base("/Modifiers") => _modifierService = modifierService;

        [RightAuthorization]
        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = new InvModifierDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false };
                model = await _modifierService.Get(TOKEN, model);
                return model.Response.ErrorOccured? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading modifiers."), IndexUrl);
            }
        }

        [RightAuthorization, HttpGet(nameof(Create))]
        public IActionResult Create() => View(new InvModifierDto());

        [JsonResponseAction, RightAuthorization, HttpPost, Route(nameof(Create), Name = "CreateModifier")]
        public async Task<IActionResult> Create(InvModifierDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _modifierService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating modifier."));
            }
        }

        [RightAuthorization, HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _modifierService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading modifier data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(InvModifierDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _modifierService.Edit(TOKEN, model));

                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating modifier data."));
            }
        }

        [RightAuthorization, HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _modifierService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading modifier data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _modifierService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting modifier."));
            }
        }

        [JsonResponseAction, RightAuthorization(RightName = "Modifiers"), HttpGet("GetAddModifier_ItemsRow")]
        public async Task<IActionResult> GetAddModifier_ItemsRow(int rowNo) => await Task.FromResult(ViewComponent("AddModifier_SubItemRow", new Tuple<int, InvModifierItemDto>(rowNo, null)));
    }
}
