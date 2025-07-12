using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.CategoryServices
{
    public class MainCategoryService : ServiceBase, IMainCategoryService, IService
    {

        public MainCategoryService(IClientManager clientManager) : base("api/category/main/", clientManager)
        {}
        public async Task<InvCategoryDto> Get(string token, InvCategoryDto model = null)
        {
            model ??= new InvCategoryDto();
            var res = await Client.Post<Response>($"{Route}Get", obj: model, token: token);
            model.Response = res;
            model.MainCategories = JsonConvert.DeserializeObject<List<InvCategoryDto>>(res.Model.String());
            return model;
        }

        public Task<InvCategoryDto> GetById(int id, string token)
            => throw new NotImplementedException();

        public async Task<Response> Create(string token, InvCategoryDto model, IFormFile categoryImage = null)
            => DeserializeResponseModel<InvCategoryDto>(await Client.Post<Response>($"{Route}Create", model, categoryImage, token: token));

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<Response> Edit(string token, InvCategoryDto model, IFormFile categoryImage = null)
            => DeserializeResponseModel<InvCategoryDto>(await Client.Post<Response>($"{Route}Edit", model, categoryImage, token: token));
    }
}
