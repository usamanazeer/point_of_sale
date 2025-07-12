using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.UserManagement;
using POS_API.Repositories.UserManagement.CompanyRepos;
using POS_API.Repositories.UserManagement.UserRepos;

namespace POS_API.Services.UserManagement.UserServices
{
    public class UserService : IUserService, IService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        public UserService(IUserRepository userRepository, ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        public async Task<UserDto> Create(UserDto user) => await _userRepository.Create(user);

        public async Task<UserDto> Edit(UserDto user) => await _userRepository.Edit(user);

        public async Task<List<UserDto>> GetAll(UserDto model) => await _userRepository.GetAll(model);

        public async Task<UserDto> GetById(UserDto model) => await _userRepository.GetById(model);

        public async Task<bool> IsExist(UserDto user) => await _userRepository.IsExist(user);

        public async Task<UserDto> Login(AuthenticationDto user) => await _userRepository.Login(user);

        public async Task<UserDto> Register(UserDto user)
        {
            var newCompanyId = await _companyRepository.AddCompany(user.Company);
            user.CompanyId = newCompanyId;
            return await _userRepository.Register(user);
        }

        public async Task<bool> Delete(UserDto model) => await _userRepository.Delete(model);
    }
}
