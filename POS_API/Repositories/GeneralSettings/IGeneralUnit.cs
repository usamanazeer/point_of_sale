//using Models.DTO.GeneralSettings;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Models.DTO.Reporting.Sales;

//namespace POS_API.Repositories.GeneralSettings
//{
//    public interface IGeneralUnit
//    {
//        #region Tax
//        Task<List<TaxDto>> GetAllTaxes(TaxDto model);
//        Task<bool> IsTaxExist(TaxDto model);
//        Task<TaxDto> CreateTax(TaxDto model);
//        Task<TaxDto> EditTax(TaxDto model);
//        Task<bool> DeleteTax(TaxDto model);
//        Task<TaxDto> GetTaxDetails(TaxDto model);
//        Task<TaxDto> GetEnabledForPos(int companyId);

//        #endregion

//        #region Tax Reporting

//        Task<RptTaxCollectionDto> GetTaxCollectionReport(RptTaxCollectionDto rptTaxCollectionDto);

//        #endregion
//    }
//}
