using Models;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DTO.InventoryManagement;

namespace Pos_WebApp.Services.InventoryManagement.PhysicalInventoryServices
{
    public class PhysicalInventoryService : ServiceBase, IPhysicalInventoryService, IService
    {
        public PhysicalInventoryService(IClientManager clientManager) : base("api/PhysicalStock/", clientManager) {}
        public async Task<InvPhysicalInventoryDto> Add(string token ,InvPhysicalInventoryDto model)
        {
            var res = await Client.Post<Response>($"{Route}Add", model, token: token);
            model = JsonConvert.DeserializeObject<InvPhysicalInventoryDto>(res.Model.String());
            model.Response = res;
            return model;
        }

        public async Task<InvPhysicalInventoryDto> Get(string token, InvPhysicalInventoryDto model = null)
        {
            
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new InvPhysicalInventoryDto();
            model.Response = res;
            model.InvPhysicalInventories = JsonConvert.DeserializeObject<List<InvPhysicalInventoryDto>>(res.Model.String());
            return model;
        }

        public async Task<InvPhysicalInventoryViewDto> GetPhysicalInventoryView(string token, PhysicalInventoryViewFilter filters)
        {
            var res = await Client.Post<Response>($"{Route}GetPhysicalInventoryView", filters, token);
            var model = new InvPhysicalInventoryViewDto
            {
                Response = res,
                PhysicalInventoryViews = JsonConvert.DeserializeObject<List<InvPhysicalInventoryViewDto>>(res.Model.String())
            };
            return model;
        }

        public async Task<InvPhysicalInventoryViewDto> GetBillDetails(string token, int billId)
        {
            var res = await Client.Get<Response>($"{Route}BillDetails?id={billId}", token);
            var model = JsonConvert.DeserializeObject<InvPhysicalInventoryViewDto>(res.Model.String());
            return model;
        }

        public async Task<InvPhysicalInventoryViewDto> GetLowInventory(string token, PhysicalInventoryViewFilter filters = null)
        {
            var res = await Client.Post<Response>($"{Route}GetLowInventory", filters, token);
            var model = JsonConvert.DeserializeObject<InvPhysicalInventoryViewDto>(res.Model.String());
            return model;
        }
    }
}
