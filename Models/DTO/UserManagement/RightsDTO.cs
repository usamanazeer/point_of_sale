using System.Collections.Generic;

namespace Models.DTO.UserManagement
{
    public class RightsDto : BaseModel
    {
        public RightsDto()
        {
            Rights = new List<RightsDto>();
        }

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
        public int? ModuleId { get; set; }
        public int? DepthLevel { get; set; }


        //dto member
        public bool IsSelected { get; set; }


        public virtual ModuleDto Module { get; set; }
        public virtual RightsDto Parent { get; set; }
        public virtual IList<RightsDto> InverseParent { get; set; }
        public virtual ICollection<RoleRightsDto> RoleRights { get; set; }
        //dto member
        public List<RightsDto> Rights { get; set; }
    }
}
