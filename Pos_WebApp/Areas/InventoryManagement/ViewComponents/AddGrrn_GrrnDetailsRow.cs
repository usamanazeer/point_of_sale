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
    public class AddGrrn_GrrnDetailsRow : ViewComponent
    {
        private readonly IItemService _itemService;
        public AddGrrn_GrrnDetailsRow(IItemService itemService)=> _itemService = itemService;

        public async Task<IViewComponentResult> InvokeAsync(Tuple<int, InvGrrnDetailsDto> data)
        {
            var token = HttpContext.Session.GetString("token");
            if (token != null)
            {
                //data.Item2 is true for rawItemsOnly false for ready items only.
                var filter = new InvItemDto
                {
                    Status = StatusTypes.Active.ToInt(), ExceptDealItems = true, ExceptRecipeItems = true
                };
                var itemsList = await _itemService.GetSelectList(token, filter);
                ViewBag.Items = itemsList;
            }
            return View("AddGrrn_GrrnDetailsRow", data);
        }
    }
}
