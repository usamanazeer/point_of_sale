using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class AccTransactionMaster
    {
        public AccTransactionMaster()
        {
            AccLedgerPosting = new HashSet<AccLedgerPosting>();
            AccTransactionDetail = new HashSet<AccTransactionDetail>();
        }

        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ReferenceNo { get; set; }
        public int CompanyId { get; set; }
        public bool SystemMade { get; set; }
        public bool IsPosted { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<AccLedgerPosting> AccLedgerPosting { get; set; }
        public virtual ICollection<AccTransactionDetail> AccTransactionDetail { get; set; }
    }
}
