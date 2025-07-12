using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;
using Models.DTO.ViewModels.SelectList.AccountsManagement;

namespace Pos_WebApp.Services.AccountsManagement.AccountsServices
{
    public interface IAccountsService
    {
        Task<Response> Create(string token,
                                   AccAccountDto accountDto);


        Task<Response> Edit(string token, AccAccountDto accountDto);


        Task<AccAccountDto> Details(string token, int id);


        Task<AccAccountTypeDto> GetChartOfAccounts(string token);
        Task<AccAccountTypeDto> GetAccountTypes(string token);


        [Obsolete]
        Task<AccAccountDto> GetAllAccounts(string token);


        Task<IList<Account_SLM>> GetAccountsSelectList(string token,
                                                       bool skipSystemMade = false,
                                                       bool skipIfParent = false,
                                                       bool selectIfParent = false,
                                                       bool selectForManualTransactions = false, 
                                                       bool selectBankAccountsOnly = false);
        Task<Response> GetAccountsSelectListResponse(string token, bool skipSystemMade = false,
                                                     bool skipIfParent = false,
                                                     bool selectIfParent = false,
                                                     bool selectForManualTransactions = false, 
                                                     bool selectBankAccountsOnly = false);
    }
}