using System;
using System.Collections.Generic;
using System.Linq;
using Models.DTO.Accounts;
using Models.Enums;

namespace Models.DTO.Reporting.Accounts
{
    public class RptAccountBalanceSheetDto
    {
        public DateTime OnDate { get; set; }
        public int CompanyId { get; set; }
        public Response Response { get; set; }
        public IList<AccTrialBalanceDto> TrialBalanceData { get; set; }

        public IList<AccTrialBalanceDto> AssetData => TrialBalanceData
                                                             .Where(x =>
                                                                        x.AccountTypeId ==
                                                                        AccountType.Asset.ToInt())
                                                             .ToList();
        public IList<AccTrialBalanceDto> LiabilityData => TrialBalanceData
                                                      .Where(x =>
                                                                 x.AccountTypeId ==
                                                                 AccountType.Liability.ToInt())
                                                      .ToList();
        public IList<AccTrialBalanceDto> EquityData => TrialBalanceData
                                                          .Where(x =>
                                                                     x.AccountTypeId ==
                                                                     AccountType.Equity.ToInt())
                                                          .ToList();
        public double NetIncome =>
            (TrialBalanceData.Where(x => x.AccountTypeId == AccountType.Revenues.ToInt())
                            .Sum(x => x.Balance)*(-1)) - TrialBalanceData
                                                   .Where(x => x.AccountTypeId == AccountType.Expenses.ToInt())
                                                   .Sum(x => x.Balance);

        public double LeftSideTotal =>
            TrialBalanceData.Where(x => x.AccountTypeId == AccountType.Asset.ToInt())
                             .Sum(x => x.Balance);


        public double RightSideTotal =>
            (TrialBalanceData.Where(x => x.AccountTypeId == AccountType.Liability.ToInt() || x.AccountTypeId == AccountType.Equity.ToInt())
                            .Sum(x => x.Balance) * (-1)) + NetIncome;
        public RptAccountBalanceSheetDto()
        {
            TrialBalanceData = new List<AccTrialBalanceDto>();
        }
    }
}
