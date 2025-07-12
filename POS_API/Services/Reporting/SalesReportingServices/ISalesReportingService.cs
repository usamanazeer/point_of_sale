using Models;
using Models.DTO.Reporting.Sales;
using System.Threading.Tasks;

namespace POS_API.Services.Reporting.SalesReportingServices
{
    public interface ISalesReportingService
    {
        Task<Response> GetItemSales(RptSalesSalesReportDto filters);
        Task<Response> GetItemSales_ByItems(RptSalesSalesReportDto filters);
        Task<Response> GetSalesAmount(RptSalesSalesReportDto filters);
        //Task<Response> GetTaxCollectedAmount(RptSalesSalesReportDto filters);
        Task<Response> GetSales_ByDeliveryServices(RptSalesSalesReportDto filters);
    }
}
