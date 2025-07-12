using System;
using System.Collections.Generic;

namespace POS_API.Data
{
    public partial class Rights
    {
        public Rights()
        {
            InverseParent = new HashSet<Rights>();
            RoleRights = new HashSet<RoleRights>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string CssClasses { get; set; }
        public string LiCssclasses { get; set; }
        public int? ParentId { get; set; }
        public int? SequenceNo { get; set; }
        public int? DepthLevel { get; set; }
        public int? ModuleId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Module Module { get; set; }
        public virtual Rights Parent { get; set; }
        public virtual ICollection<Rights> InverseParent { get; set; }
        public virtual ICollection<RoleRights> RoleRights { get; set; }
    }
}
