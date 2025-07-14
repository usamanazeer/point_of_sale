using System;
using System.Collections.Generic;
using System.Linq;
using Models.DTO.Accounts;

namespace Models.DTO.Reporting.Accounts
{
    public class RptAccountsTrialBalanceDto
    {
        public DateTime OnDate { get; set; }
        public int CompanyId { get; set; }
        public IList<AccTrialBalanceDto> TrialBalances { get; set; }
        public Response Response { get; set; }
        public RptAccountsTrialBalanceDto()
        {
            TrialBalances = new List<AccTrialBalanceDto>();
        }

        public decimal DrTotal => TrialBalances.Where(x => x.Balance >= 0).Sum(x => x.Balance);
        public decimal CrTotal => TrialBalances.Where(x => x.Balance < 0).Sum(x => x.Balance);
    }
}
