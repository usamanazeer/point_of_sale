using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.InventoryManagement.PurchaseServices
{
    public class PurchaseService: ServiceBase, IPurchaseService, IService
    {
        public PurchaseService(IClientManager clientManager) : base("api/purchase/", clientManager){}
        public async Task<Response> Create(string token, InvPurchaseMasterDto purchaseMasterDto)
            => DeserializeResponseModel<InvPurchaseMasterDto>(await Client.Post<Response>(Route + "Create", purchaseMasterDto, token: token));


        public async Task<InvPurchaseMasterDto> Get(string token, InvPurchaseMasterDto model = null)
        {
            var url = new StringBuilder( $"{Route}Get");
            if (model != null)
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new InvPurchaseMasterDto();
            model.Response = res;
            model.Purchases = JsonConvert.DeserializeObject<List<InvPurchaseMasterDto>>(res.Model.String());
            return model;
        }


        public async Task<InvPurchaseMasterDto> Details(string token, int id)
        {
            var model = new InvPurchaseMasterDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<InvPurchaseMasterDto>(response.Model.String());
            model.Response = response;
            return model;
        }


        
    }
}
