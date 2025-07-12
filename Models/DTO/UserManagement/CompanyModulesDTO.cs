using System.Collections.Generic;

namespace Models.DTO.UserManagement
{
    public class CompanyModulesDto : BaseModel
    {
        public int ModuleId { get; set; }

        public virtual CompanyDto Company { get; set; }
        public virtual ModuleDto Module { get; set; }
        public virtual List<CompanyModulesDto> CompanyModules { get; set; }
    }
}
