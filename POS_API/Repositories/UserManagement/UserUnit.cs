//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Models;
//using Models.DTO.Notifications;
//using Models.DTO.UserManagement;
//using Models.DTO.ViewModels.SelectList.UserManagement;
//using POS_API.Repositories.UserManagement.BranchRepos;
//using POS_API.Repositories.UserManagement.CompanyRepos;
//using POS_API.Repositories.UserManagement.RightsRepos;
//using POS_API.Repositories.UserManagement.RoleRepos;
//using POS_API.Repositories.UserManagement.UserRepos;

//namespace POS_API.Repositories.UserManagement
//{
//    public class UserUnit : IUserUnit
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly IRoleRepository _roleRepository;
//        private readonly ICompanyRepository _companyRepository;
//        private readonly IRightsRepository _rightsRepository;
//        private readonly IBranchRepository _branchRepository;


//        public UserUnit(IUserRepository userRepository, IRoleRepository roleRepository, ICompanyRepository companyRepository,IBranchRepository branchRepository,  IRightsRepository rightsRepository)
//        {
//            _userRepository = userRepository;
//            _roleRepository = roleRepository;
//            _companyRepository = companyRepository;
//            _rightsRepository = rightsRepository;
//            _branchRepository = branchRepository;
//        }





//        #region User Region
//        public async Task<UserDto> Login(AuthenticationDto user) => await _userRepository.Login(user);
//        public async Task<bool> IsUserExist(UserDto user) => await _userRepository.IsExist(user);
//        public async Task<List<UserDto>> GetAllUsers(UserDto model) => await _userRepository.GetAll(model);
//        public async Task<UserDto> GetUserById(UserDto model) => await _userRepository.GetById(model);
//        public async Task<bool> DeleteUser(UserDto model) => await _userRepository.Delete(model);
//        public async Task<UserDto> EditUser(UserDto user) => await _userRepository.Edit(user);
//        public async Task<UserDto> CreateUser(UserDto user) => await _userRepository.Create(user);
//        #endregion end User Region




//        #region Role Region
//        public async Task<RoleDto> ActiveRole(int id) => await _roleRepository.Active(id);

//        public async Task<RoleDto> CreateRole(RoleDto role) => await _roleRepository.Create(role);

//        public async Task<bool> DeleteRole(RoleDto model) => await _roleRepository.Delete(model);

//        public async Task<RoleDto> GetRole(SearchModel model) => await _roleRepository.Get(model);

//        public async Task<IEnumerable<RoleDto>> GetAllRoles(RoleDto model) => await _roleRepository.GetAll(model);

//        public async Task<RoleDto> InActiveRole(int id) => await _roleRepository.InActive(id);

//        public async Task<RoleDto> EditRole(RoleDto model) => await _roleRepository.Edit(model);
//        public async Task<bool> IsRoleExists(RoleDto model) => await _roleRepository.IsExists(model);

//        public async Task<IList<NotiRoleNotificationDto>> GetRoleNotificationTypes(RoleDto roleDto) => await _roleRepository.GetRoleNotificationTypes(roleDto);
//        #endregion end Role Region




//        #region Company Region
//        public async Task<UserDto> RegisterCompany(UserDto user)
//        {
//            var newCompanyId = await _companyRepository.AddCompany(user.Company);
//            user.CompanyId = newCompanyId;
//            return await _userRepository.Register(user);
//        }
//        public async Task<CompanyDto> EditCompany(CompanyDto model) => await _companyRepository.Edit(model);
//        public async Task<List<CompanyDto>> GetCompanies(SearchModel model) => await _companyRepository.Get(model);
//        public async Task<bool> ChangeCompanyStatus(CompanyDto model) => await _companyRepository.ChangeStatus(model);
//        public async Task<CompanyDto> GetCompanyByUserId(int userId) => await _companyRepository.GetByUserId(userId);
//        public async Task<CompanyDto> GetCompanyById(int id, bool fromCache = false) => await _companyRepository.GetCompanyById(id, fromCache);
//        public async Task<bool> UpdateCompanyLogoPath(CompanyDto model)
//        {
//            return await _companyRepository.UpdateLogoPath(model);
//        }


//        public async Task<bool> SetupPrinters(CompanyDto companyDto)
//        {
//            return await _companyRepository.SetupPrinters(companyDto);
//        }

//        #endregion end Company Region




//        #region Rights Region
//        public async Task<IEnumerable<CompanyModulesDto>> GetAllRights(CompanyModulesDto model)
//        {
//            return await _rightsRepository.GetAll(model);
//        }
//        public async Task<IEnumerable<RightsDto>> GetRightsByRole(RoleDto role) => await _rightsRepository.GetByRole(role);
//        public async Task<bool> ClaimRight(RoleRightsDto model) => await _rightsRepository.ClaimRight(model);
//        public async Task<RightsDto> ClaimRight1(RoleRightsDto model) => await _rightsRepository.ClaimRight1(model);
//        public async Task<RightsDto> CreateRight(RightsDto model) => await _rightsRepository.Create(model);

//        public async Task<RightsDto> EditRight(RightsDto model) => await _rightsRepository.Edit(model);

//        public async Task<bool> ChangeRightStatus(RightsDto model) => await _rightsRepository.ChangeStatus(model);
//        #endregion End Rights Region




//        #region Branches Region

//        public async Task<IList<BranchDto>> GetAllBranches(BranchDto model) => await _branchRepository.GetAll(model);
//        public async Task<BranchDto> GetBranchDetails(BranchDto model) => await _branchRepository.GetDetails(model);
//        public async Task<BranchDto> CreateBranch(BranchDto model) => await _branchRepository.Create(model);
//        public async Task<BranchDto> EditBranch(BranchDto model) => await _branchRepository.Edit(model);
//        public async Task<bool> DeleteBranch(BranchDto model) => await _branchRepository.Delete(model);
//        public async Task<IList<Branch_SLM>> GetBranchSelectList(BranchDto model) => await _branchRepository.GetSelectList(model);
//        public async Task<bool> IsBranchExists(BranchDto model) => await _branchRepository.IsExists(model);
//        #endregion End Branches Region
//    }
//}
