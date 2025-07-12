using System.Collections.Generic;

namespace Models.DTO.UserManagement
{
    public sealed class ModuleDto : BaseModel
    {
        public ModuleDto()
        {
            CompanyModules = new List<CompanyModulesDto>();
            Rights = new List<RightsDto>();
        }


        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public IList<CompanyModulesDto> CompanyModules { get; set; }
        public IList<RightsDto> Rights { get; set; }
    }
}
