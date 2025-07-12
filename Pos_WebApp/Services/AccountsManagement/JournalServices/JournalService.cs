using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.AccountsManagement.JournalServices
{
    public class JournalService: ServiceBase, IJournalService, IService
    {
        public JournalService(IClientManager clientManager):base("api/journal/",clientManager)
        {}

        public async Task<AccTransactionMasterDto> Details(string token, int id)
        {
            var model = new AccTransactionMasterDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null) model = JsonConvert.DeserializeObject<AccTransactionMasterDto>(response.Model.String());
            model.Response = response;
            return model;
        }


        public async  Task<Response> AddTransaction(string token, AccTransactionMasterDto transactionMasterDto)
        {
            return DeserializeResponseModel<AccTransactionMasterDto>(await Client.Post<Response>(Route + nameof(AddTransaction),
                                                                                                       transactionMasterDto,
                                                                                                       token: token));
        }


        public async Task<Response> VerifyJournalEntry(string token, int id)
        {
            var response = await Client.Get<Response>(url: $"{Route}VerifyJournalEntry/{id}",token: token);
            response.Model = (bool)response.Model;
            return response;
        }


        //public Task<AccTransactionMasterDto> AddTransaction(tokAccTransactionMasterDto transactionMasterDto)
        //{
        //    
        //}


        public async Task<AccTransactionMasterDto> Get(string token, AccTransactionMasterDto model = null)
        {
            var url = $"{Route}Get";
            //if (model != null) url += "?id=" + model.Id + "&status=" + model.Status + "&getDeleted=" + model.DisplayDeleted;
            var res = await Client.Post<Response>(url: url,model??new AccTransactionMasterDto(), token: token);
            model ??= new AccTransactionMasterDto();
            model.Response = res;
            if (res.Model != null)
                model.TransactionsList = JsonConvert.DeserializeObject<List<AccTransactionMasterDto>>(value: res.Model.String());
            return model;
        }
    }
}