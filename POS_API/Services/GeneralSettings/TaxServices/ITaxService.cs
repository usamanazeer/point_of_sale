using Models;
using Models.DTO.GeneralSettings;
using System.Threading.Tasks;
using Models.DTO.Reporting.Sales;

namespace POS_API.Services.GeneralSettings.TaxServices
{
    public interface ITaxService
    {
        Task<Response> GetAll(TaxDto model);
        Task<Response> Create(TaxDto model);
        Task<Response> Edit(TaxDto model);
        Task<bool> Delete(TaxDto model);
        Task<bool> IsExist(TaxDto model);
        Task<Response> GetDetails(TaxDto model);
        Task<Response> GetEnabledForPos(int companyId);
        Task<Response> GetTaxCollectionReport(RptTaxCollectionDto rptTaxCollectionDto);
    }
}
