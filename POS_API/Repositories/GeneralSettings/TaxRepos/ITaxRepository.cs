using Models.DTO.GeneralSettings;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.Reporting.Sales;

namespace POS_API.Repositories.GeneralSettings.TaxRepos
{
    public interface ITaxRepository
    {
        Task<TaxDto> Create(TaxDto model);
        Task<TaxDto> Edit(TaxDto model);
        Task<List<TaxDto>> GetAll(TaxDto model);
        Task<bool> IsExist(TaxDto model);
        Task<bool> Delete(TaxDto model);
        Task<TaxDto> GetDetails(TaxDto model);
        Task<TaxDto> GetEnabledForPos(int companyId);
        Task<RptTaxCollectionDto> TaxCollectionReport(RptTaxCollectionDto rptTaxCollectionDto);
    }
}
