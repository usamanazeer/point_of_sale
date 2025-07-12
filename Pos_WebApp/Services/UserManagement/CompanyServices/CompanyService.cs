using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.UserManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System;
using System.Threading.Tasks;
using StatusCodesEnum = Models.Enums.StatusCodes;

namespace Pos_WebApp.Services.UserManagement.CompanyServices
{

    public class CompanyService : ServiceBase, ICompanyService, IService
    {
        public CompanyService(IClientManager clientManager) : base("api/company/", clientManager)
        {}
        
        public Task<Response> Create(CompanyDto user, string token) => throw new NotImplementedException();


        public async Task<Response> Edit(string token,CompanyDto model, IFormFile logoFile = null)
        {
            var response = await Client.Post<Response>($"{Route}Edit", model, token: token);
            if (response.ResponseCode == StatusCodesEnum.Updated.ToInt() && logoFile != null)
            {
                var res1 = await Client.Post<Response>($"{Route}SaveLogo", model, logoFile, token: token);
                if (!res1.ErrorOccured)
                {
                    if (res1.Model != null)
                        model.Logo = JsonConvert.DeserializeObject<CompanyDto>(res1.Model.String()).Logo;
                }
                else
                {
                    response = res1;
                }
            }
            response.Model = model;
            return response;
        }

        public async Task<Response> GetByUserId(int userId, string token)
        {
            var response = await Client.Get<Response>($"{Route}GetByUserId?userId={userId}", token);
            response.Model = JsonConvert.DeserializeObject<CompanyDto>(response.Model.String());
            return response;
        }

        public async Task<Response> GetCompanyById(string token)
        {
            var response = await Client.Get<Response>($"{Route}GetCompanyById", token);
            response.Model = JsonConvert.DeserializeObject<CompanyDto>(response.Model.String());
            return response;
        }


        public async Task<Response> SetupPrinter(string token, CompanyDto modelCompany)
        {
            var res = await Client.Post<Response>(url: $"{Route}SetupPrinters", obj: modelCompany, token);
            if (res.Model != null) res.Model = res.Model;
            return res;
        }
    }
}
