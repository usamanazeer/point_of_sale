using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.VendorServices
{
    public class VendorService : ServiceBase, IVendorService, IService
    {
        public VendorService(IClientManager clientManager) : base("api/vendor/", clientManager) {}

        public Task<bool> IsExist(string token, InvVendorDto vendor) => throw new NotImplementedException();

        public async Task<InvVendorDto> Get(string token, int? id = null, int? status = null, bool? getDeleted = null)
        {
            var model = new InvVendorDto();
            var res =  await Client.Get<Response>($"{Route}Get?id={id}&status={status}&getDeleted={getDeleted}", token);
            model.Response = res;
            model.Vendors = JsonConvert.DeserializeObject<List<InvVendorDto>>(res.Model.String());
            return model;
        }

        public async Task<InvVendorDto> Details(string token, int id)
        {
            var model = new InvVendorDto();
            var response = await Client.Get<Response>(url: $"{Route}Details?id={id}", token: token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvVendorDto>(value: response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<Response> Edit(string token, InvVendorDto vendor)
        {
            var response = await Client.Post<Response>($"{Route}Edit", vendor, token: token);
            if (response.Model != null)
                response.Model = JsonConvert.DeserializeObject<InvVendorDto>(response.Model.String());
            return response;
        }

        public async Task<Response> Create(string token, InvVendorDto vendor)
        {
            var response = await Client.Post<Response>($"{Route}Create", vendor, token: token);
            if (response.Model!=null)
                response.Model = JsonConvert.DeserializeObject<InvVendorDto>(response.Model.String());
            return response;
        }

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<Response> GetSelectListResponse(string token, InvVendorDto model = null)
        {
            var res = await Client.Post<Response>($"{Route}GetSelectList", model ?? new InvVendorDto(), token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<InvVendor_SLM>>(res.Model.String());
            return res;
        }

        public async Task<List<InvVendor_SLM>> GetSelectList(string token, InvVendorDto model = null)
        {
            var res = await GetSelectListResponse(token, model);
            return res.Model != null ? (List<InvVendor_SLM>) res.Model : new List<InvVendor_SLM>();
        }
    }
}
