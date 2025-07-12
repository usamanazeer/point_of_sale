using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class AccBillPaymentType
    {
        public AccBillPaymentType()
        {
            AccBillPayment = new HashSet<AccBillPayment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccBillPayment> AccBillPayment { get; set; }
    }
}
