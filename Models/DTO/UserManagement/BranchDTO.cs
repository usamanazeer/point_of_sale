using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.UserManagement
{
    public class BranchDto : ExtendedBaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }

        [DisplayName("Show PhoneNo On Bill")]
        public bool ShowPhoneOnBill { get; set; }

        [DisplayName("Show MobileNo On Bill")]
        public bool ShowMobileOnBill { get; set; }
        [DisplayName("Is Main Branch")]
        public bool? IsMainBranch { get; set; }
        public string IsMainBranchText => IsMainBranch == true ? "Yes" : "No";
        public virtual CompanyDto Company { get; set; }
        public virtual IList<UserDto> User { get; set; }
        public List<BranchDto> Branches { get; set; }
    }
}
