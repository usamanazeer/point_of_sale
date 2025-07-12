using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class StatusType
    {
        public int Id { get; set; }
        public string StatusType1 { get; set; }
        public bool? Selectable { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
