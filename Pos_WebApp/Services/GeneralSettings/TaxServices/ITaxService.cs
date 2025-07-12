using Models;
using Models.DTO.GeneralSettings;
using System.Threading.Tasks;
using Models.DTO.Reporting.Sales;

namespace Pos_WebApp.Services.GeneralSettings.TaxServices
{
    public interface ITaxService
    {
        Task<TaxDto> Get(string token, TaxDto model = null);
        Task<Response> Create(string token, TaxDto model);
        Task<Response> Edit(string token, TaxDto model);
        Task<Response> Delete(string token, int id);
        Task<TaxDto> Details(string token, int id);
        Task<Response> GetEnabledForPos(string token);
        Task<Response> GetTaxCollectionReportResponse(string token , RptTaxCollectionDto rptTaxCollectionDto);
        Task<RptTaxCollectionDto> GetTaxCollectionReport(string token, RptTaxCollectionDto rptTaxCollectionDto);
    }
}
