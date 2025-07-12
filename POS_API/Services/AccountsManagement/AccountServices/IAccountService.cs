using System;
using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;

namespace POS_API.Services.AccountsManagement.AccountServices
{
    public interface IAccountService
    {
        Task<Response> GetChartOfAccount(int companyId);
        Task<Response> Create(AccAccountDto accAccountDto);
        Task<Response> Edit(AccAccountDto accAccountDto);
        Task<Response> GetDetails(AccAccountDto accAccountDto);
        Task<Response> GetAccountTypes();
        [Obsolete]
        Task<Response> GetAllAccounts(int companyId);
        Task<Response> GetAccountsSelectList(int companyId, bool skipSystemMade = false,
                                             bool skipIfParent = false,
                                             bool selectIfParent = false,
                                             bool selectForManualTransactions = false, bool selectBankAccountsOnly = false);
    }
}
