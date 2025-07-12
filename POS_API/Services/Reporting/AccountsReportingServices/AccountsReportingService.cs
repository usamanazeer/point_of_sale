using Models;
using Models.Enums;
using System.Threading.Tasks;
using Models.DTO.Reporting.Accounts;
using POS_API.Repositories.Reporting.AccountsReportingRepos;

namespace POS_API.Services.Reporting.AccountsReportingServices
{
    public class AccountsReportingService:IAccountsReportingService, IService
    {
        private readonly IAccountsReportingRepository _accountsReportingRepository;

        public AccountsReportingService(IAccountsReportingRepository accountsReportingRepository) => _accountsReportingRepository = accountsReportingRepository;

        public async Task<Response> GetLedger(RptAccountsLedgerDto accLedgerPosting)
        {
            var response = new Response();
            var res = await _accountsReportingRepository.GetLedger(accLedgerPosting);
            response.Model = res;
            response.ResponseCode = StatusCodes.OK.ToInt();
            return response;
        }

        public async Task<Response> GetTrialBalance(RptAccountsTrialBalanceDto rptTrialBalanceDto)
        {
            var response = new Response();
            var res = await _accountsReportingRepository.GetTrialBalance(rptTrialBalanceDto);
            response.Model = res;
            response.ResponseCode = StatusCodes.OK.ToInt();
            return response;
        }

        public async Task<Response> GetIncomeStatement(RptAccountsIncomeStatementDto incomeStatementDto)
        {
            var response = new Response();
            var res = await _accountsReportingRepository.GetIncomeStatement(incomeStatementDto);
            response.Model = res;
            response.ResponseCode = StatusCodes.OK.ToInt();
            return response;
        }

        public async Task<Response> GetBalanceSheet(RptAccountBalanceSheetDto rptAccountBalanceSheetDto)
        {
            var response = new Response();
            var rptTrialBalanceDto = new RptAccountsTrialBalanceDto
             {
                 OnDate = rptAccountBalanceSheetDto.OnDate,
                 CompanyId = rptAccountBalanceSheetDto.CompanyId
             };
            var res = await _accountsReportingRepository.GetTrialBalance(rptTrialBalanceDto);
            rptAccountBalanceSheetDto.TrialBalanceData = res.TrialBalances;
            response.Model = rptAccountBalanceSheetDto;
            response.ResponseCode = StatusCodes.OK.ToInt();
            return response;
        }
    }
}