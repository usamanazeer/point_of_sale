using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class CompanyModules
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ModuleId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Company Company { get; set; }
        public virtual Module Module { get; set; }
    }
}
