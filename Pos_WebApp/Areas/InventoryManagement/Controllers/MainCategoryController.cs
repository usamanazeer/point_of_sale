using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.CategoryServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), RightAuthorization, Route("[controller]")]
    public class MainCategoryController : BaseController
    {
        private readonly IMainCategoryService _mainCategoryService;
        public MainCategoryController(IMainCategoryService mainCategoryService):base("/MainCategory") => _mainCategoryService = mainCategoryService;

        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                //filters
                var model = new InvCategoryDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false };
                model = await _mainCategoryService.Get(TOKEN, model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading categories."), IndexUrl);
            }
        }

        [HttpGet(nameof(Create))]
        public IActionResult Create()=> View(new InvCategoryDto());

        [JsonResponseAction, HttpPost, Route(nameof(Create), Name = "CreateMainCategory")]
        public async Task<IActionResult> Create(InvCategoryDto model, IFormFile categoryImage)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _mainCategoryService.Create(TOKEN, model, categoryImage)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating main-category."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var model = new InvCategoryDto();
            try
            {
                //filters
                model.Id = id;
                var responseModel = await _mainCategoryService.Get(TOKEN, model);
                if (responseModel.MainCategories != null)
                {
                    model = responseModel.MainCategories.FirstOrDefault();
                    if (model != null) model.Response = responseModel.Response;
                    if (model != null && model.Response.ErrorOccured)
                        return Error(model.Response, IndexUrl);
                }
                if (model != null && model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                    return NotFound(model.Response, IndexUrl);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading category data."), backUrl: IndexUrl);
            }
            return View(model: model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new InvCategoryDto();
            try
            {
                model.Id = id;
                var responseModel = (await _mainCategoryService.Get(TOKEN, model));
                if (responseModel.MainCategories != null)
                {
                    model = responseModel.MainCategories.FirstOrDefault();
                    if (model != null) model.Response = responseModel.Response;
                }
                else
                {
                    return model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt() ? NotFound(model.Response, IndexUrl) : Error(model.Response, IndexUrl);
                }
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading category data."), backUrl: IndexUrl);
            }
            return View(model);
        }

        [JsonResponseAction, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(InvCategoryDto model, IFormFile categoryImage)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _mainCategoryService.Edit(TOKEN, model, categoryImage));

                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating category data."));
            }
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _mainCategoryService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting category."));
            }
        }
    }
}
