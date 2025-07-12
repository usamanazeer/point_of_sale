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
    public class SubCategoryService : ISubCategoryService, IService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IConfiguration _configuration;

        //private const string SubCategoryImagesFolder = "imgs/SubCategoryImages";
        //private const string DEFAULT_IMAGE = "Default.png";
        public SubCategoryService(ISubCategoryRepository subCategoryRepository, IConfiguration configuration)
        {
            _subCategoryRepository = subCategoryRepository;
            _configuration = configuration;
        }
        private string GetDirectoryPath(string[] param = null)
        {
            var folders = Paths.SubCategoryImagesFolder.Split("/");
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration.GetSection(Paths.AppSettings_RootFolder).Value);
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
            }
            return directoryPath;
        }
        public async Task<Response> Create(InvSubCategoryDto model, IFormFile file)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Sub-Category Already Exists.", model: model);

            var response = new Response();
            var filepath = "";
            if (file != null)
            {
                var fi = new FileInfo(file.FileName);
                var fileName = Guid.NewGuid().ToString() + fi.Extension;

                var directoryPath = GetDirectoryPath();
                filepath = Path.Combine(directoryPath, fileName);
                model.ImageUrl = Paths.SubCategoryImagesFolder + "/" + fileName;
            }
            var res = await _subCategoryRepository.Create(model);
            response.SetMessage("Sub-Category Created Successfully.", StatusCodesEnums.Created, res);

            if (file == null) return response;
            var imageSaved = await file.SaveFileAsync(filepath);
            if (imageSaved) return response;

            response.SetError("Failed to save Sub-Category Image.");
            return response;
        }

        public async Task<bool> IsExist(InvSubCategoryDto model) => await _subCategoryRepository.IsExist(model);

        public async Task<Response> Delete(InvSubCategoryDto model)
        {
            return await _subCategoryRepository.Delete(model) ? Response.Message("Sub-Category Deleted Successfully.", model: true)
                : Response.Message("Sub-Category Not Found.", StatusCodesEnums.Not_Found, false);
        }

        public async Task<Response> Edit(InvSubCategoryDto model, IFormFile file)
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
                model.ImageUrl = Paths.SubCategoryImagesFolder + "/" + fileName;
            }

            var res = await _subCategoryRepository.Edit(model);
            if (res == null)
                return Response.Error("Sub-Category Not Found.", StatusCodesEnums.Not_Found, model);

            var response = new Response();

            if (file != null)
            {
                var imageSaved = await file.SaveFileAsync(filepath);
                if (!imageSaved)
                {
                    response.SetError("Failed to save Sub-Category Image.");
                }
                else
                {
                    //delete old image
                    if (oldImagePath != null) File.Delete(oldImagePath);
                }
            }
            res.ImageUrl = res.ImageUrl != null ? host + res.ImageUrl : host + Paths.DEFAULT_IMAGE;
            response.SetMessage("Sub-Category Updated Successfully.", StatusCodesEnums.Updated, res);
            return response;
        }

        public async Task<Response> GetAll(InvSubCategoryDto model)
        {
            var host = _configuration.GetSection(Paths.AppSettings_ApiHost).Value;
            var res = await _subCategoryRepository.GetAll(model);
            foreach (var item in res)
                item.ImageUrl = item.ImageUrl != null ? host + item.ImageUrl : host + Paths.DEFAULT_IMAGE;
            
            return res.Any() ? Response.Message(null, model: res) : Response.Message("No Sub-Category Found.", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetSelectList(InvSubCategoryDto model, bool forPos = false)
        {
            var itemsList = await _subCategoryRepository.GetSelectList(model);
            return itemsList != null ? Response.Message(null,model: itemsList) : Response.Message("Subcategories Not Found.", model: StatusCodesEnums.Not_Found);
        }
    }
}
