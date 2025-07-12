using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.Accounts;
using Models.DTO.ViewModels.SelectList.AccountsManagement;

namespace POS_API.Repositories.AccountsManagement.AccountRepositories
{
    public interface IAccountRepository
    {
        Task<IList<AccAccountTypeDto>> GetChartOfAccount(int companyId);
        Task<AccAccountDto> Create(AccAccountDto accAccountDto, bool? isEditable = null);
        Task<AccAccountDto> Edit(AccAccountDto accAccountDto, bool forceEdit = false);
        Task<AccAccountDto> GetDetails(AccAccountDto accAccountDto);
        Task<AccAccountDto> GetDetails(int accountId, int companyId);
        Task<bool> IsExist(AccAccountDto accAccountDto);
        Task<IList<AccAccountTypeDto>> GetAccountTypes();

        [Obsolete]
        Task<IList<AccAccountDto>> GetAllAccounts(int companyId);
        Task<IList<Account_SLM>> GetAccountsSelectList(int companyId, bool skipSystemMade = false,
                                                       bool skipIfParent = false,
                                                       bool selectIfParent = false,
                                                       bool selectForManualTransactions = false, bool selectBankAccountsOnly = false);


        Task<AccAccountDto[]> GetAllChildAccounts(int accountId,int companyId);
    }
}