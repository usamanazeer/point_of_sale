using System;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;
using Models.Enums;
using POS_API.Repositories.AccountsManagement.AccountRepositories;

namespace POS_API.Services.AccountsManagement.AccountServices
{
    public class AccountService : IAccountService, IService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService( IAccountRepository accountRepository) => _accountRepository = accountRepository;

        public async Task<Response> GetChartOfAccount(int companyId)
        {
            var response = new Response();
            var res = await _accountRepository.GetChartOfAccount(companyId: companyId);
            if (res.Any())
                response.SetMessage(null, model: res);
            else
                response.SetMessage("No Data Found For Chart Of Accounts.", StatusCodes.Not_Found);
            return response;
        }

        public async Task<Response> Create(AccAccountDto accAccountDto)
        {
            var response = new Response();
            var isExists = await IsExist(accAccountDto: accAccountDto);
            if (!isExists)
            {
                accAccountDto.AllowForManualTransaction = !accAccountDto.IsParent;
                var retRes = await _accountRepository.Create(accAccountDto: accAccountDto);
                response.SetMessage("Account Created Successfully.", StatusCodes.Created, retRes);
                return response;
            }
            response.SetError("Account Already Exists.", model: accAccountDto);
            return response;
        }

        public async Task<Response> Edit(AccAccountDto accAccountDto)
        {
            var response = new Response();
            var isExists = await IsExist(accAccountDto: accAccountDto);
            if (!isExists)
            {
                response.Model = await _accountRepository.Edit(accAccountDto: accAccountDto);
                if (response.Model != null)
                    response.SetMessage("Account Updated Successfully.", StatusCodes.Updated);
                else
                    response.SetMessage("Account Not Found.", StatusCodes.Not_Found);
                return response;
            }
            response.SetError("Account Already Exists.", model: accAccountDto);
            return response;
        }

        public async Task<Response> GetDetails(AccAccountDto accAccountDto)
        {
            var response = new Response();
            var res = await _accountRepository.GetDetails(accAccountDto: accAccountDto);

            if (res != null)
                response.SetMessage(null, model: res);
            else
                response.SetMessage("Account Not Found.", StatusCodes.Not_Found);
            return response;
        }

        public async Task<Response> GetAccountTypes()
        {
            var response = new Response();
            var res = await _accountRepository.GetAccountTypes();
            if (res.Any())
                response.SetMessage(null, model: res);
            else
                response.SetMessage("Account Types Not Found.", StatusCodes.Not_Found);
            return response;
        }

        [Obsolete]
        public async Task<Response> GetAllAccounts(int companyId)
        {
            var response = new Response();
            var res = await _accountRepository.GetAllAccounts(companyId: companyId);
            if (res.Any())
                response.SetMessage(null, model: res);
            else
                response.SetMessage("Accounts Not Found.", StatusCodes.Not_Found);
            return response;
        }

        public async Task<Response> GetAccountsSelectList(int companyId, bool skipSystemMade = false,
                                                          bool skipIfParent = false,
                                                          bool selectIfParent = false,
                                                          bool selectForManualTransactions = false, bool selectBankAccountsOnly = false)
        {
            var response = new Response();
            var itemsList = await _accountRepository.GetAccountsSelectList(companyId: companyId, skipSystemMade, skipIfParent, selectIfParent, selectForManualTransactions, selectBankAccountsOnly);
            if (itemsList != null)
                response.SetMessage(null, model: itemsList);
            else
                response.SetMessage("Delivery Boy Not Found.", StatusCodes.Not_Found);
            return response;
        }

        private async Task<bool> IsExist(AccAccountDto accAccountDto) => await _accountRepository.IsExist(accAccountDto: accAccountDto);
    }
}