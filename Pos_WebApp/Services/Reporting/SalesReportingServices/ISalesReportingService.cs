using Models;
using Models.DTO.Reporting.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.Reporting.SalesReportingServices
{
    public interface ISalesReportingService
    {
        Task<Response> GetItemSalesResponse(string token, RptSalesSalesReportDto reportFormat);
        Task<Response> GetItemSales_ByItemsResponse(string token, RptSalesSalesReportDto reportFormat);
        Task<RptSalesSalesReportDto> GetItemSales(string token, RptSalesSalesReportDto reportFormat);
        Task<RptSalesSalesReportDto> GetItemSales_ByItems(string token, RptSalesSalesReportDto reportFormat);
        Task<Response> GetSales_ByDeliveryServicesResponse(string token, RptSalesSalesReportDto reportFormat);
        Task<RptSalesSalesReportDto> GetSales_ByDeliveryServices(string token, RptSalesSalesReportDto reportFormat);
        Task<double> GetSalesAmount(string token, RptSalesSalesReportDto filters);
        Task<Response> GetSalesAmountResponse(string token, RptSalesSalesReportDto filters);
        
    }
}
