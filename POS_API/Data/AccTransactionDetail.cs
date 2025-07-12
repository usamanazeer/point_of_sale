using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class AccTransactionDetail
    {
        public AccTransactionDetail()
        {
            AccLedgerPosting = new HashSet<AccLedgerPosting>();
        }

        public int Id { get; set; }
        public int TransactionMasterId { get; set; }
        public string Statement { get; set; }
        public int AccountId { get; set; }
        public double Dr { get; set; }
        public double Cr { get; set; }

        public virtual AccAccount Account { get; set; }
        public virtual AccTransactionMaster TransactionMaster { get; set; }
        public virtual ICollection<AccLedgerPosting> AccLedgerPosting { get; set; }
    }
}
