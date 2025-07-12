using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class AccAccountType
    {
        public AccAccountType()
        {
            AccAccount = new HashSet<AccAccount>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccAccount> AccAccount { get; set; }
    }
}
