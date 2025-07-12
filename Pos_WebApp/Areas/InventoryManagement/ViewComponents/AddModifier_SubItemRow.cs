using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Pos_WebApp.Services.InventoryManagement.ItemServices;
using Pos_WebApp.Services.InventoryManagement.ItemBarCodeServices;
using System;
using System.Threading.Tasks;
using Models;
using StatusTypes = Models.Enums.StatusTypes;

namespace Pos_WebApp.Areas.InventoryManagement.ViewComponents
{
    // ReSharper disable once InconsistentNaming
    public class AddModifier_SubItemRow: ViewComponent
    {
        private readonly IItemService _itemService;
        private readonly IItemBarCodeService _barcodeService;
        public AddModifier_SubItemRow(IItemService itemService, IItemBarCodeService barcodeService)
        {
            _itemService = itemService;
            _barcodeService = barcodeService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Tuple<int, InvModifierItemDto> data)
        {
            var token = HttpContext.Session.GetString("token");
            if (token != null)
            {
                var itemFilter = new InvItemDto() { ExceptDealItems = true, Status = StatusTypes.Active.ToInt() };
                ViewBag.Items = await _itemService.GetSelectList(token, itemFilter);
                ViewBag.ItemBarCodes = await _barcodeService.GetSelectList(token, new InvItemBarCodeDto() { Item = itemFilter, Status = StatusTypes.Active.ToInt()});
            }
            return View(data);
        }
    }
}
