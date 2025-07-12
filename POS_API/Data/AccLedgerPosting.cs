using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class AccLedgerPosting
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AccountId { get; set; }
        public int TransactionMasterId { get; set; }
        public int TransactionDetailId { get; set; }
        public string VoucherNo { get; set; }
        public string Description { get; set; }
        public double Dr { get; set; }
        public double Cr { get; set; }
        public double Balance { get; set; }
        public DateTime? PostedOn { get; set; }
        public int? PostedBy { get; set; }

        public virtual AccAccount Account { get; set; }
        public virtual AccTransactionDetail TransactionDetail { get; set; }
        public virtual AccTransactionMaster TransactionMaster { get; set; }
    }
}
