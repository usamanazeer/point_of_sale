using Models.DTO.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Services.UserManagement.UserServices
{
    public interface IUserService
    {
        Task<UserDto> Register(UserDto user);
        Task<UserDto> Login(AuthenticationDto user);
        Task<bool> IsExist(UserDto user);
        Task<List<UserDto>> GetAll(UserDto model);
        Task<UserDto> GetById(UserDto model);
        Task<UserDto> Edit(UserDto user);
        Task<UserDto> Create(UserDto user);
        Task<bool> Delete(UserDto user);
    }
}
