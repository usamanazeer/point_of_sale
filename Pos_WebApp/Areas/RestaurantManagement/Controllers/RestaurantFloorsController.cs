using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.RestaurantManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.RestaurantManagement.RestaurantFloorsServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.RestaurantManagement.Controllers
{
    [Area("RestaurantManagement"), RightAuthorization, Route("[controller]")]
    public class RestaurantFloorsController : BaseController
    {
        private readonly IRestaurantFloorsService _restaurantFloorsService;
        public RestaurantFloorsController(IRestaurantFloorsService restaurantFloorsService) : base("/RestaurantFloors") 
            => _restaurantFloorsService = restaurantFloorsService;
        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = new RestRestaurantFloorsDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false };
                model = await _restaurantFloorsService.Get(TOKEN, model: model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Floors."), IndexUrl);
            }
        }

        [HttpGet(nameof(Create))]
        public IActionResult Create()=> View(new RestRestaurantFloorsDto());

        [HttpPost, Route(nameof(Create), Name = "CreateRestaurantFloor")]
        public async Task<IActionResult> Create(RestRestaurantFloorsDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _restaurantFloorsService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception )
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating Floor."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _restaurantFloorsService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Floor."), backUrl: IndexUrl);
            }
        }


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _restaurantFloorsService.Details(token: TOKEN, id: id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Floor data."), backUrl: IndexUrl);
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(RestRestaurantFloorsDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _restaurantFloorsService.Edit(TOKEN, model));
                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating Floor."));
            }
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _restaurantFloorsService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting Floor."));
            }
        }
    }
}