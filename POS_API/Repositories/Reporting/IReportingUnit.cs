//using Models.DTO.Reporting.Sales;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Models.DTO.Reporting.Accounts;

//namespace POS_API.Repositories.Reporting
//{
//    public interface IReportingUnit
//    {
//        #region Sales Reporting
//        Task<List<RptSalesSalesReportRowDto>> GetItemSales(RptSalesSalesReportDto filters);
//        Task<List<RptSalesSalesReportRowDto>> GetItemSales_ByItems(RptSalesSalesReportDto filters);
//        Task<List<RptSalesSalesReportRowDto>> GetSales_ByDeliveryServices(RptSalesSalesReportDto filters);
//        Task<double> GetSalesAmount(RptSalesSalesReportDto filters);
//        //Task<double> GetTaxCollectedAmount(RptSalesSalesReportDto filters);
//        #endregion

//        #region Accounts Reporting
//        Task<RptAccountsLedgerDto> GetLedger(RptAccountsLedgerDto accLedgerDto);
//        Task<RptAccountsTrialBalanceDto> GetTrialBalance(RptAccountsTrialBalanceDto rptTrialBalanceDto);
//        Task<RptAccountsIncomeStatementDto> GetIncomeStatement(RptAccountsIncomeStatementDto incomeStatementDto);
//        #endregion

        
//    }
//}
