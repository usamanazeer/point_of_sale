using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using POS_API.Services.InventoryManagement.PurchaseServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class PurchaseController : BaseController
    {
        private readonly IPurchaseService _purchaseService;
        public PurchaseController(ILogger<PurchaseController> logger,
                 IAuthenticationUtilities authenticationService
                 , IPurchaseService purchaseService) : base(logger, authenticationService) =>
            _purchaseService = purchaseService;


        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            try
            {
                var model = new InvPurchaseMasterDto{ Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var response = await _purchaseService.GetAll(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting purchases."));
            }
        }


        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(InvPurchaseMasterDto purchaseMasterDto)
        {
            try
            {
                purchaseMasterDto.CompanyId = COMPANY_ID;
                purchaseMasterDto.CreatedBy = USER_ID;
                purchaseMasterDto.CreatedOn = DateTime.Now;
                purchaseMasterDto.InvPurchaseDetail = purchaseMasterDto.InvPurchaseDetail.Where(x => x.Status != StatusTypes.Delete.ToInt()).ToList();
                var response = await _purchaseService.Create(purchaseMasterDto);
                //purchaseMasterDto = (InvPurchaseMasterDto)response.Model;
                return Ok(response);
            }
            catch(Exception )
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Creating Purchases.", model: purchaseMasterDto));
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = new InvPurchaseMasterDto { Id = id, CompanyId = COMPANY_ID };
                var response = await _purchaseService.GetDetails(model);
                return Ok(response);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), Models.Response.Error("Api Error while Getting Purchases data."));
            }
        }
    }
}