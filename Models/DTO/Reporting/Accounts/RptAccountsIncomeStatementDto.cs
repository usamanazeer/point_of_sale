using System;
using System.Collections.Generic;
using System.Linq;
using Models.DTO.Accounts;
using Models.Enums;

namespace Models.DTO.Reporting.Accounts
{
    public class RptAccountsIncomeStatementDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CompanyId { get; set; }
        public IList<AccTrialBalanceDto> RevenueTrialBalances { get; set; }
        public IList<AccTrialBalanceDto> ExpenseTrialBalances { get; set; }
        public double TotalRevenue => RevenueTrialBalances.Sum(x => x.Balance);
        public double TotalExpense => ExpenseTrialBalances.Sum(x => x.Balance);
        public double NetIncome => TotalRevenue - TotalExpense;
        public RptAccountsIncomeStatementDto()
        {
            RevenueTrialBalances = new List<AccTrialBalanceDto>();
            ExpenseTrialBalances = new List<AccTrialBalanceDto>();
        }
        public void SetData(List<AccTrialBalanceDto> data) {
            RevenueTrialBalances = data.Where(x => x.AccountTypeId == AccountType.Revenues.ToInt()).ToList();
            ExpenseTrialBalances = data.Where(x => x.AccountTypeId == AccountType.Expenses.ToInt()).ToList();
        }
    }
}
