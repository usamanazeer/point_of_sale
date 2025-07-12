using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.CategoryServices
{
    public class SubCategoryService : ServiceBase, ISubCategoryService, IService
    {
        public SubCategoryService(IClientManager clientManager) : base("api/category/sub/", clientManager){}

        public async Task<InvSubCategoryDto> Get(string token, InvSubCategoryDto model)
        {
            model ??= new InvSubCategoryDto();
            var res = await Client.Post<Response>($"{Route}Get", obj: model, token: token);
            model.Response = res;
            model.SubCategories = JsonConvert.DeserializeObject<List<InvSubCategoryDto>>(res.Model.String());
            return model;
        }

        public Task<InvSubCategoryDto> GetById(int id, string token)
            => throw new NotImplementedException();

        public async Task<Response> Edit(string token, InvSubCategoryDto model, IFormFile categoryImage = null)
            => DeserializeResponseModel<InvSubCategoryDto>(await Client.Post<Response>($"{Route}Edit", model, categoryImage, token: token));

        public async Task<Response> Create(string token, InvSubCategoryDto model, IFormFile categoryImage = null)
            => DeserializeResponseModel<InvSubCategoryDto>(await Client.Post<Response>($"{Route}Create", model, categoryImage, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<IList<InvSubCategory_SLM>> GetSelectList(string token, InvSubCategoryDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (IList<InvSubCategory_SLM>) res.Model : new List<InvSubCategory_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, InvSubCategoryDto model = null)
        {
            var url = $"{Route}GetSelectList";
            var res = await Client.Post<Response>(url, model ?? new InvSubCategoryDto(), token);
            if (res.Model != null)
            {
                res.Model = JsonConvert.DeserializeObject<IList<InvSubCategory_SLM>>(res.Model.String());
            }
            return res;
        }
    }
}
