using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO.Reporting.Accounts;

namespace Pos_WebApp.Services.Reporting.AccountsReportingServices
{
    public interface IAccountsReportingService
    {
        Task<RptAccountsLedgerDto> GetLedger(string token, RptAccountsLedgerDto rptLedgerDto);
        Task<Response> GetLedgerResponse(string token, RptAccountsLedgerDto rptLedgerDto);



        Task<Response> GetTrialBalanceResponse(string token, RptAccountsTrialBalanceDto rptTrialBalanceDto);
        Task<RptAccountsTrialBalanceDto> GetTrialBalance(string token,
                             RptAccountsTrialBalanceDto rptTrialBalanceDto);


        Task<Response> GetIncomeStatementResponse(string token, RptAccountsIncomeStatementDto rptIncomeStatementDto);
        Task<RptAccountsIncomeStatementDto> GetIncomeStatement(string token,
                                                               RptAccountsIncomeStatementDto rptIncomeStatementDto);


        Task<Response> GetBalanceSheetResponse(string token,
                                                                RptAccountBalanceSheetDto rptAccountBalanceSheetDto);


        Task<RptAccountBalanceSheetDto> GetBalanceSheet(string token,
                                                        RptAccountBalanceSheetDto rptAccountBalanceSheetDto);
    }
}
