using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.RestaurantManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.RastaurantManagement.WaitersServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.RestaurantManagement.Controllers
{
    [Area("RestaurantManagement"), RightAuthorization, Route("[controller]")]
    public class WaitersController : BaseController
    {
        private readonly IWaitersService _waitersService;
        public WaitersController(IWaitersService waitersService):base("/Waiters") => _waitersService = waitersService;


        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = new RestWaiterDto { Id = id, Status = status,  DisplayDeleted = getDeleted ?? false };
                model = await _waitersService.Get(TOKEN, model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Waiters."), IndexUrl);
            }
        }

        [HttpGet(nameof(Create))]
        public IActionResult Create()=>View(new RestWaiterDto());

        [JsonResponseAction, HttpPost, Route(nameof(Create), Name = "CreateWaiter")]
        public async Task<IActionResult> Create(RestWaiterDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _waitersService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception )
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating Waiter."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _waitersService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Waiter data."), backUrl: IndexUrl);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = (await _waitersService.Details(TOKEN, id));
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Waiter data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(RestWaiterDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _waitersService.Edit(TOKEN, model));
                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating Waiter."));
            }
        }


        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _waitersService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting Waiter."));
            }
        }
    }
}
