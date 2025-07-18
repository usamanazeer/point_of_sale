﻿using System;

namespace Models.DTO.Accounts
{
    public class AccLedgerPostingDto
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AccountId { get; set; }
        public int TransactionDetailId { get; set; }
        public string VoucherNo { get; set; }
        public string Description { get; set; }
        public double Dr { get; set; }
        public double Cr { get; set; }
        public double Balance { get; set; }
        public DateTime? PostedOn { get; set; }
        public int? PostedBy { get; set; }


        public virtual AccAccountDto Account { get; set; }  
        public virtual AccTransactionDetailDto TransactionDetail { get; set; }
        public virtual AccTransactionMasterDto TransactionMaster { get; set; }
    }
}
