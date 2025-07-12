using Models.DTO.Reporting.Sales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.Reporting.SalesReportingRepositories
{
    public interface ISalesReportingRepository
    {
        Task<List<RptSalesSalesReportRowDto>> GetItemSales(RptSalesSalesReportDto filters);
        Task<List<RptSalesSalesReportRowDto>> GetItemSales_ByItems(RptSalesSalesReportDto filters);

        Task<double> GetSalesAmount(RptSalesSalesReportDto filters);
        //Task<double> GetTaxCollectedAmount(RptSalesSalesReportDto filters);
        Task<List<RptSalesSalesReportRowDto>> GetSales_ByDeliveryServices(RptSalesSalesReportDto filters);
    }
}
