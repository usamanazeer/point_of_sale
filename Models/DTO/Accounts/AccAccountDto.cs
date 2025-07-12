using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Models.DTO.Accounts
{
    public sealed class AccAccountDto : ExtendedBaseModel
    {
        public AccAccountDto()
        {
            AccLedgerPosting = new List<AccLedgerPostingDto>();
            AccTransactionDetail = new List<AccTransactionDetailDto>();
            InverseParent = new List<AccAccountDto>();
        }
        public string Title { get; set; }
        public string Code { get; set; }
        public string AccNo { get; set; }
        public int AccountTypeId { get; set; }
        public int? ParentId { get; set; }
        public int AccLevel { get; set; }
        public bool IsEditable { get; set; }
        public bool SystemMade { get; set; }
        public bool HasParentChild => InverseParent != null && InverseParent.Any(x => x.IsParent);
        public bool HasNonParentChild => InverseParent != null && InverseParent.Any(x => x.IsParent==false);
        public bool HasNoChild => InverseParent != null && InverseParent.Any() == false;

        [DisplayName("As Parent Account")]
        public bool IsParent { get; set; }
        public bool AllowForManualTransaction { get; set; }
        public AccAccountTypeDto AccountType { get; set; }
        public AccAccountDto Parent { get; set; }
        public IList<AccAccountDto> InverseParent { get; set; }
        public IList<AccLedgerPostingDto> AccLedgerPosting { get; set; }
        public IList<AccTransactionDetailDto> AccTransactionDetail { get; set; }
        //dto props
        public IList<AccAccountDto> AccountsList { get; set; }

    }
}
