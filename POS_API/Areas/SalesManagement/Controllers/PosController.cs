using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using POS_API.Services.SalesManagement.PosServices;
using POS_API.Utilities.Authentication;

namespace POS_API.Areas.SalesManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class PosController : BaseController
    {
        private readonly IPosService _posService;
        public PosController( ILogger<PosController> logger, IAuthenticationUtilities authenticationService, IPosService posService ) 
            : base(logger, authenticationService) => _posService = posService;

        [HttpGet(nameof(ApplyCategoryFilter))]
        public async Task<ActionResult> ApplyCategoryFilter(int? categoryId)
        {
            var response = new Response();
            try
            {
                response = await _posService.ApplyCategoryFilter(COMPANY_ID, categoryId);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting data.");
                return BadRequest(response);
            }
        }

        [HttpGet(nameof(ApplySubCategoryFilter))]
        public async Task<ActionResult> ApplySubCategoryFilter(int? categoryId, int? subcategoryId)
        {
            var response = new Response();
            try
            {
                response = await _posService.ApplySubCategoryFilter(COMPANY_ID, categoryId, subcategoryId);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting data.");
                return BadRequest(response);
            }
        }

        [HttpGet(nameof(AllDealsFilter))]
        public async Task<ActionResult> AllDealsFilter()
        {
            var response = new Response();
            try
            {
                response = await _posService.AllDealsFilter(COMPANY_ID);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting data.");
                return BadRequest(response);
            }
        }

        [HttpGet(nameof(SubCategoryDealsFilter))]
        public async Task<ActionResult> SubCategoryDealsFilter(int? subcategoryId)
        {
            var response = new Response();
            try
            {
                response = await _posService.SubCategoryDealsFilter(COMPANY_ID, subcategoryId);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting data.");
                return BadRequest(response);
            }
        }

        [HttpGet(nameof(ApplySearchTextFilter))]
        public async Task<ActionResult> ApplySearchTextFilter(string searchText)
        {
            var response = new Response();
            try
            {
                response = await _posService.ApplySearchTextFilter(COMPANY_ID, searchText);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting data.");
                return BadRequest(response);
            }
        }
    }
}