using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using Pos_WebApp.Services.InventoryManagement.ItemBarCodeServices;
using Pos_WebApp.Services.InventoryManagement.ItemServices;

namespace Pos_WebApp.Areas.InventoryManagement.ViewComponents
{
    public class PurchaseDetailRow: ViewComponent
    {
        private readonly IItemService _itemService;
        private readonly IItemBarCodeService _itemBarCodeService;
        public PurchaseDetailRow(IItemService itemService, IItemBarCodeService itemBarCodeService)
        {
            _itemService = itemService;
            _itemBarCodeService = itemBarCodeService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Tuple<int, InvPurchaseDetailDto> data)
        {
            var token = HttpContext.Session.GetString("token");

            if (token != null)
            {
                var itemFilters = new InvItemDto() { ExceptDealItems = true, ExceptRecipeItems = true, Status = StatusTypes.Active.ToInt() };
                ViewBag.Items = (await _itemService.GetSelectList(token, itemFilters));
                ViewBag.ItemBarCodes = await _itemBarCodeService.GetSelectList(token, new InvItemBarCodeDto() { Item = itemFilters, Status = StatusTypes.Active.ToInt() });
            }
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(data);
        }
    }
}
