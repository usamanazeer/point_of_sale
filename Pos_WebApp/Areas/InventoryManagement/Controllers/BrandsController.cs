using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.BrandServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area(areaName: "InventoryManagement"), RightAuthorization, Route(template: "[controller]")]
    public class BrandsController : BaseController
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService):base("/Brands") => _brandService = brandService;

        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = new InvBrandDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false };
                model = await _brandService.Get(token: TOKEN, model: model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading brands."), IndexUrl);
            }
        }

        [HttpGet(template: nameof(Create))]
        public IActionResult Create() => View(model: new InvBrandDto());


        [JsonResponseAction, HttpPost, Route(template: nameof(Create), Name = "CreateBrand")]
        public async Task<IActionResult> Create(InvBrandDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _brandService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating brand."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _brandService.Details(token: TOKEN, id: id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading brand data."), backUrl: IndexUrl);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _brandService.Details(token: TOKEN, id: id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading brand data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost(template: nameof(Edit))]
        public async Task<IActionResult> Edit(InvBrandDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _brandService.Edit(TOKEN, model));
                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating Brand."));
            }
        }

        [JsonResponseAction, HttpGet(template: "Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _brandService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting brand."));
            }
        }
    }
}