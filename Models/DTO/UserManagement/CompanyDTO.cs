using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Models.DTO.UserManagement
{
    public sealed class CompanyDto : BaseModel
    {
        public CompanyDto()
        {
            Branch = new List<BranchDto>();
            CompanyModules = new List<CompanyModulesDto>();
            //BusinessType = new BusinessTypeDTO();
            User = new List<UserDto>();
        }

        [DisplayName("Company Name")]
        public string Name { get; set; }
        public int BusinessTypeId { get; set; }
        public string Email { get; set; }

        [DisplayName("Company Logo")]
        public string Logo { get; set; }
        [DisplayName("On Desk Printer")]
        public string OnDeskPrinter { get; set; }
        [DisplayName("Kitchen Printer")]
        public string OffDeskPrinter { get; set; }

        [IgnoreDataMember]
        public string LogoPhysicalPath { get; set; }

        public BusinessTypeDto BusinessType { get; set; }
        public IList<CompanyModulesDto> CompanyModules { get; set; }
        public IList<BranchDto> Branch { get; set; }
        public IList<UserDto> User { get; set; }

        //DTO
        public BranchDto MainBranch => Branch.FirstOrDefault(x => x.IsMainBranch == true);

    }
}
