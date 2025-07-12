using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Accounts;
using Models.DTO.Reporting.Accounts;
using POS_API.Data;
using POS_API.Repositories.AccountsManagement.AccountRepositories;

namespace POS_API.Repositories.Reporting.AccountsReportingRepos
{
    public class AccountsReportingRepository : RepositoryBase, IAccountsReportingRepository, IRepository
    {
        private readonly IAccountRepository _accountRepository;


        public AccountsReportingRepository(PosDB_Context dbContext,
                                           IMapper mapper,
                                           IAccountRepository accountRepository) : base(dbContext: dbContext,
                                                                                        mapper: mapper)
        {
            _accountRepository = accountRepository;
        }


        public async Task<RptAccountsLedgerDto> GetLedger(RptAccountsLedgerDto accLedgerDto)
        {
            var account = await _dbContext.AccAccount.AsNoTracking()
                .Include(navigationPropertyPath: x => x.AccountType)
                .FirstOrDefaultAsync(predicate: x => x.Id == accLedgerDto.AccountId && x.CompanyId == accLedgerDto.CompanyId);
            if (!account.IsParent)
            {
                var ledgerData1 = await _dbContext.AccLedgerPosting.AsNoTracking()
                    .Where(predicate: x => x.AccountId == accLedgerDto.AccountId && x.TransactionDate >= accLedgerDto.FromDate && x.TransactionDate <= accLedgerDto.ToDate)
                    .OrderBy(keySelector: x => x.TransactionDate)
                    .ThenBy(keySelector: x => x.PostedOn).ToListAsync();

                var opening1 = await _dbContext.AccLedgerPosting.AsNoTracking()
                    .OrderBy(keySelector: x => x.TransactionDate)
                    .LastOrDefaultAsync(predicate: x => x.TransactionDate < accLedgerDto.FromDate && x.AccountId == accLedgerDto.AccountId);
                
                accLedgerDto.OpeningBalance = opening1?.Balance ?? 0;

                accLedgerDto.Account = _mapper.Map<AccAccountDto>(source: account);
                accLedgerDto.AccLedgerPostings = _mapper.Map<IList<AccLedgerPostingDto>>(source: ledgerData1);
                return accLedgerDto;
            }

            var childAccounts = await _accountRepository.GetAllChildAccounts(accountId: account.Id,
                                                                             companyId: account.CompanyId);
            var childAccIds = childAccounts.Where(predicate: x => x.IsParent == false).Select(selector: x => x.Id)
                                           .ToList();
            var ledgerData2 = await _dbContext.AccLedgerPosting.Where(predicate: x =>
                                                                          childAccIds.Contains(x.AccountId) &&
                                                                          x.TransactionDate >=
                                                                          accLedgerDto.FromDate &&
                                                                          x.TransactionDate <= accLedgerDto.ToDate)
                                              .OrderBy(keySelector: x => x.TransactionDate).AsNoTracking().ToListAsync();
            var sqlRaw = $@"SELECT	* FROM ( 
            SELECT *,
            ROW_NUMBER() OVER(PARTITION BY AccountId ORDER BY AccountId, TransactionDate DESC, PostedOn DESC) AS ROWNO FROM dbo.Acc_LedgerPosting WHERE
            AccountId IN({string.Join(separator: ",", values: childAccIds)}) AND
            TransactionDate < '{accLedgerDto.FromDate.ToString(format: "yyyy-MM-dd")}') AS DATA WHERE DATA.ROWNO = 1";

            var openingBalances = await _dbContext.AccLedgerPosting.FromSqlRaw(sql: sqlRaw).ToListAsync();
            var openingDr = openingBalances.Select(selector: x => x.Dr).Sum();
            var openingCr = openingBalances.Select(selector: x => x.Cr).Sum();
            var opening2 = openingDr - openingCr;
            accLedgerDto.OpeningBalance = opening2;

            for (var index = 0; index < ledgerData2.Count; index++)
            {
                var previous = ledgerData2[index - 1];
                var current = ledgerData2[index];
                if (index == 0) {
                    current.Balance = current.Dr > 0 ? opening2 + current.Dr : opening2 - current.Cr;
                    continue;
                }
                current.Balance = current.Dr > 0 ? previous.Balance + current.Dr : previous.Balance - current.Cr;      
            }

            accLedgerDto.Account = _mapper.Map<AccAccountDto>(source: account);
            accLedgerDto.AccLedgerPostings = _mapper.Map<IList<AccLedgerPostingDto>>(source: ledgerData2);
            return accLedgerDto;
        }


        public async Task<RptAccountsTrialBalanceDto> GetTrialBalance(RptAccountsTrialBalanceDto rptTrialBalanceDto)
        {
            var data = await _dbContext.Rpt_Acc_TrialBalanceReport(rptTrialBalanceDto.OnDate, rptTrialBalanceDto.CompanyId);
            rptTrialBalanceDto.TrialBalances = DataTableToTrialBalanceList(data);
            return rptTrialBalanceDto;
        }


        public async Task<RptAccountsIncomeStatementDto> GetIncomeStatement(RptAccountsIncomeStatementDto incomeStatementDto)
        {
            var dt = await _dbContext.Rpt_Acc_IncomeStatementReport(incomeStatementDto.FromDate, incomeStatementDto.ToDate, incomeStatementDto.CompanyId);
            incomeStatementDto.SetData(data: DataTableToTrialBalanceList(dt));
            return incomeStatementDto;
        }

        private List<AccTrialBalanceDto> DataTableToTrialBalanceList(DataTable dt) { 
            return (from DataRow dr in dt.Rows
                    select new AccTrialBalanceDto
                    {
                        AccountId = Convert.ToInt32(dr["AccountId"]),
                        AccNo = Convert.ToString(dr["AccNo"]),
                        Code = Convert.ToString(dr["Code"]),
                        Title = Convert.ToString(dr["Title"]),
                        AccountTypeId = Convert.ToInt32(dr["AccountTypeId"]),
                        Balance = Convert.ToInt32(dr["Balance"]),
                    }).ToList();
        }
    }
}