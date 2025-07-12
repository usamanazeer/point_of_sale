using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;
using Models.Enums;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.AccountsManagement.BillsServices
{
    public class BillsService : ServiceBase, IBillsService, IService                                                                                                                                                                                             
    {
        public BillsService(
                 IClientManager clientManager) : base("api/bill/", clientManager)
        {
        }

        public async Task<BillDto> Details(string token, int id)
        {
            var model = new BillDto();
            var response = await Client.Get<Response>(Route + "Details?id=" + id, token);
            if (response.Model != null)
                model = JsonConvert.DeserializeObject<BillDto>(response.Model.String());
            model.Response = response;
            return model;
        }

        public async Task<BillDto> PayBill(string token,
                                           AccBillPaymentDto model)
        {
            var res = await Client.Post<Response>(Route + "PayBill", model, token: token);

            var returnModel = JsonConvert.DeserializeObject<BillDto>(res.Model.String());
            returnModel ??= new BillDto
                            {
                                Response = res
                            };
            return returnModel;
        }


        public async Task<Response> GetBillsByFilters(string token,
                                      BillDto model)
        {
            var url = $"{Route}GetBillsByFilters";
            
            var res = await Client.Post<Response>(url, model, token);
            if (res.ResponseCode == StatusCodes.OK.ToInt())
            {
                var bills = JsonConvert.DeserializeObject<IList<BillDto>>(res.Model.String());
                res.Model = bills;
            }
            return res;
        }


        public async Task<BillDto> Get(string token, BillDto billDto = null)
        {
            var urlBuilder = new StringBuilder($"{Route}Get");
            if (billDto != null)
            {
                urlBuilder.Append($"?id={billDto.Id}&status={billDto.Status}&getDeleted={billDto.DisplayDeleted}&excludePaidBills={billDto.ExcludePaidBills}");
            }
            var res = await Client.Get<Response>(urlBuilder.ToString(), token);
            billDto ??= new BillDto();
            billDto.Response = res;
            billDto.Bills = JsonConvert.DeserializeObject<List<BillDto>>(res.Model.String());
            return billDto;
        }
    }
}
