using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Services.GeneralSettings.TaxServices;
using Pos_WebApp.Services.InventoryManagement.ItemServices;
using Pos_WebApp.Services.InventoryManagement.VendorServices;
using Pos_WebApp.Services.InventoryManagement.ItemBarCodeServices;
using System;
using System.Threading.Tasks;
using Models;
using StatusCode = Models.Enums.StatusCodes;
using StatusTypes = Models.Enums.StatusTypes;

namespace Pos_WebApp.Areas.InventoryManagement.ViewComponents
{
    public class AddStockTableRow : ViewComponent
    {
        private readonly IItemService _itemService;
        private readonly IVendorService _vendorService;
        private readonly ITaxService _taxService;
        private readonly IItemBarCodeService _itemBarCodeService;
        public AddStockTableRow(IItemService itemService, IVendorService vendorService, ITaxService taxService, IItemBarCodeService itemBarCodeService)
        {
            _itemService = itemService;
            _vendorService = vendorService;
            _taxService = taxService;
            _itemBarCodeService = itemBarCodeService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Tuple<int, InvPhysicalInventoryItemDto> data)
        {
            var token = HttpContext.Session.GetString("token");
            if (token != null)
            {
                var itemFilters = new InvItemDto() { ExceptDealItems = true, ExceptRecipeItems = true, Status = StatusTypes.Active.ToInt() };
                ViewBag.Items = (await _itemService.GetSelectList(token, itemFilters));
                ViewBag.ItemBarCodes = await _itemBarCodeService.GetSelectList(token, new InvItemBarCodeDto() { Item = itemFilters, Status = StatusTypes.Active.ToInt() });
                var vendorModel = await _vendorService.Get(token);
                if (vendorModel.Response.ResponseCode == StatusCode.OK.ToInt())
                {
                    var vendors = vendorModel.Vendors;
                    ViewBag.Vendors = vendors;
                }
                var taxModel = await _taxService.Get(token);
                if (taxModel.Response.ResponseCode == StatusCode.OK.ToInt())
                {
                    var taxes = taxModel.Taxes;
                    ViewBag.Taxes = taxes;
                }
            }
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(data);
        }
    }
}
