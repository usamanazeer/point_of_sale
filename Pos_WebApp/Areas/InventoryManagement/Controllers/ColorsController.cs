using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.ColorServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), RightAuthorization, Route("[controller]")]
    public class ColorsController : BaseController
    {
        private readonly IColorService _colorService;
        public ColorsController(IColorService colorService):base("/Colors") => _colorService = colorService;

        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = new InvColorDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false };
                model = await _colorService.Get(TOKEN, model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading colors."), IndexUrl);
            }
        }

        [HttpGet(nameof(Create))]
        public IActionResult Create() => View(new InvColorDto());


        [JsonResponseAction, HttpPost, Route(nameof(Create), Name = "CreateColor")]
        public async Task<IActionResult> Create(InvColorDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _colorService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating color."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _colorService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading color data."), backUrl: IndexUrl);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _colorService.Details(token: TOKEN, id: id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading color data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost, Route(nameof(Edit))]
        public async Task<IActionResult> Edit(InvColorDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _colorService.Edit(TOKEN, model));

                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating color data."));
            }
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _colorService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting color."));
            }
        }
    }
}
