using Models;
using System.Threading.Tasks;
using Models.DTO.Reporting.Accounts;

namespace POS_API.Services.Reporting.AccountsReportingServices
{
    public interface IAccountsReportingService
    {
        Task<Response> GetLedger(RptAccountsLedgerDto accLedgerPosting);
        Task<Response> GetTrialBalance(RptAccountsTrialBalanceDto rptTrialBalanceDto);
        Task<Response> GetIncomeStatement(RptAccountsIncomeStatementDto incomeStatementDto);
        Task<Response> GetBalanceSheet(RptAccountBalanceSheetDto rptAccountBalanceSheetDto);
    }
}
