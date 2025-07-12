using System.Collections.Generic;

namespace Models.DTO.UserManagement
{
    public class BusinessTypeDto : BaseModel
    {
        public string TypeName { get; set; }

        public virtual ICollection<CompanyDto> Company { get; set; }
    }
}
