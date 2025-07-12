using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Services.UserManagement.CompanyServices
{
    public interface ICompanyService
    {
        Task<CompanyDto> Edit(CompanyDto model);
        Task<List<CompanyDto>> Get(SearchModel model);
        Task<CompanyDto> GetByUserId(int userId);
        Task<bool> ChangeStatus(CompanyDto model);
        Task<CompanyDto> GetCompanyById(int id/*, bool fromCache = false*/);
        Task<Response> SaveLogo(CompanyDto model, IFormFile file);
        string GetLogoPath(string logo);
        Task<bool> SetupPrinters(CompanyDto companyDto);
    }
}
