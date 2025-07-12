using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.InventoryManagement.ItemBarCodeServices;
using Pos_WebApp.Services.InventoryManagement.ItemServices;
using StatusCodesEnums = Models.Enums.StatusCodes;
using StatusTypes = Models.Enums.StatusTypes;

namespace Pos_WebApp.Areas.InventoryManagement.Controllers
{
    [Area("InventoryManagement"), RightAuthorization, Route("[controller]")]
    public class ItemBarCodesController : BaseController
    {
        private readonly IItemService _itemService;
        private readonly IItemBarCodeService _itemBarCodeService;
        public ItemBarCodesController(IItemBarCodeService itemBarCodeService, IItemService itemService):base("/ItemBarCodes")
        {
            _itemBarCodeService = itemBarCodeService;
            _itemService = itemService;
        }

        public async Task<IActionResult> Index(int? id, int? status, bool? displayDeleted = false)
        {
            try
            {
                var model = new InvItemDto { Id = id, DisplayDeleted = displayDeleted ?? false };
                if (id.HasValue)
                {
                    model.InvItemBarCode = (await _itemBarCodeService.Get(TOKEN, new InvItemBarCodeDto
                                                                              { ItemId = model.Id, DisplayDeleted = displayDeleted ?? false, Status = status })).ItemBarCodes ?? new List<InvItemBarCodeDto>();
                }
                ViewBag.Items = (await _itemService.GetSelectList(TOKEN, new InvItemDto { Status = StatusTypes.Active.ToInt() })) ?? new List<InvItem_SLM>();

                return model.Response.ErrorOccured ? Error(model.Response, IndexUrl) : View(model);
            }
            catch
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading Item BarCodes data."), IndexUrl);
            }
        }

        [HttpGet("Create/{itemId}")]
        public async Task<IActionResult> Create(int itemId)
        {
            try
            {
                var item = await _itemService.Details(TOKEN, itemId);
                var model = new InvItemBarCodeDto { ItemId = item.Id, Item = item };
                return View(model);
            }
            catch
            {
                return Error(global::Models.Response.Error("An Error Occurred."));
            }
        }

        [JsonResponseAction, HttpPost, Route(nameof(Create), Name = "CreateItemBarCode")]
        public async Task<IActionResult> Create(InvItemBarCodeDto model)
        {
            try
            {
                return ModelState.IsValid ?
                    Json(await _itemBarCodeService.Create(TOKEN, model)) :
                    Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while creating barcode."));
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await _itemBarCodeService.Details(TOKEN, id);
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading BarCode data."), backUrl: IndexUrl);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = (await _itemBarCodeService.Details(TOKEN, id));
                return GetView(model);
            }
            catch (Exception)
            {
                return Error(global::Models.Response.Error("An Error Occurred, while loading ItemBarCode data."), backUrl: IndexUrl);
            }
        }

        [JsonResponseAction, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(InvItemBarCodeDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    return Json(await _itemBarCodeService.Edit(TOKEN, model));

                return Json(global::Models.Response.Error("Please Fill the form care fully.", StatusCodesEnums.Invalid_State));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while updating BarCode data."));
            }
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                return Json(await _itemBarCodeService.Delete(TOKEN, id));
            }
            catch (Exception)
            {
                return Json(global::Models.Response.Error("An Error Occurred, while deleting BarCode."));
            }
        }
    }
}
