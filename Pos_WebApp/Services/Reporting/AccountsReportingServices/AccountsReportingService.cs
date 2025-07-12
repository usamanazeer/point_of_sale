using System.Threading.Tasks;
using Models;
using Models.Enums;
using Models.DTO.Reporting.Accounts;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.Reporting.AccountsReportingServices
{
    public class AccountsReportingService : ServiceBase, IAccountsReportingService, IService
    {
        public AccountsReportingService(IClientManager clientManager) : base("api/accountreporting/", clientManager){}

        public async Task<Response> GetLedgerResponse(string token, RptAccountsLedgerDto rptLedgerDto)
        {
            var res = await Client.Post<Response>($"{Route}GetLedger", rptLedgerDto, token: token);
            if (res.ResponseCode == StatusCodes.OK.ToInt() && res.Model != null)
                res.Model = JsonConvert.DeserializeObject<RptAccountsLedgerDto>(res.Model.String());
            return res;
        }

        public async Task<RptAccountsLedgerDto> GetLedger(string token, RptAccountsLedgerDto rptLedgerDto)
        {
            var res = await GetLedgerResponse(token, rptLedgerDto);
            if (res.Model != null)
                return (RptAccountsLedgerDto)res.Model;
            return new RptAccountsLedgerDto();
        }

        public async Task<Response> GetTrialBalanceResponse(string token,RptAccountsTrialBalanceDto rptTrialBalanceDto)
        {
            var res = await Client.Post<Response>($"{Route}GetTrialBalance", rptTrialBalanceDto, token: token);
            if (res.ResponseCode == StatusCodes.OK.ToInt() && res.Model != null)
                res.Model = JsonConvert.DeserializeObject<RptAccountsTrialBalanceDto>(res.Model.String());
            return res;
        }

        public async Task<RptAccountsTrialBalanceDto> GetTrialBalance(string token,RptAccountsTrialBalanceDto rptTrialBalanceDto)
        {
            var res = await GetTrialBalanceResponse(token, rptTrialBalanceDto);
            return res.Model != null ? (RptAccountsTrialBalanceDto) res.Model : new RptAccountsTrialBalanceDto();
        }

        public async Task<Response> GetIncomeStatementResponse(string token, RptAccountsIncomeStatementDto rptIncomeStatementDto)
        {
            var res = await Client.Post<Response>($"{Route}GetIncomeStatement", rptIncomeStatementDto, token: token);
            if (res.ResponseCode == StatusCodes.OK.ToInt() && res.Model != null)
                res.Model = JsonConvert.DeserializeObject<RptAccountsIncomeStatementDto>(res.Model.String());
            return res;
        }

        public async Task<RptAccountsIncomeStatementDto> GetIncomeStatement(string token, RptAccountsIncomeStatementDto rptIncomeStatementDto)
        {
            var res = await GetIncomeStatementResponse(token, rptIncomeStatementDto);
            return res.Model != null ? (RptAccountsIncomeStatementDto) res.Model : new RptAccountsIncomeStatementDto();
        }

        public async Task<Response> GetBalanceSheetResponse(string token, RptAccountBalanceSheetDto rptAccountBalanceSheetDto)
        {
            var res = await Client.Post<Response>($"{Route}GetBalanceSheet", rptAccountBalanceSheetDto, token: token);
            if (res.ResponseCode == StatusCodes.OK.ToInt() && res.Model != null)
                res.Model = JsonConvert.DeserializeObject<RptAccountBalanceSheetDto>(res.Model.String());
            return res;
        }

        public async Task<RptAccountBalanceSheetDto> GetBalanceSheet(string token, RptAccountBalanceSheetDto rptAccountBalanceSheetDto)
        {
            var res = await GetBalanceSheetResponse(token, rptAccountBalanceSheetDto);
            return res.Model != null ? (RptAccountBalanceSheetDto) res.Model : new RptAccountBalanceSheetDto();
        }
    }
}
