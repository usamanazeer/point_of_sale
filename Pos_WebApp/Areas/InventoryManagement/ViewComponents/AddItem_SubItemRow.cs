using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Models.Enums;
using Pos_WebApp.Services.InventoryManagement.ItemBarCodeServices;
using Pos_WebApp.Services.InventoryManagement.ItemServices;
using System;
using System.Threading.Tasks;
using Models;

namespace Pos_WebApp.Areas.InventoryManagement.ViewComponents
{
    // ReSharper disable once InconsistentNaming
    public class AddItem_SubItemRow: ViewComponent
    {
        private readonly IItemService _itemService;
        private readonly IItemBarCodeService _itemBarCodeService;
        public AddItem_SubItemRow(IItemService itemService, IItemBarCodeService itemBarCodeService)
        {
            _itemService = itemService;
            _itemBarCodeService = itemBarCodeService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Tuple<int, InvItemRecipeDto, int> data)
        {
            var token = HttpContext.Session.GetString("token");

            if (token != null)
            {
                //data.Item2 is true for rawItemsOnly false for ready items only.
                var filter = new InvItemDto() { Status = StatusTypes.Active.ToInt() };
                
                if (data.Item3 == ItemTypes.RecipeItem.ToInt())
                    filter.ExceptDealItems = true;
                if (data.Item3 == ItemTypes.DealItem.ToInt())
                    filter.ExceptRawItems = true;

                var itemsList = await _itemService.GetSelectList(token,filter);
                ViewBag.Items = itemsList;
                //for barcodes
                var barCodeFilter = new InvItemBarCodeDto
                {
                    Status = StatusTypes.Active.ToInt(),
                    Item = filter
                };
                var itemBarCodes = await _itemBarCodeService.GetSelectList(token, barCodeFilter);
                ViewBag.ItemBarCodes = itemBarCodes;
            }
            return View(data);
        }
    }
}
