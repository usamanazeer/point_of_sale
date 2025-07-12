//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Models.DTO.Accounts;
//using Models.DTO.ViewModels.SelectList.AccountsManagement;

//namespace POS_API.Repositories.AccountsManagement
//{
//    public interface IAccountsUnit
//    {
//        #region Accounts

//        Task<IList<AccAccountTypeDto>> GetChartOfAccount(int companyId);
//        Task<AccAccountDto> CreateAccount(AccAccountDto accAccountDto);
//        Task<AccAccountDto> EditAccount(AccAccountDto accAccountDto);
//        Task<AccAccountDto> GetAccountDetails(AccAccountDto accAccountDto);
//        Task<bool> IsAccountExist(AccAccountDto accAccountDto);
//        Task<IList<AccAccountTypeDto>> GetAccountTypes();
        
//        [Obsolete]
//        Task<IList<AccAccountDto>> GetAllAccounts(int companyId);
//        Task<IList<Account_SLM>> GetAccountsSelectList(int companyId, bool skipSystemMade = false,
//                                                       bool skipIfParent = false,
//                                                       bool selectIfParent = false,
//                                                       bool selectAllowedForTransactions = false, bool selectBankAccountsOnly = false);

//        #endregion





//        #region Journal
//        Task<IList<AccTransactionMasterDto>> GetAllTransactions(AccTransactionMasterDto transactionMasterDto);
//        Task<AccTransactionMasterDto> AddTransaction(AccTransactionMasterDto transactionMasterDto);
//        Task<bool> VerifyJournalEntry(AccTransactionMasterDto model);
//        #endregion
        
//        #region Fiscal Year
//        Task<IList<AccFiscalYearDto>> GetAllFiscalYears(AccFiscalYearDto fiscalYearDto);
//        Task<AccFiscalYearDto> CreateFiscalYear(AccFiscalYearDto fiscalYearDto);
//        Task<bool> IsFiscalYearExist(AccFiscalYearDto fiscalYearDto);
//        #endregion


//        #region Bills

//        Task<IList<BillDto>> GetAllBills(BillDto model);
//        Task<BillDto> GetBillDetails(BillDto model);
//        Task<BillDto> PayBill(AccBillPaymentDto billPaymentDto);
//        #endregion


//    }
//}