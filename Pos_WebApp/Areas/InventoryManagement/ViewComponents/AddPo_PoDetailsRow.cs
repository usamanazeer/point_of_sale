using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Models.Enums;
using Pos_WebApp.Services.InventoryManagement.ItemServices;
using System;
using System.Threading.Tasks;
using Models;

namespace Pos_WebApp.Areas.InventoryManagement.ViewComponents
{
    // ReSharper disable once InconsistentNaming
    public class AddPo_PoDetailsRow:ViewComponent
    {
        private readonly IItemService _itemService;
        public AddPo_PoDetailsRow(IItemService itemService) => _itemService = itemService;

        public async Task<IViewComponentResult> InvokeAsync(Tuple<int, InvPoDetailsDto> data)
        {
            var token = HttpContext.Session.GetString("token");
            if (token != null)
            {
                //data.Item2 is true for rawItemsOnly false for ready items only.
                InvItemDto filter = new InvItemDto
                {
                    Status = StatusTypes.Active.ToInt(),
                    ExceptDealItems = true,
                    ExceptRecipeItems = true
                };
                var itemsList = await _itemService.GetSelectList(token, filter);
                ViewBag.Items = itemsList;
            }
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View("AddPo_PoDetailsRow",data);
        }
    }
}
