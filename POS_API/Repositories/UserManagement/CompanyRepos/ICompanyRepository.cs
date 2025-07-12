using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.UserManagement;

namespace POS_API.Repositories.UserManagement.CompanyRepos
{
    public interface ICompanyRepository
    {
        Task<int> AddCompany(CompanyDto companyDto);
        Task<CompanyDto> Edit(CompanyDto model);
        Task<List<CompanyDto>> Get(SearchModel model);
        Task<bool> ChangeStatus(CompanyDto model);
        Task<CompanyDto> GetByUserId(int userId);
        Task<CompanyDto> GetCompanyById(int id/*, bool fromCache = false*/);
        Task<bool> UpdateLogoPath(CompanyDto model);
        Task<bool> SetupPrinters(CompanyDto companyDto);
    }
}
