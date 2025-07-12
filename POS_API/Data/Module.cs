using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class Module
    {
        public Module()
        {
            CompanyModules = new HashSet<CompanyModules>();
            Rights = new HashSet<Rights>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CraetedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<CompanyModules> CompanyModules { get; set; }
        public virtual ICollection<Rights> Rights { get; set; }
    }
}
