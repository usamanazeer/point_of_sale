using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTO.UserManagement;
using Models.Enums;
using POS_API.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using POS_API.Repositories.UserManagement.CompanyRepos;

namespace POS_API.Services.UserManagement.CompanyServices
{
    public class CompanyService : ICompanyService, IService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IConfiguration _configuration;
        private const string LogoImageFolder = "imgs/company_logos";
        private const string DefaultLogo = "Default_Logo.png";
        private readonly string _host;
        #pragma warning disable 618
        private readonly IHostingEnvironment _hostingEnvironment;
        #pragma warning restore 618
        #pragma warning disable 618
        public CompanyService(ICompanyRepository companyRepository, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        #pragma warning restore 618
        {
            _companyRepository = companyRepository;
            _configuration = configuration;
            _host = _configuration.GetSection("AppSettings:ApiHost").Value;
            _hostingEnvironment = hostingEnvironment;
        }
        public string GetLogoPath(string logo) => logo != null ? $"{_host}{logo}" : $"{_host}{DefaultLogo}";


        public async Task<bool> SetupPrinters(CompanyDto companyDto) => await _companyRepository.SetupPrinters(companyDto);


        private string GetDirectoryPath(string[] param = null)
        {
            var folders = LogoImageFolder.Split("/");
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration.GetSection("AppSettings:RootFolder").Value);
            if (param == null)
            {
                foreach (var folder in folders)
                {
                    directoryPath = Path.Combine(directoryPath, folder);
                    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                }
            }
            else
            {
                foreach (var folder in param)
                    directoryPath = Path.Combine(directoryPath, folder);
                //if (!Directory.Exists(directoryPath))
                //{
                //    Directory.CreateDirectory(directoryPath);
                //}
            }
            return directoryPath;
        }
        public async Task<bool> ChangeStatus(CompanyDto model) => await _companyRepository.ChangeStatus(model);


        public async Task<CompanyDto> Edit(CompanyDto model)
        {
            var res = await _companyRepository.Edit(model);
            res.Logo = res.Logo != null ? _host + res.Logo : _host + DefaultLogo;
            return res;
        }
        public async Task<Response> SaveLogo(CompanyDto model, IFormFile file)
        {
            string oldImagePath = null;
            var response = new Response
            {
                Model = false
            };
            model.Status = StatusTypes.Active.ToInt();
            model = await _companyRepository.GetCompanyById(model.Id??0);
            if (model != null)
            {
                if (model.Id != null)
                {

                    model.Logo ??= "";
                    model.Logo = model.Logo.Replace(_host, "").Replace(DefaultLogo, "");
                    if (model.Logo != "")
                        oldImagePath = GetDirectoryPath(model.Logo.Split(new[] {'/'}));
                    else
                        model.Logo = null;
                }
                var filepath = "";
                if (file != null)
                {
                    var fi = new FileInfo(file.FileName);
                    var fileName = Guid.NewGuid().ToString() + fi.Extension;

                    var directoryPath = GetDirectoryPath();
                    filepath = Path.Combine(directoryPath, fileName);
                    model.Logo = $"{LogoImageFolder}/{fileName}";
                }
                var imageSaved = false;
                if (file != null) imageSaved = await file.SaveFileAsync(filepath);
                if (imageSaved)
                {
                    imageSaved = await _companyRepository.UpdateLogoPath(model);
                    if (imageSaved)
                    {
                        model.Logo = model.Logo != null ? _host + model.Logo : _host + DefaultLogo;
                        response.Model = model;

                        response.ResponseCode = Models.Enums.StatusCodes.OK.ToInt();
                        response.ResponseMessage = "Company Logo Image saved.";
                    }
                    else
                    {
                        response.ErrorCode = Models.Enums.StatusCodes.Error_Occured.ToInt();
                        response.ErrorMessage = "Failed to save Company Logo Image.";
                    }
                    //delete old image
                    if (oldImagePath != null) File.Delete(oldImagePath);
                }
                else
                {
                    response.ErrorCode = Models.Enums.StatusCodes.Error_Occured.ToInt();
                    response.ErrorMessage = "Failed to save Company Logo Image.";
                }
                //LowInventoryNotification
                //_ = SendLowInventoryNotification(model, Convert.ToInt32(model.CreatedBy));

                //response.ResponseCode = StatusCodesEnums.Created.ToInt();
                //response.ResponseMessage = "Item Created Successfully.";
                return response;
            }
            response.Model = false;
            response.ErrorCode = Models.Enums.StatusCodes.Error_Occured.ToInt();
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (model == null) response.ErrorMessage = "Company Not Found";
            return response;
        }
        public async Task<List<CompanyDto>> Get(SearchModel model) => await _companyRepository.Get(model);


        public async Task<CompanyDto> GetByUserId(int userId) => await _companyRepository.GetByUserId(userId);


        public async Task<CompanyDto> GetCompanyById(int id/*, bool fromCache = false*/)
        {
            var res = await _companyRepository.GetCompanyById(id/*, fromCache*/);
            res.LogoPhysicalPath = res.Logo != null ? $"{_hostingEnvironment.WebRootPath}\\{res.Logo}" : $"{_hostingEnvironment.WebRootPath}{DefaultLogo}";
            res.LogoPhysicalPath = res.LogoPhysicalPath.Replace("\\", "/");
            res.Logo = res.Logo != null ? $"{_host}{res.Logo}" : $"{ _host}{ DefaultLogo}";
            return res;
        }
    }
}
