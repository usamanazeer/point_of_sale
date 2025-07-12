using Models.DTO.UserManagement;

namespace Models
{
    public class ExtendedBaseModel : BaseModel
    {
        public UserDto CreatedByUser { get; set; }
        public UserDto ModifiedByUser { get; set; }
        
    }
}
