using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Pos_WebApp.Services.InventoryManagement.ModifierServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Areas.InventoryManagement.ViewComponents
{
    // ReSharper disable once InconsistentNaming
    public class AddItem_ItemModifiersRow:ViewComponent
    {
        private readonly IModifierService _modifierService;
        public AddItem_ItemModifiersRow(IModifierService modifierService)=> _modifierService = modifierService;
        
        public async Task<IViewComponentResult> InvokeAsync(Tuple<int, InvItemModifierDto> data)
        {
            //Tuple<rowNo, rowData, itemId>
            var token = HttpContext.Session.GetString("token");
            if (token != null)
            {
                var modifiersList = await _modifierService.GetSelectList(token);
                ViewBag.Modifiers = modifiersList?? new List<InvModifier_SLM>();
            }
            return View(data);
        }
    }
}
