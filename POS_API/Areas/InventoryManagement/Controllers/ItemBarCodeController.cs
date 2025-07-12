using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Services.InventoryManagement.ItemBarCodeServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class ItemBarCodeController : BaseController
    {
        private readonly IItemBarCodeService _itemBarCodeService;
        public ItemBarCodeController( ILogger<ItemBarCodeController> logger, IAuthenticationUtilities authenticationService ,IItemBarCodeService itemBarCodeService)
            : base(logger, authenticationService) => _itemBarCodeService = itemBarCodeService;

        [HttpPost(nameof(Get))]
        public async Task<ActionResult> Get(InvItemBarCodeDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _itemBarCodeService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting bar-codes."));
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new InvItemBarCodeDto { Id = id, CompanyId = COMPANY_ID };
                var response = await _itemBarCodeService.GetDetails(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting bar-codes."));
            }
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(InvItemBarCodeDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                response = await _itemBarCodeService.Create(model);
                //model = (InvItemBarCodeDto)response.Model;
                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Creating BarCode.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(InvItemBarCodeDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _itemBarCodeService.Edit(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating BarCode."));
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new InvItemBarCodeDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _itemBarCodeService.Delete(model));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting BarCode.", model: false));
            }
        }

        [HttpPost(nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(InvItemBarCodeDto model)
        {
            
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _itemBarCodeService.GetSelectList(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting item bar-codes.", model: model));
            }
        }
    }
}