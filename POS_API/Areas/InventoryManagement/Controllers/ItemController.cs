using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Services.InventoryManagement.ItemServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;

        public ItemController(ILogger<ItemController> logger, IAuthenticationUtilities authenticationService, IItemService itemService)
            : base(logger: logger, authenticationService: authenticationService) => _itemService = itemService;

        [HttpPost(template: nameof(Get))]
        public async Task<ActionResult> Get(InvItemDto model/*, bool fromCache = false*/)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                response = await _itemService.GetAll(model: model/*, fromCache: fromCache*/);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting items.");
                return BadRequest(error: response);
            }
        }

        [HttpGet(template: nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            var response = new Response();
            try
            {
                var model = new InvItemDto { Id = id, CompanyId = COMPANY_ID };
                response = await _itemService.GetDetails(model: model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting items.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(Create))]
        public async Task<ActionResult> Create(InvItemDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _itemService.Create(model: model);
                //model = (InvItemDto)response.Model;
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Item."));
            }
        }

        [HttpPost(template: nameof(SaveImage))]
        public async Task<ActionResult> SaveImage([FromForm] BaseModel model, [FromForm] IFormFile file = null)
        {
            try
            {
                var model1 = new InvItemDto { Id = model.Id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                var response = await _itemService.SaveImage(model: model1, file: file);
                return StatusCode(statusCode: response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Item."));
            }
        }

        [HttpPost(template: nameof(Edit))]
        public async Task<ActionResult> Edit(InvItemDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                response = await _itemService.Edit(model: model);
                return Ok(value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Updating Item.");
                return StatusCode(statusCode: StatusCodesEnums.Error_Occured.ToInt(),value: response);
            }
        }

        [HttpGet(template: "Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new InvItemDto {Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _itemService.Delete(model));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting item.", model: false));
            }
        }

        [HttpPost(template: nameof(GetSelectList))]
        public async Task<IActionResult> GetSelectList(InvItemDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _itemService.GetSelectList(model: model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt() , Models.Response.Error("Api Error while Getting items.", model: model));
            }
        }

        [HttpGet(nameof(GetItemsBulkUploadSamplePath))]
        public IActionResult GetItemsBulkUploadSamplePath()
        {
            try
            {
                return Ok(Models.Response.Message(null, model: _itemService.GetBulkImportSampleFilePath()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("An error Occurred while getting file path."));
            }
        }

        [HttpPost(nameof(BulkUpload))]
        public async Task<IActionResult> BulkUpload(IFormFile file)
        {
            try
            {
                return file is null ? 
                    StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("File can not be null.")) 
                    : Ok(await _itemService.BulkUpload(new InvItemDto { CompanyId = COMPANY_ID, CreatedBy = USER_ID }, file));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("An error Occurred while uploading data."));
            }
        }
    }
}