//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Models.DTO.Accounts;
//using Models.DTO.ViewModels.SelectList.AccountsManagement;
//using POS_API.Repositories.AccountsManagement.AccountRepositories;
//using POS_API.Repositories.AccountsManagement.AccountTransactionRepositories;
//using POS_API.Repositories.AccountsManagement.BillsRepositories;
//using POS_API.Repositories.AccountsManagement.FiscalYearRepositories;

//namespace POS_API.Repositories.AccountsManagement
//{
//    public class AccountsUnit : IAccountsUnit
//    {
//        private readonly IAccountRepository _accountRepository;
//        private readonly IAccountTransactionRepository _transactionRepository;
//        private readonly IFiscalYearRepository _fiscalYearRepository;
//        private readonly IBillsRepository _billsRepository;


//        public AccountsUnit(IAccountRepository accountRepository, IAccountTransactionRepository transactionRepository,
//                            IFiscalYearRepository fiscalYearRepository,
//                            IBillsRepository billsRepository)
//        {
//            _accountRepository = accountRepository;
//            _transactionRepository = transactionRepository;
//            _fiscalYearRepository = fiscalYearRepository;
//            _billsRepository = billsRepository;
//        }


//        #region Accounts

//        public async Task<IList<AccAccountTypeDto>> GetChartOfAccount(int companyId)
//        {
//            return await _accountRepository.GetChartOfAccount(companyId: companyId);
//        }


//        public async Task<AccAccountDto> CreateAccount(AccAccountDto accAccountDto)
//        {
//            return await _accountRepository.Create(accAccountDto: accAccountDto);
//        }


//        public async Task<AccAccountDto> EditAccount(AccAccountDto accAccountDto)
//        {
//            return await _accountRepository.Edit(accAccountDto: accAccountDto);
//        }


//        public async Task<AccAccountDto> GetAccountDetails(AccAccountDto accAccountDto)
//        {
//            return await _accountRepository.GetDetails(accAccountDto: accAccountDto);
//        }


//        public async Task<bool> IsAccountExist(AccAccountDto accAccountDto)
//        {
//            return await _accountRepository.IsExist(accAccountDto: accAccountDto);
//        }


//        public async Task<IList<AccAccountTypeDto>> GetAccountTypes()
//        {
//            return await _accountRepository.GetAccountTypes();
//        }

//        [Obsolete]
//        public async Task<IList<AccAccountDto>> GetAllAccounts(int companyId)
//        {
//            return await _accountRepository.GetAllAccounts(companyId);
//        }
//        public async Task<IList<Account_SLM>> GetAccountsSelectList(int companyId, bool skipSystemMade = false,
//                                                                    bool skipIfParent = false,
//                                                                    bool selectIfParent = false,
//                                                                    bool selectForManualTransactions = false, bool selectBankAccountsOnly = false)
//        {
//            return await _accountRepository.GetAccountsSelectList(companyId, skipSystemMade, skipIfParent, selectIfParent, selectForManualTransactions, selectBankAccountsOnly);
//        }
//        #endregion





//        #region Journal
//        public async Task<IList<AccTransactionMasterDto>> GetAllTransactions(AccTransactionMasterDto transactionMasterDto)
//        {
//            return await _transactionRepository.GetAll(transactionMasterDto);
//        }


//        public async Task<AccTransactionMasterDto> AddTransaction(AccTransactionMasterDto transactionMasterDto)
//        {
//            return await _transactionRepository.Create(transactionMasterDto);
//        }


//        public async Task<bool> VerifyJournalEntry(AccTransactionMasterDto model)
//        {
//            return await _transactionRepository.VerifyJournalEntry(model);
//        }




//        #endregion

//        #region Fiscal Year
//        public async Task<IList<AccFiscalYearDto>> GetAllFiscalYears(AccFiscalYearDto fiscalYearDto)
//        {
//            return await _fiscalYearRepository.GetAll(fiscalYearDto);
//        }


//        public async Task<AccFiscalYearDto> CreateFiscalYear(AccFiscalYearDto fiscalYearDto)
//        {
//            return await _fiscalYearRepository.Create(fiscalYearDto);
//        }


//        public async Task<bool> IsFiscalYearExist(AccFiscalYearDto fiscalYearDto)
//        {
//            return await _fiscalYearRepository.IsExist(fiscalYearDto);
//        }

//        #endregion


//        #region Bills

//        public async Task<IList<BillDto>> GetAllBills(BillDto model)
//        {
//            return await _billsRepository.GetAll(model);
//        }


//        public async Task<BillDto> GetBillDetails(BillDto model)
//        {
//            return await _billsRepository.GetDetails(model);
//        }


//        public async Task<BillDto> PayBill(AccBillPaymentDto billPaymentDto)
//        {
//            return await _billsRepository.PayBill(billPaymentDto);
//        }

//        #endregion
//    }
//}