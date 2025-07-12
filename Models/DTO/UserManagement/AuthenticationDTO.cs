namespace Models.DTO.UserManagement
{
    public class AuthenticationDto
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }

        public Response Response { get; set; }
    }
}