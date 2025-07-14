using System;
using System.Collections.Generic;
using System.Linq;
using Models.DTO.Accounts;

namespace Models.DTO.Reporting.Accounts
{
    public class RptAccountsLedgerDto
    {
        public int CompanyId { get; set; }
        public AccAccountDto Account { get; set; }
        public bool DisplayDeleted { get; set; }
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
        public RptAccountsLedgerDto()
        {
            AccLedgerPostings = new List<AccLedgerPostingDto>();
        }
        public int AccountId {get; set; }
        public decimal OpeningBalance {get; set; }
        public decimal ClosingBalance => AccLedgerPostings.Any() ? AccLedgerPostings.Last().Balance : OpeningBalance;
        public IList<AccLedgerPostingDto> AccLedgerPostings { get; set; }
        public Response Response { get; set; }
    }
}
