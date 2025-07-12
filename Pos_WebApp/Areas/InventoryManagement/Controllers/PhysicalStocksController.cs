using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using Pos_WebApp.Areas.InventoryManagement.Models;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.ItemBarCodeServices;
using Pos_WebApp.Services.InventoryManagement.ItemServices;
using Pos_WebApp.Services.InventoryManagement.PhysicalInventoryServices;
using Pos_WebApp.Services.InventoryManagement.VendorServices;
using Pos_WebApp.Services.UserManagement.BranchServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area(areaName: "InventoryManagement"), Route(template: "[controller]")]
    public class PhysicalStocksController : BaseController
    {
        private readonly IItemBarCodeService _barCodeService;
        private readonly IBranchService _branchService;
        private readonly IItemService _itemService;
        private readonly IPhysicalInventoryService _physicalInventoryService;
        private readonly IVendorService _vendorService;

        public PhysicalStocksController(IPhysicalInventoryService physicalInventoryService, IItemService itemService, 
            IItemBarCodeService barCodeService, IVendorService vendorService, IBranchService branchService)
        {
            _physicalInventoryService = physicalInventoryService;
            _itemService = itemService;
            _barCodeService = barCodeService;
            _vendorService = vendorService;
            _branchService = branchService;
        }

        [RightAuthorization]
        public async Task<IActionResult> Index()
        {
            var model = new PhysicalStocks_ViewModel();
            try
            {
                model.OnlyIfRemaining = true;
                model.PhysicalInventories = (await _physicalInventoryService.Get(TOKEN)).InvPhysicalInventories;
                var itemFilters = new InvItemDto { ExceptDealItems = true, ExceptRecipeItems = true };
                ViewBag.Items = await _itemService.GetSelectList(TOKEN, itemFilters);
                ViewBag.ItemBarCodes = await _barCodeService.GetSelectList(TOKEN, new InvItemBarCodeDto { Item = itemFilters });
                model.Vendors = (await _vendorService.Get(TOKEN)).Vendors;
            }
            catch (Exception)
            {
                model.Response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                model.Response.ErrorMessage = "An Error Occurred, while loading data.";
                return Error(model.Response, "");
            }

            ModelState.Clear();
            return View(model: model);
        }

        [RightAuthorization, HttpPost]
        public async Task<IActionResult> Index(PhysicalStocks_ViewModel model)
        {
            try
            {
                if (model.Request != null)
                {
                    var dataViewFilter = new PhysicalInventoryViewFilter
                    {
                        BillIds = model.Id != null ? new[] {model.Id.Value} : Array.Empty<int>(),
                        ItemIds = model.ItemId != null? new[] {model.ItemId.Value}: Array.Empty<int>(),
                        ItemBarCodeIds = model.BarCodeId != null ? new[] { model.BarCodeId.Value } : Array.Empty<int>(),
                        VendorIds = model.VendorId != null ? new[] {model.VendorId.Value } : Array.Empty<int>(),
                        BillDateStart = model.BillDate,
                        ExpiryDate = model.ExpiryDate,
                        OnlyIfRemaining = model.OnlyIfRemaining
                    };
                    model.PhysicalInventoryView = await _physicalInventoryService.GetPhysicalInventoryView(TOKEN, dataViewFilter);
                }
                else
                {
                    model.OnlyIfRemaining = true;
                }
                model.PhysicalInventories = (await _physicalInventoryService.Get(token: TOKEN)).InvPhysicalInventories;
                var itemFilters = new InvItemDto { ExceptDealItems = true, ExceptRecipeItems = true };
                ViewBag.Items = await _itemService.GetSelectList(TOKEN, itemFilters);
                ViewBag.ItemBarCodes = await _barCodeService.GetSelectList(TOKEN, new InvItemBarCodeDto { Item = itemFilters });
                model.Vendors = (await _vendorService.Get(TOKEN)).Vendors;
            }
            catch (Exception)
            {
                // ignored
            }

            ModelState.Clear();
            return View(model);
        }

        [RightAuthorization, HttpGet(template: "Add")]
        public async Task<IActionResult> Add()
        {
            ViewBag.Branches = await _branchService.GetSelectList(TOKEN);
            return View(new InvPhysicalInventoryDto());
        }

        [RightAuthorization, HttpPost, Route(template: "Add", Name = "AddPhysicalStock")]
        public async Task<IActionResult> Add(InvPhysicalInventoryDto model)
        {
            try
            {
                model.Response = (await _physicalInventoryService.Add(TOKEN, model)).Response;
                ViewBag.Branches = await _branchService.GetSelectList(TOKEN);
                return View(model);
            }
            catch (Exception)
            {
                return Error(new Response(){ErrorMessage = "An Error Occurred."});
            }
        }

        [RightAuthorization, HttpGet(template: "BillDetails/{id}")]
        public async Task<IActionResult> BillDetails(int id)
        {
            var model = await _physicalInventoryService.GetBillDetails(TOKEN, id);
            return View(model);
        }

        [JsonResponseAction, RightAuthorization(RightName = "PhysicalStocks"), HttpGet(template: "GetStockTableRow")]
        public IActionResult GetStockTableRow(int rowNo) => ViewComponent(componentName: "AddStockTableRow", arguments: new Tuple<int, InvPhysicalInventoryItemDto>(item1: rowNo, item2: null));
    }
}