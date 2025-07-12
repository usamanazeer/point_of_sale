namespace Models.DTO.UserManagement
{
    public class RoleRightsDto : BaseModel
    {


        public int RoleId { get; set; }
        public int RightId { get; set; }
        //public int? CompanyId { get; set; }

        public virtual RoleDto Role { get; set; }
        public virtual RightsDto Right { get; set; }

    }
}
