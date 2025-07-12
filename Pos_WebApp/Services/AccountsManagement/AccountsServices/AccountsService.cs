using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;
using Models.DTO.ViewModels.SelectList.AccountsManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.AccountsManagement.AccountsServices
{
    public class AccountsService :ServiceBase, IAccountsService, IService
    {
        public AccountsService(IClientManager clientManager) : base("api/account/",  clientManager)
        {}

        public async Task<Response> Create(string token, AccAccountDto accountDto)
            => DeserializeResponseModel<AccAccountDto>(await Client.Post<Response>(Route + nameof(Create), accountDto, token: token));


        public async Task<Response> Edit(string token, AccAccountDto accountDto)
            => DeserializeResponseModel<AccAccountDto>(await Client.Post<Response>(Route + nameof(Edit), accountDto, token: token));
        
        public async Task<AccAccountDto> Details(string token, int id)
        {
            var accountDto = new AccAccountDto();
            var response = await Client.Get<Response>(url: Route + "GetDetails?id=" + id, token: token);
            if (response.Model != null)
                accountDto = JsonConvert.DeserializeObject<AccAccountDto>(value: response.Model.String());
            accountDto.Response = response;
            return accountDto;
        }

        public async Task<AccAccountTypeDto> GetChartOfAccounts(string token)
        {
            var accountDto = new AccAccountTypeDto();
            var response = await Client.Get<Response>(url: $"{Route}GetChartOfAccounts", token: token);
            if (response.Model != null)
                accountDto.ChartOfAccounts = JsonConvert.DeserializeObject<IList<AccAccountTypeDto>>(value: response.Model.String());
            accountDto.Response = response;
            return accountDto;
        }


        public async Task<AccAccountTypeDto> GetAccountTypes(string token)
        {
            var accountDto = new AccAccountTypeDto();
            var response = await Client.Get<Response>(url: $"{Route}GetAccountTypes",
                                                       token: token);
            if (response.Model != null)
                accountDto.AccountTypes = JsonConvert.DeserializeObject<IList<AccAccountTypeDto>>(value: response.Model.String());
            accountDto.Response = response;
            return accountDto;
        }


        [Obsolete]
        public async Task<AccAccountDto> GetAllAccounts(string token)
        {
            var accountDto = new AccAccountDto();
            var response = await Client.Get<Response>(url: $"{Route}GetAllAccounts", token: token);
            if (response.Model != null)
                accountDto.AccountsList = JsonConvert.DeserializeObject<IList<AccAccountDto>>(value: response.Model.String());
            accountDto.Response = response;
            return accountDto;
        }


        public async Task<IList<Account_SLM>> GetAccountsSelectList(string token,
                                                                    bool skipSystemMade = false,
                                                                    bool skipIfParent = false,
                                                                    bool selectIfParent = false,
                                                                    bool selectForManualTransactions = false, 
                                                                    bool selectBankAccountsOnly = false)
        {
            var res = await GetAccountsSelectListResponse(token, skipSystemMade, skipIfParent: skipIfParent, selectIfParent, selectForManualTransactions, selectBankAccountsOnly);
            return res.Model != null ? (IList<Account_SLM>) res.Model : new List<Account_SLM>();
        }


        public async Task<Response> GetAccountsSelectListResponse(string token,
                                                                  bool skipSystemMade = false,
                                                                  bool skipIfParent = false,
                                                                  bool selectIfParent = false,
                                                                  bool selectForManualTransactions = false, bool selectBankAccountsOnly = false)
        {
            var res =
                await Client.Get<Response>(url:
                                            $"{Route}GetAccountsSelectList?skipSystemMade={skipSystemMade}&skipIfParent={skipIfParent}&selectIfParent={selectIfParent}&selectForManualTransactions={selectForManualTransactions}&selectBankAccountsOnly={selectBankAccountsOnly}",
                                            token: token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<Account_SLM>>(value: res.Model.String());
            return res;
        }
    }
}