using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.Accounts;
using POS_API.Services.AccountsManagement.BillsServices;
using POS_API.Utilities.Authentication;

namespace POS_API.Areas.AccountsManagement.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class BillController : BaseController
    {
        private readonly IBillsService _billsService;
        public BillController(ILogger<BaseController> logger,
                 IAuthenticationUtilities authenticationService, IBillsService billsService) 
            : base(logger, authenticationService) => _billsService = billsService;

        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status, bool? getDeleted = null, bool? excludePaidBills = null)
        {
            var response = new Response();
            var model = new BillDto();
            try
            {
                model.Id = id;
                model.Status = status;
                model.DisplayDeleted = getDeleted.HasValue && getDeleted.Value;
                model.ExcludePaidBills = excludePaidBills.HasValue && excludePaidBills.Value;
                model.CompanyId = COMPANY_ID;

                response = await _billsService.GetAll(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting bills.");
                return BadRequest(response);
            }
        }

        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            var response = new Response();
            var model = new BillDto();
            try
            {
                model.Id = id;
                model.CompanyId = COMPANY_ID;

                response = await _billsService.GetDetails(model);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Bill Details.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(PayBill))]
        public async Task<ActionResult> PayBill(AccBillPaymentDto billPaymentDto)
        {
            var response = new Response();
            try
            {
                billPaymentDto.CompanyId = COMPANY_ID;
                billPaymentDto.CreatedBy = USER_ID;
                billPaymentDto.CreatedOn = DateTime.Now;
                response = await _billsService.PayBill(billPaymentDto);
                //model = (InvBrandDto)response.Model;
                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Paying Bill.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(GetBillsByFilters))]
        public async Task<ActionResult> GetBillsByFilters(BillDto billPaymentDto)
        {
            var response = new Response();
            try
            {
                billPaymentDto.CompanyId = COMPANY_ID;
                billPaymentDto.CreatedBy = USER_ID;
                billPaymentDto.CreatedOn = DateTime.Now;
                response = await _billsService.GetAll(billPaymentDto);
                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while getting Bills.");
                return BadRequest(response);
            }
        }
    }
}
