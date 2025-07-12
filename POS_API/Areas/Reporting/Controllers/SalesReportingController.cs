using Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Reporting.Sales;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using POS_API.Utilities.Authentication;
using POS_API.Services.Reporting.SalesReportingServices;

namespace POS_API.Areas.Reporting.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class SalesReportingController : BaseController
    {
        private readonly ISalesReportingService _salesReportingService;
        public SalesReportingController(
            ILogger<SalesReportingController> logger, IAuthenticationUtilities authenticationService, ISalesReportingService salesReportingService
            ) : base(logger, authenticationService) => _salesReportingService = salesReportingService;


        [HttpPost(nameof(GetItemSales))]
        public async Task<ActionResult> GetItemSales(RptSalesSalesReportDto filters)
        {
            var response = new Response();
            try
            {
                filters.CompanyId = COMPANY_ID;
                filters.BranchId = BRANCH_ID;
                response = await _salesReportingService.GetItemSales(filters);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting Sales Data.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(GetItemSales_ByItems))]
        public async Task<ActionResult> GetItemSales_ByItems(RptSalesSalesReportDto filters)
        {
            var response = new Response();
            try
            {
                filters.CompanyId = COMPANY_ID;
                filters.BranchId = BRANCH_ID;
                response = await _salesReportingService.GetItemSales_ByItems(filters);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting Sales Data.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(GetSales_ByDeliveryServices))]
        public async Task<ActionResult> GetSales_ByDeliveryServices(RptSalesSalesReportDto filters)
        {
            var response = new Response();
            try
            {
                filters.CompanyId = COMPANY_ID;
                filters.BranchId = BRANCH_ID;
                response = await _salesReportingService.GetSales_ByDeliveryServices(filters);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Sales Data.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(GetSalesAmount))]
        public async Task<ActionResult> GetSalesAmount(RptSalesSalesReportDto filters)
        {
            var response = new Response();
            try
            {
                
                filters.CompanyId = COMPANY_ID;
                filters.BranchId = BRANCH_ID;
                response = await _salesReportingService.GetSalesAmount(filters);
                return !response.ErrorOccured ? Ok(response) : StatusCode(response.ErrorCode, response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting Sales Data.");
                return BadRequest(response);
            }
        }
    }
}
