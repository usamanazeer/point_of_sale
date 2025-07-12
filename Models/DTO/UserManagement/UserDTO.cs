using Models.DTO.Notifications;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.DTO.UserManagement
{
    public sealed class UserDto : BaseModel
    {
        public UserDto()
        {
            Users = new List<UserDto>();
            Role = new RoleDto();
            Company = new CompanyDto();
            NotiNotificationRecipient = new List<NotiNotificationRecipientDto>();
        }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [JsonIgnore, MinLength(5, ErrorMessage = "Password length must be >= 5.")]
        public string Password { get; set; }

        public bool? Gender { get; set; }

        [DisplayName("Gender")]
        public string GenderName => Gender == true ? "Male" : Gender != null ? "Female" : "";

        [DisplayName("Primary Email")]
        public string PrimaryEmail { get; set; }

        [DisplayName("Secondary Email")]
        public string OtherEmail { get; set; }

        public string Phone { get; set; }
        public bool MainUser { get; set; }


        [DisplayName("Role")]
        public int? RoleId { get; set; }
        public BranchDto Branch { get; set; }
        public CompanyDto Company { get; set; }
        public RoleDto Role { get; set; }
        public List<UserDto> Users { get; set; }
        public IList<NotiNotificationRecipientDto> NotiNotificationRecipient { get; set; }



    }
}
