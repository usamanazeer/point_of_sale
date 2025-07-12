using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class AccAccount
    {
        public AccAccount()
        {
            AccLedgerPosting = new HashSet<AccLedgerPosting>();
            AccTransactionDetail = new HashSet<AccTransactionDetail>();
            InverseParent = new HashSet<AccAccount>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string AccNo { get; set; }
        public int AccountTypeId { get; set; }
        public int? ParentId { get; set; }
        public int AccLevel { get; set; }
        public int CompanyId { get; set; }
        public bool IsEditable { get; set; }
        public bool SystemMade { get; set; }
        public bool IsParent { get; set; }
        public bool AllowForManualTransaction { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual AccAccountType AccountType { get; set; }
        public virtual AccAccount Parent { get; set; }
        public virtual ICollection<AccLedgerPosting> AccLedgerPosting { get; set; }
        public virtual ICollection<AccTransactionDetail> AccTransactionDetail { get; set; }
        public virtual ICollection<AccAccount> InverseParent { get; set; }
    }
}
