using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Services.InventoryManagement.CategoryServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class CategoryController : BaseController
    {
        private readonly IMainCategoryService _mainCategoryService;
        private readonly ISubCategoryService _subCategoryService;
        public CategoryController(
            ILogger<CategoryController> logger, IAuthenticationUtilities authenticationService,
            IMainCategoryService mainCategoryService, ISubCategoryService subCategoryService)
            : base(logger, authenticationService)
        {
            _mainCategoryService = mainCategoryService;
            _subCategoryService = subCategoryService;
        }

        [HttpPost("Main/Get")]
        public async Task<ActionResult> MainGet(InvCategoryDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _mainCategoryService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Categories."));
            }
        }

        [HttpPost("Main/Create")]
        public async Task<ActionResult> MainCreate([FromForm] InvCategoryDto model, [FromForm] IFormFile file = null)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _mainCategoryService.Create(model, file);
                //model = (InvCategoryDto)response.Model;
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Category."));
            }
        }

        //[HttpPost("Main/Edit")]
        //public async Task<ActionResult> MainEdit([FromForm] InvCategoryDto model, [FromForm] IFormFile file = null)
        //{
        //    try
        //    {
        //        model.CompanyId = COMPANY_ID;
        //        model.ModifiedBy = USER_ID;
        //        model.ModifiedOn = DateTime.Now;
        //        var response = await _mainCategoryService.Edit(model, file);
        //        return Ok(response);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating Category."));
        //    }
        //}

        [HttpGet("Main/Delete/{id}")]
        public async Task<ActionResult> MainDelete(int id)
        {
            try
            {
                var model = new InvCategoryDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _mainCategoryService.Delete(model));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting category.", model: false));
            }
        }

        [HttpPost("Sub/Get")]
        public async Task<ActionResult> SubGet(InvSubCategoryDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _subCategoryService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(Models.Response.Error("Api Error while Getting Sub-Categories."));
            }
        }

        [HttpPost("Sub/Create")]
        public async Task<ActionResult> SubCreate([FromForm] InvSubCategoryDto model, [FromForm] IFormFile file = null)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var response = await _subCategoryService.Create(model, file);
                //model = (InvSubCategoryDto)response.Model;
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Sub-Category."));
            }
        }

        [HttpPost("Sub/Edit")]
        public async Task<ActionResult> SubEdit([FromForm] InvSubCategoryDto model, [FromForm] IFormFile file = null)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var response = await _subCategoryService.Edit(model, file);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Updating Sub-Category."));
            }
        }

        [HttpGet("Sub/Delete/{id}")]
        public async Task<ActionResult> SubDelete(int id)
        {
            try
            {
                var model = new InvSubCategoryDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                return Ok(await _subCategoryService.Delete(model));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while deleting sub-category.", model: false));
            }
        }

        [HttpPost("Sub/GetSelectList")]
        public async Task<IActionResult> SubGetSelectList(InvSubCategoryDto model)
        {
            try
            {
                model.CompanyId = COMPANY_ID;
                var response = await _subCategoryService.GetSelectList(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while subcategories.", model: model));
            }
        }
    }
}