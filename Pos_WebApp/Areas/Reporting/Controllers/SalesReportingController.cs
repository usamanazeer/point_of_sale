using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Reporting.Sales;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.DeliveryService.DeliveryServiceVendorServices;
using Pos_WebApp.Services.InventoryManagement.ItemServices;
using Pos_WebApp.Services.RastaurantManagement.WaitersServices;
using Pos_WebApp.Services.Reporting.SalesReportingServices;

namespace Pos_WebApp.Areas.Reporting.Controllers
{
    [Area(areaName: "Reporting"), Route(template: "[controller]")]
    public class SalesReportingController : BaseController
    {
        private readonly ISalesReportingService _salesReportingService;
        private readonly IItemService _itemService;
        private readonly IWaitersService _waitersService;
        private readonly IDeliveryServiceVendorService _deliveryServiceVendorService;


        public SalesReportingController(
            ISalesReportingService salesReportingService, IItemService itemService, 
            IDeliveryServiceVendorService deliveryServiceVendorService, IWaitersService waitersService)
        {
            _salesReportingService = salesReportingService;
            _itemService = itemService;
            _deliveryServiceVendorService = deliveryServiceVendorService;
            _waitersService = waitersService;
        }


        [RightAuthorization, HttpGet(nameof(Basic))]
        public IActionResult Basic()
        {
            return View(model: new RptSalesSalesReportDto());
        }

        [RightAuthorization, HttpGet(nameof(SalesByItems))]
        public async Task<IActionResult> SalesByItems()
        {
            var getItemsListTask = _itemService.GetSelectList(token: TOKEN);
            var getWaitersListTask = _waitersService.GetSelectList(tOKEN: TOKEN);
            
            await Task.WhenAll(getItemsListTask, getWaitersListTask);
            
            ViewBag.ItemsList = getItemsListTask.Result;
            ViewBag.WaitersList = getWaitersListTask.Result;
            
            return View(model: new RptSalesSalesReportDto());
        }

        [RightAuthorization, HttpGet(nameof(SalesByDeliveryServices))]
        public async Task<IActionResult> SalesByDeliveryServices()
        {
            ViewBag.DeliveryServicsList = await _deliveryServiceVendorService.GetSelectList(TOKEN);
            return View(model: new RptSalesSalesReportDto());
        }

        [RightAuthorization(RightName = "SalesReports"), JsonResponseAction, HttpPost(template: nameof(BasicSalesReport))]
        public async Task<IActionResult> BasicSalesReport(RptSalesSalesReportDto reportFormat)
        {
            var res = await _salesReportingService.GetItemSalesResponse(token: TOKEN, reportFormat: reportFormat);
            return Json(data: res);
        }


        [RightAuthorization(RightName = "SalesReports"), JsonResponseAction, HttpPost(template: nameof(SalesReportByItems))]
        public async Task<IActionResult> SalesReportByItems(RptSalesSalesReportDto reportFormat)
        {
            var res = await _salesReportingService.GetItemSales_ByItemsResponse(token: TOKEN, reportFormat: reportFormat);
            return Json(data: res);
        }
        [RightAuthorization(RightName = "SalesReports"), JsonResponseAction, HttpPost(template: nameof(SalesByDeliveryServicesReport))]
        public async Task<IActionResult> SalesByDeliveryServicesReport(RptSalesSalesReportDto reportFormat)
        {
            var res = await _salesReportingService.GetSales_ByDeliveryServicesResponse(token: TOKEN,
                                                                        reportFormat: reportFormat);
            return Json(data: res);
        }

        [RightAuthorization(RightName = "SalesReports"), JsonResponseAction, HttpPost(nameof(GetSalesAmount))]
        public async Task<IActionResult> GetSalesAmount(RptSalesSalesReportDto filters)
        {
            var res = await _salesReportingService.GetSalesAmountResponse(token: TOKEN,filters: filters);
            return Json(data: res);
        }
    }
}