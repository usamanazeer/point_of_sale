using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.Accounts
{
    public sealed class AccTransactionMasterDto:ExtendedBaseModel
    {
        public AccTransactionMasterDto()
        {
            AccLedgerPosting = new List<AccLedgerPostingDto>();
            AccTransactionDetail = new List<AccTransactionDetailDto>();
            TransactionsList = new List<AccTransactionMasterDto>();
        }

        public string TransactionId { get; set; }
        public string Description { get; set; }
        [DisplayName("Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [DisplayName("Reference No")]
        public string ReferenceNo { get; set; }
        public bool SystemMade { get; set; }
        public bool IsPosted { get; set; }
        
        public  IList<AccLedgerPostingDto> AccLedgerPosting { get; set; }
        public IList<AccTransactionDetailDto> AccTransactionDetail { get; set; }
        
        //dto prop
        public IList<AccTransactionMasterDto> TransactionsList { get; set; }
        public bool? SelectUnverifiedOnly { get; set; }
        public bool? SelectVerifiedOnly { get; set; }
        public bool? IncludeDetails { get; set; }
    }
}
