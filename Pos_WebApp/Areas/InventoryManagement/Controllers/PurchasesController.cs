using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.PurchaseServices;
using Pos_WebApp.Services.InventoryManagement.VendorServices;
using StatusCodeEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area(areaName: "InventoryManagement"), Route(template: "[controller]")]
    public class PurchasesController : BaseController
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IVendorService _vendorService;
        public PurchasesController(IPurchaseService purchaseService, IVendorService vendorService):base("/Purchases")
        {
            _purchaseService = purchaseService;
            _vendorService = vendorService;
        }

        [RightAuthorization]
        public async Task<IActionResult> Index(int? id, int? status)
        {
            
            try
            {
                var model = new InvPurchaseMasterDto { Id = id, Status = status };
                model = await _purchaseService.Get(token: TOKEN, model: model);
                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch (Exception )
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading purchases."), IndexUrl);
            }
        }

        [RightAuthorization, HttpGet(template: nameof(Create))]
        public async Task<ActionResult> Create()
        {
            ViewBag.Vendors = await _vendorService.GetSelectList(TOKEN);
            return View(new InvPurchaseMasterDto());
        }

        [JsonResponseAction, RightAuthorization, HttpPost, Route(template: nameof(Create), Name = "CreatePurchases")]
        public async Task<IActionResult> Create(InvPurchaseMasterDto purchaseMasterDto)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _purchaseService.Create(TOKEN, purchaseMasterDto)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodeEnums.Invalid_State));
            }
            catch
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating purchases."));
            }
        }

        [HttpGet(template: "Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _purchaseService.Details(token: TOKEN, id: id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading purchases data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, RightAuthorization(RightName = "CreatePurchases"), HttpGet(template: "GetPurchaseDetailRow")]
        public IActionResult GetPurchaseDetailRow(int rowNo) => ViewComponent(componentName: "PurchaseDetailRow", arguments: new Tuple<int, InvPurchaseDetailDto>(item1: rowNo, item2: null));
    }
}