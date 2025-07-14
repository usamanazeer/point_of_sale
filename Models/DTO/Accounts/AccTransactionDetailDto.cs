
using System.Collections.Generic;

namespace Models.DTO.Accounts
{
    public class AccTransactionDetailDto:BaseModel
    {
        public AccTransactionDetailDto()
        {
            AccLedgerPosting = new List<AccLedgerPostingDto>();
        }
        public int TransactionMasterId { get; set; }
        public string Statement { get; set; }
        public int AccountId { get; set; }
        public decimal Dr { get; set; }
        public decimal Cr { get; set; }

        public virtual AccAccountDto Account { get; set; }
        public virtual AccTransactionMasterDto TransactionMaster { get; set; }
        public virtual IList<AccLedgerPostingDto> AccLedgerPosting { get; set; }
    }
}
