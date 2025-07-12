//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Models;
//using Models.DTO.Notifications;
//using Models.DTO.UserManagement;
//using Models.DTO.ViewModels.SelectList.UserManagement;

//namespace POS_API.Repositories.UserManagement
//{
//    public interface IUserUnit
//    {
//        #region User Region
//        Task<UserDto> RegisterCompany(UserDto user);
//        Task<UserDto> Login(AuthenticationDto user);
//        Task<bool> IsUserExist(UserDto user);
//        Task<List<UserDto>> GetAllUsers(UserDto model);
//        Task<UserDto> GetUserById(UserDto model);
//        Task<UserDto> CreateUser(UserDto user);
//        Task<UserDto> EditUser(UserDto user);
        
//        Task<bool> DeleteUser(UserDto model);
        
//        #endregion End User Region




//        #region Company Region
//        Task<CompanyDto> EditCompany(CompanyDto model);
//        Task<List<CompanyDto>> GetCompanies(SearchModel model);
//        Task<bool> ChangeCompanyStatus(CompanyDto model);
//        Task<CompanyDto> GetCompanyById(int id, bool fromCache = false);
//        Task<CompanyDto> GetCompanyByUserId(int userId);
//        Task<bool> UpdateCompanyLogoPath(CompanyDto model);

//        Task<bool> SetupPrinters(CompanyDto companyDto);

//        #endregion End Company Region




//        #region Branches Region

//        Task<IList<BranchDto>> GetAllBranches(BranchDto model);
//        Task<BranchDto> GetBranchDetails(BranchDto model);
//        Task<BranchDto> CreateBranch(BranchDto model);
//        Task<BranchDto> EditBranch(BranchDto model);
//        Task<bool> DeleteBranch(BranchDto model);
//        Task<bool> IsBranchExists(BranchDto model);
//        Task<IList<Branch_SLM>> GetBranchSelectList(BranchDto model);
//        #endregion End Branches Region




//        #region Rights Region
//        Task<bool> ChangeRightStatus(RightsDto model);
        
//        Task<RightsDto> CreateRight(RightsDto model);
//        Task<RightsDto> EditRight(RightsDto model);
//        Task<IEnumerable<CompanyModulesDto>> GetAllRights(CompanyModulesDto model);
        
//        Task<IEnumerable<RightsDto>> GetRightsByRole(RoleDto role);
//        Task<bool> ClaimRight(RoleRightsDto model);
//        Task<RightsDto> ClaimRight1(RoleRightsDto model);
//        #endregion End Rights Region









//        #region Roles Region
//        Task<RoleDto> ActiveRole(int id);
        
//        Task<RoleDto> CreateRole(RoleDto role);
//        Task<bool> DeleteRole(RoleDto model);
//        Task<RoleDto> GetRole(SearchModel model);
//        Task<IEnumerable<RoleDto>> GetAllRoles(RoleDto model);
//        Task<RoleDto> EditRole(RoleDto model);
//        Task<RoleDto> InActiveRole(int id);
//        Task<bool> IsRoleExists(RoleDto model);
//        Task<IList<NotiRoleNotificationDto>> GetRoleNotificationTypes(RoleDto roleDto);

//        #endregion End Roles Region

//    }
//}
