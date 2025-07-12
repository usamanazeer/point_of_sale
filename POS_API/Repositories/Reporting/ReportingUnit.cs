//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Models.DTO.Reporting.Accounts;
//using Models.DTO.Reporting.Sales;
////using POS_API.Repositories.GeneralSettings.TaxRepos;
//using POS_API.Repositories.Reporting.AccountsReportingRepos;
//using POS_API.Repositories.Reporting.SalesReportingRepositories;

//namespace POS_API.Repositories.Reporting
//{
//    public class ReportingUnit : IReportingUnit
//    {
//        private readonly ISalesReportingRepository _salesReportingRepository;
//        private readonly IAccountsReportingRepository _accountsReportingRepository;
//        //private readonly ITaxRepository _taxRepository;
//        public ReportingUnit(ISalesReportingRepository salesReportingRepository,
//                             IAccountsReportingRepository accountsReportingRepository
//                             /*,ITaxRepository taxRepository*/)
//        {
//            _salesReportingRepository = salesReportingRepository;
//            _accountsReportingRepository = accountsReportingRepository;
//            //_taxRepository = taxRepository;
//        }

//        #region Sales Reporting
//        public async Task<List<RptSalesSalesReportRowDto>> GetItemSales(RptSalesSalesReportDto filters)
//        {
//            return await _salesReportingRepository.GetItemSales(filters);
//        }
//        public async Task<List<RptSalesSalesReportRowDto>> GetItemSales_ByItems(RptSalesSalesReportDto filters)
//        {
//            return await _salesReportingRepository.GetItemSales_ByItems(filters);
//        }


//        public async Task<List<RptSalesSalesReportRowDto>> GetSales_ByDeliveryServices(RptSalesSalesReportDto filters)
//        {
//            return await _salesReportingRepository.GetSales_ByDeliveryServices(filters);
//        }


//        public async Task<double> GetSalesAmount(RptSalesSalesReportDto filters)
//        {
//            return await _salesReportingRepository.GetSalesAmount(filters);
//        }


//        //public async Task<double> GetTaxCollectedAmount(RptSalesSalesReportDto filters)
//        //{
//        //    return await _salesReportingRepository.GetTaxCollectedAmount(filters);
//        //}

//        #endregion



//        #region Accounts Reporting
//        public async Task<RptAccountsLedgerDto> GetLedger(RptAccountsLedgerDto accLedgerDto)
//        {
//            return await _accountsReportingRepository.GetLedger(accLedgerDto);
//        }


//        public async Task<RptAccountsTrialBalanceDto> GetTrialBalance(RptAccountsTrialBalanceDto rptTrialBalanceDto)
//        {
//            return await _accountsReportingRepository.GetTrialBalance(rptTrialBalanceDto);
//        }


//        public async Task<RptAccountsIncomeStatementDto> GetIncomeStatement(RptAccountsIncomeStatementDto incomeStatementDto)
//        {
//            return await _accountsReportingRepository.GetIncomeStatement(incomeStatementDto);
//        }

//        #endregion
//    }
//}
