using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.UserManagement.CompanyServices
{
    public interface ICompanyService
    {
        Task<Response> Edit(string token, CompanyDto user, IFormFile logoFile = null);
        Task<Response> Create(CompanyDto user, string token);
        Task<Response> GetByUserId(int userId, string token);
        Task<Response> GetCompanyById( string token);


        Task<Response> SetupPrinter(string token,
                                    CompanyDto modelCompany);
    }
}
