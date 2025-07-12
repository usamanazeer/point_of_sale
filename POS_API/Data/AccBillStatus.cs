using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class AccBillStatus
    {
        public AccBillStatus()
        {
            InvPurchaseMaster = new HashSet<InvPurchaseMaster>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<InvPurchaseMaster> InvPurchaseMaster { get; set; }
    }
}
