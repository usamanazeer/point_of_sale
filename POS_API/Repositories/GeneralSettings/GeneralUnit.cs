//using Models.DTO.GeneralSettings;
//using POS_API.Repositories.GeneralSettings.TaxRepos;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Models.DTO.Reporting.Sales;

//namespace POS_API.Repositories.GeneralSettings
//{
//    public class GeneralUnit : IGeneralUnit
//    {
//        private readonly ITaxRepository _taxRepository;

//        public GeneralUnit(ITaxRepository taxRepository)
//        {
//            _taxRepository = taxRepository;
//        }


//        #region Tax

//        public async Task<TaxDto> CreateTax(TaxDto model)
//        {
//            return await  _taxRepository.Create(model);
//        }

//        public async Task<bool> DeleteTax(TaxDto model)
//        {
//            return await  _taxRepository.Delete(model);
//        }

//        public async Task<TaxDto> EditTax(TaxDto model)
//        {
//            return await  _taxRepository.Edit(model);
//        }

//        public async Task<List<TaxDto>> GetAllTaxes(TaxDto model)
//        {
//            return await  _taxRepository.GetAll(model);
//        }

//        public async Task<TaxDto> GetTaxDetails(TaxDto model)
//        {
//            return await  _taxRepository.GetDetails(model);
//        }


//        public async Task<TaxDto> GetEnabledForPos(int companyId)
//        {
//            return await _taxRepository.GetEnabledForPos(companyId);
//        }

//        public async Task<bool> IsTaxExist(TaxDto model)
//        {
//            return await  _taxRepository.IsExist(model);
//        }
//        #endregion

//        #region Tax Reporting
//        public async Task<RptTaxCollectionDto> GetTaxCollectionReport(RptTaxCollectionDto rptTaxCollectionDto)
//        {
//            return await _taxRepository.TaxCollectionReport(rptTaxCollectionDto);
//        }

//        #endregion
       
//    }
//}
