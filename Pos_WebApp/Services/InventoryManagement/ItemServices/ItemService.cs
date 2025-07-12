using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.InventoryManagement.ViewDTO;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Threading.Tasks;
using StatusCode = Models.Enums.StatusCodes;


namespace Pos_WebApp.Services.InventoryManagement.ItemServices
{
    public class ItemService : ServiceBase, IItemService, IService
    {
        public ItemService(IClientManager clientManager) : base("api/item/", clientManager) {}
        public async Task<Response> Create(string token, InvItemDto model, IFormFile itemImage = null)
        {
            var response = await Client.Post<Response>($"{Route}Create", model, token: token);
            if (response.Model != null)
            {
                model = JsonConvert.DeserializeObject<InvItemDto>(response.Model.String());
                if (response.ResponseCode == StatusCode.Created.ToInt() && itemImage != null)
                {
                    var res1 = await Client.Post<Response>($"{Route}SaveImage", model, itemImage, token: token);
                    if(!res1.ErrorOccured)
                    {
                        if (res1.Model != null)
                            model.ImageUrl = (JsonConvert.DeserializeObject<InvItemDto>(res1.Model.String())).ImageUrl;
                    }
                    else
                    {
                        response = res1;
                    }
                }
            }
            response.Model = model;
            return response;
        }


        public async Task<Response> BulkUpload(IFormFile file, string token)
            => await Client.Post<Response>($"{Route}BulkUpload", null, file, token);


        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<Response> Edit(string token, InvItemDto model, IFormFile itemImage = null)
        {
            var response = await Client.Post<Response>(Route + "Edit", model, token: token);
            if (response.Model != null)
            {
                model = JsonConvert.DeserializeObject<InvItemDto>(response.Model.String());
                if (response.ResponseCode == StatusCode.Updated.ToInt() && itemImage != null)
                {
                    var res1 = await Client.Post<Response>(Route + "SaveImage", model, itemImage, token: token);
                    if (!res1.ErrorOccured)
                    {
                        if (res1.Model != null)
                        {
                            model.ImageUrl = (JsonConvert.DeserializeObject<InvItemDto>(res1.Model.String())).ImageUrl;
                        }
                    }
                    else
                    {
                        response = res1;
                    }
                }
            }
            response.Model = model;
            return response;
        }

        public async Task<InvItemDto> Get(string token, InvItemDto model = null)
        {
            var res = await Client.Post<Response>($"{Route}Get", model?? new InvItemDto(), token);
            model ??= new InvItemDto();
            model.Response = res;
            model.ViewList = res.Model!=null ? JsonConvert.DeserializeObject<List<InvItemViewDto>>(res.Model.String()) : new List<InvItemViewDto>();
            return model;
        }

        public async Task<InvItemDto> Details(string token, int id)
        {
            var model = new InvItemDto();
            
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvItemDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> GetSelectListResponse(string token, InvItemDto model = null)
        {
            var res = await Client.Post<Response>($"{Route}GetSelectList", model??new InvItemDto(), token);
            if (res.Model!=null)
                res.Model = JsonConvert.DeserializeObject<IList<InvItem_SLM>>(res.Model.String());
            return res;
        }
        public async Task<IList<InvItem_SLM>> GetSelectList(string token, InvItemDto model = null)
        {
            var res = await GetSelectListResponse(token,  model);
            return res.Model != null ? (IList<InvItem_SLM>) res.Model : new List<InvItem_SLM>();
        }


        public async Task<string> GetItemsBulkUploadSamplePath(string token)
        {
            var url = $"{Route}GetItemsBulkUploadSamplePath";
            var res = await Client.Get<Response>(url, token);
            return res.Model?.ToString();

        }
    }
}
               