using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.RestaurantManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.RestaurantManagement.DiningTableServices;
using Pos_WebApp.Services.RestaurantManagement.RestaurantFloorsServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.RestaurantManagement.Controllers
{
    [Area("RestaurantManagement"), RightAuthorization, Route("[controller]")]
    public class DiningTablesController : BaseController
    {
        private readonly IDiningTableService _diningTableService;
        private readonly IRestaurantFloorsService _restaurantFloorsService;
        public DiningTablesController(IDiningTableService diningTableService, IRestaurantFloorsService restaurantFloorsService):base("/DiningTables")
        {
            _diningTableService = diningTableService;
            _restaurantFloorsService = restaurantFloorsService;
        }

        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = new RestDiningTableDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false };
                if (getDeleted.HasValue) model.DisplayDeleted = getDeleted.Value;
                model = await _diningTableService.Get(TOKEN, model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Dining Tables."), IndexUrl);
            }
        }

        [HttpGet(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            var model = new RestDiningTableDto();
            try
            {
                ViewBag.Floors = await _restaurantFloorsService.GetSelectList(TOKEN);
                return View(model);
            }
            catch (Exception )
            {
                return Error(global::Models.Response.Error("An Error Occurred."), IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost, Route(nameof(Create), Name = "CreateDiningTable")]
        public async Task<IActionResult> Create(RestDiningTableDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _diningTableService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception )
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating Dining Table."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _diningTableService.Details(token: TOKEN, id: id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Dining Table."), backUrl: IndexUrl);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _diningTableService.Details(token: TOKEN, id: id);
                ViewBag.Floors = await _restaurantFloorsService.GetSelectList(TOKEN);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Dining Table."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(RestDiningTableDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _diningTableService.Edit(TOKEN, model));
                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating Dining Table."));
            }
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _diningTableService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting Dining Table."));
            }
        }

        [JsonResponseAction, HttpPost(nameof(ReleaseOrOccupy))]
        public async Task<JsonResult> ReleaseOrOccupy(RestDiningTableDto model)
        {
            try
            {
                return Json(await _diningTableService.ReleaseOrOccupy(TOKEN, model));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while performing operation on Table."));
            }
        }
    }
}
