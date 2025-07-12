using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Services.InventoryManagement.VendorServices;
using StatusCodesEnums = Models.Enums.StatusCodes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Attributes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), RightAuthorization, Route("[controller]")]
    public class VendorsController : BaseController
    {
        private readonly IVendorService _vendorService;
        public VendorsController(IVendorService vendorService):base("/Vendors") => _vendorService = vendorService;

        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            try
            {
                var model = await _vendorService.Get(token: TOKEN, id, status, getDeleted);
                //filters
                model.Id = id;
                model.Status = status;
                model.DisplayDeleted = getDeleted??false;

                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading vendors data."), IndexUrl);
            }
            
        }

        [HttpGet(template: "Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                //filters
                var model = await _vendorService.Details(token: TOKEN, id: id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Delivery Service Vendor data."), backUrl: IndexUrl);
            }
        }

        [HttpGet(nameof(Create))]
        public IActionResult Create() => View(new InvVendorDto());

        
        [JsonResponseAction, HttpPost, Route(nameof(Create), Name = "CreateVendor")]
        public async Task<IActionResult> Create(InvVendorDto model)
        {
            try
            {
                return ModelState.IsValid ? 
                    Json( await _vendorService.Create(TOKEN, model)) : 
                    Json( global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating vendor."));
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _vendorService.Details(TOKEN, id);

                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading vendor data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(InvVendorDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _vendorService.Edit(TOKEN, model));

                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating vendor data."));
            }
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _vendorService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting vendor."));
            }
        }
    }
}
