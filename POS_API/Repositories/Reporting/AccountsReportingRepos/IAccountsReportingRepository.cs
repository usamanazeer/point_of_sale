using System.Threading.Tasks;
using Models.DTO.Reporting.Accounts;

namespace POS_API.Repositories.Reporting.AccountsReportingRepos
{
    public interface IAccountsReportingRepository
    {
        Task<RptAccountsLedgerDto> GetLedger(RptAccountsLedgerDto accLedgerDto);
        Task<RptAccountsTrialBalanceDto> GetTrialBalance(RptAccountsTrialBalanceDto rptTrialBalanceDto);
        Task<RptAccountsIncomeStatementDto> GetIncomeStatement(RptAccountsIncomeStatementDto incomeStatementDto);
    }
}