using Models;
using Models.DTO.Reporting.Sales;
using Models.Enums;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.Reporting.SalesReportingServices
{
    public class SalesReportingService : ServiceBase, ISalesReportingService, IService
    {
        public SalesReportingService(IClientManager clientManager) : base("api/SalesReporting/", clientManager)
        { }

        public async Task<Response> GetItemSalesResponse(string token, RptSalesSalesReportDto reportFormat)
        {
            var res = await Client.Post<Response>($"{Route}GetItemSales", reportFormat, token: token);
            if (res.ResponseCode == StatusCodes.OK.ToInt() && res.Model != null)
                res.Model = JsonConvert.DeserializeObject<RptSalesSalesReportDto>(res.Model.String());
            return res;
        }
        public async Task<RptSalesSalesReportDto> GetItemSales(string token, RptSalesSalesReportDto reportFormat)
        {
            var res = await GetItemSalesResponse(token, reportFormat);
            return res.Model != null ? (RptSalesSalesReportDto) res.Model : new RptSalesSalesReportDto();
        }

        public async Task<Response> GetItemSales_ByItemsResponse(string token, RptSalesSalesReportDto reportFormat)
        {
            var res = await Client.Post<Response>($"{Route}GetItemSales_ByItems", reportFormat, token: token);
            if (res.ResponseCode == StatusCodes.OK.ToInt() && res.Model != null)
                res.Model = JsonConvert.DeserializeObject<RptSalesSalesReportDto>(res.Model.String());
            return res;
        }
        
        public async Task<RptSalesSalesReportDto> GetItemSales_ByItems(string token, RptSalesSalesReportDto reportFormat)
        {
            var res = await GetItemSales_ByItemsResponse(token, reportFormat);
            return res.Model != null ? (RptSalesSalesReportDto) res.Model : new RptSalesSalesReportDto();
        }

        public async Task<Response> GetSales_ByDeliveryServicesResponse(string token, RptSalesSalesReportDto reportFormat)
        {
            var res = await Client.Post<Response>($"{Route}GetSales_ByDeliveryServices", reportFormat, token: token);
            if (res.ResponseCode == StatusCodes.OK.ToInt() && res.Model != null)
                res.Model = JsonConvert.DeserializeObject<RptSalesSalesReportDto>(res.Model.String());
            return res;
        }
        
        public async Task<RptSalesSalesReportDto> GetSales_ByDeliveryServices(string token, RptSalesSalesReportDto reportFormat)
        {
            var res = await GetSales_ByDeliveryServicesResponse(token, reportFormat);
            return res.Model != null ? (RptSalesSalesReportDto) res.Model : new RptSalesSalesReportDto();
        }

        public async Task<double> GetSalesAmount(string token, RptSalesSalesReportDto filters)
        {
            var res = await GetSalesAmountResponse(token, filters);
            return res.ResponseCode == StatusCodes.OK.ToInt() && res.Model != null ? (double) res.Model : 0;
        }

        public async Task<Response> GetSalesAmountResponse(string token, RptSalesSalesReportDto filters)
            => await Client.Post<Response>($"{Route}GetSalesAmount", filters, token: token);
    }
}
