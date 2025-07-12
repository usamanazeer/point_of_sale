using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.UserManagement;

namespace POS_API.Repositories.UserManagement.UserRepos
{
    public interface IUserRepository
    {
        Task<UserDto> Register(UserDto user);
        Task<UserDto> Login(AuthenticationDto user);
        Task<bool> IsExist(UserDto user);
        Task<UserDto> GetById(UserDto model);
        Task<List<UserDto>> GetAll(UserDto model);
        Task<UserDto> Edit(UserDto user);
        Task<UserDto> Create(UserDto user);
        Task<bool> Delete(UserDto model);
    }
}
