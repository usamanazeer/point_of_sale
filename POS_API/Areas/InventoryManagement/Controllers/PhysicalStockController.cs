using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using Models.Enums;
using POS_API.Services.InventoryManagement.PhysicalInventory.PhysicalInventoryServices;
using POS_API.Utilities.Authentication;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.InventoryManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class PhysicalStockController : BaseController
    {
        private readonly IPhysicalInventoryService _physicalInventoryService;
        public PhysicalStockController(
            ILogger<PhysicalStockController> logger, IAuthenticationUtilities authenticationService
            ,IPhysicalInventoryService physicalInventoryService
            )
            : base(logger, authenticationService) =>
            _physicalInventoryService = physicalInventoryService;


        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null)
        {
            var response = new Response();
            var model = new InvPhysicalInventoryDto();
            try
            {
                model.Id = id;
                model.Status = status;
                model.DisplayDeleted = getDeleted??false;
                model.CompanyId = COMPANY_ID;
                response = await _physicalInventoryService.GetAll(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Bills.");
                return BadRequest(response);
            }
        }
        [HttpGet(nameof(BillDetails))]
        public async Task<ActionResult> BillDetails(int id)
        {
            var response = new Response();
            var model = new PhysicalInventoryViewFilter { Id = id };
            try
            {
                
                model.CompanyId = COMPANY_ID;
                response = await _physicalInventoryService.GetBillDetails(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Bill.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Add))]
        public async Task<ActionResult> Add(InvPhysicalInventoryDto model)
        {
            var response = new Response();
            try
            {
                
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                model.InvPhysicalInventoryItem = model.InvPhysicalInventoryItem.Where(x => x.Status != StatusTypes.Delete.ToInt() && x.ItemId != null).ToList();
                response = await _physicalInventoryService.Add(model);
                model = (InvPhysicalInventoryDto)response.Model;

                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError(errorMessage: "Api Error while Adding Stock.", model: model);
                return BadRequest(response);
            }
        }
        [HttpPost(nameof(GetLowInventory))]
        public async Task<ActionResult> GetLowInventory(PhysicalInventoryViewFilter filters) 
        {
            var response = new Response();
            try
            {
                filters.CompanyId = COMPANY_ID;
                response = await _physicalInventoryService.GetLowInventory(filters);
                //model = (InvPhysicalInventoryDTO)response.Model;
                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Low Inventory.");
                //response.Model = model;
                return BadRequest(response);
            }
        }
        [HttpPost(nameof(GetPhysicalInventoryView))]
        public async Task<ActionResult> GetPhysicalInventoryView(PhysicalInventoryViewFilter filters)
        {
            var response = new Response();
            try
            {
                filters.CompanyId = COMPANY_ID;
                filters.CreatedBy = USER_ID;
                filters.CreatedOn = DateTime.Now;
                response = await _physicalInventoryService.GetPhysicalInventory_View(filters);
                //model = (InvPhysicalInventoryDTO)response.Model;
                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Bills.");
                //response.Model = model;
                return BadRequest(response);
            }
        }
    }
}
