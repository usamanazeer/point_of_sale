using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Repositories.InventoryManagement.CategoryRepos;
using POS_API.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Services.InventoryManagement.CategoryServices
{
    public class MainCategoryService : IMainCategoryService, IService
    {
        private readonly IMainCategoryRepository _mainCategoryRepository;
        private readonly IConfiguration _configuration;
        //private const string CategoryImagesFolder = "imgs/CategoryImages";
        //private const string DEFAULT_IMAGE = "Default.png";
        public MainCategoryService(IMainCategoryRepository mainCategoryRepository, IConfiguration configuration)
        {
            _mainCategoryRepository = mainCategoryRepository;
            _configuration = configuration;
        }
        private string GetDirectoryPath(string[] param = null)
        {
            var folders = Paths.CategoryImagesFolder.Split("/");
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration.GetSection(Paths.AppSettings_RootFolder).Value);
            if (param is null)
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
            }
            return directoryPath;
        }
        public async Task<Response> Create(InvCategoryDto model, IFormFile file)
        {
            
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Main-Category Already Exists.", model: model);

            var response = new Response();
            var filepath = "";
            if (file!=null)
            {
                var fi = new FileInfo(file.FileName);
                var fileName = Guid.NewGuid().ToString() + fi.Extension;

                var directoryPath = GetDirectoryPath();
                filepath = Path.Combine(directoryPath, fileName);
                model.ImageUrl = Paths.CategoryImagesFolder + "/" + fileName;
            }
            var res = await _mainCategoryRepository.Create(model);
            response.SetMessage("Main-Category Created Successfully.", StatusCodesEnums.Created, res);

            if (file == null) return response;
            var imageSaved = await file.SaveFileAsync(filepath);
            if (imageSaved) return response;

            response.SetError("Failed to save Category Image.");
            return response;
        }

        public async Task<bool> IsExist(InvCategoryDto model) => await _mainCategoryRepository.IsExist(model);


        public async Task<Response> Delete(InvCategoryDto model)
        {
            return await _mainCategoryRepository.Delete(model) ? Response.Message("Category Deleted Successfully.", model: true)
                : Response.Message("Category Not Found.", StatusCodesEnums.Not_Found, false);
        } 

        public async Task<Response> Edit(InvCategoryDto model, IFormFile file)
        {
            var host = _configuration.GetSection(Paths.AppSettings_ApiHost).Value;
            
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Main-Category Already Exists.", model: model);
            
            string oldImagePath = null;
            model.ImageUrl ??= "";
            model.ImageUrl = model.ImageUrl.Replace(host, "").Replace(Paths.DEFAULT_IMAGE, "");
                
            if (model.ImageUrl != "") oldImagePath = GetDirectoryPath(model.ImageUrl.Split(new[] {'/'}));
            var filepath = "";
            if (file != null)
            {
                var fi = new FileInfo(file.FileName);
                var fileName = Guid.NewGuid().ToString() + fi.Extension;

                var directoryPath = GetDirectoryPath();
                filepath = Path.Combine(directoryPath, fileName);
                model.ImageUrl = Paths.CategoryImagesFolder + "/" + fileName;
            }

            var res = await _mainCategoryRepository.Edit(model);

            if (res == null)
                return Response.Error("Main-Category Not Found.", StatusCodesEnums.Not_Found, model);

            var response = new Response();
            if (file != null)
            {
                var imageSaved = await file.SaveFileAsync(filepath);
                if (!imageSaved)
                {
                    response.SetError("Failed to save Category Image.");
                }
                else
                {
                    //delete old image
                    if (oldImagePath != null) File.Delete(oldImagePath);
                }
            }
            res.ImageUrl = res.ImageUrl != null ? $"{host}{res.ImageUrl}" : $"{host}{Paths.DEFAULT_IMAGE}";
            response.SetMessage("Main-Category Updated Successfully.", StatusCodesEnums.Updated, res);
            return response;
        }

        public async Task<Response> GetAll(InvCategoryDto model)
        {
            var host = _configuration.GetSection(Paths.AppSettings_ApiHost).Value;
            var res = await _mainCategoryRepository.GetAll(model);
            foreach (var item in res)
                item.ImageUrl = item.ImageUrl != null ? $"{host}{item.ImageUrl}" : $"{host}{Paths.DEFAULT_IMAGE}";
            
            return res.Any() ? Response.Message(null, model:res) : Response.Message("No Main-Category Found.", StatusCodesEnums.Not_Found);
            
        }
    }
}