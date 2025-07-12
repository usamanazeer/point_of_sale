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
    public class SubCategoryController : BaseController
    {
        private readonly IMainCategoryService _mainCategoryService;
        private readonly ISubCategoryService _subCategoryService;
        public SubCategoryController(ISubCategoryService subCategoryService, IMainCategoryService mainCategoryService):base("/SubCategory")
        {
            _subCategoryService = subCategoryService;
            _mainCategoryService = mainCategoryService;
        }


        //[Obsolete, JsonResponseAction, HttpGet("Get")]
        //public async Task<JsonResult> Get(int? id, int? parentCategoryId, int? status, bool? getDeleted = false) {
        //    var model = new InvSubCategoryDto();
        //    try
        //    {
        //        if (SessionExists)
        //        {
        //            //filters
        //            model.Id = id;
        //            model.CategoryId = parentCategoryId;
        //            model.Status = status;
        //            model.DisplayDeleted = getDeleted??false;
        //            model = await _subCategoryService.Get(TOKEN, model);
        //        }
        //        model.Response.ErrorMessage = "Session Expired";
        //        return Json(model);
        //    }
        //    catch (Exception)
        //    {
        //        model.Response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
        //        model.Response.ErrorMessage = "An Error Occurred, while loading sub-categories.";
        //        return Json(model);
        //    }
        //}

        public async Task<IActionResult> Index(int? id, int? parentCategoryId, int? status, bool? getDeleted = false)
        {
            try
            {
                //filters
                var model = new InvSubCategoryDto { Id = id, CategoryId = parentCategoryId, Status = status, DisplayDeleted = getDeleted ?? false };
                model = await _subCategoryService.Get(TOKEN, model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading sub-categories."), IndexUrl);
            }
        }

        [HttpGet(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new InvSubCategoryDto { Category = await _mainCategoryService.Get(TOKEN) };
                return View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred."), IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost, Route(nameof(Create), Name = "CreateSubCategory")]
        public async Task<IActionResult> Create(InvSubCategoryDto model, IFormFile categoryImage)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _subCategoryService.Create(TOKEN, model, categoryImage)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating sub-category."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var model = new InvSubCategoryDto();
            try
            {
                //filters
                model.Id = id;
                var responseModel = await _subCategoryService.Get(TOKEN, model);
                if (responseModel.SubCategories != null)
                {
                    model = responseModel.SubCategories.FirstOrDefault();
                    if (model != null) model.Response = responseModel.Response;
                    if (model != null && model.Response.ErrorOccured)
                        return Error(model.Response, IndexUrl);
                }
                if (model != null && model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                    return NotFound(model.Response, IndexUrl);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading sub-category data."), backUrl: IndexUrl);
            }
            return View(model);
        }
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new InvSubCategoryDto();
            try
            {
                model.Id = id;
                var responseModel = (await _subCategoryService.Get(TOKEN, model));
                if (responseModel.SubCategories != null)
                {
                    model = responseModel.SubCategories.FirstOrDefault();
                    if (model != null)
                    {
                        model.Response = responseModel.Response;
                        model.Category = await _mainCategoryService.Get(TOKEN);
                    }
                }
                else
                {
                    return model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt() ? NotFound(model.Response, IndexUrl) : Error(model.Response, IndexUrl);
                }
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading sub-category data."), backUrl: IndexUrl);
            }
            ModelState.Clear();
            return View(model);
        }

        [JsonResponseAction, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(InvSubCategoryDto model, IFormFile categoryImage)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _subCategoryService.Edit(TOKEN, model, categoryImage));

                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating sub-category data."));
            }
        }


        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _subCategoryService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting sub category."));
            }
        }


        [JsonResponseAction, HttpGet("GetSelectList")]
        public async Task<JsonResult> GetSelectList(int? categoryId)
        {
            try
            {
                var filter = new InvSubCategoryDto { CategoryId = categoryId };
                var response = await _subCategoryService.GetSelectListResponse(TOKEN, filter);
                return Json(response);
            }
            catch
            {
                return Json(global::Models.Response.Error("An Error Occurred, while getting Subcategories."));
            }
        }
    }
}
