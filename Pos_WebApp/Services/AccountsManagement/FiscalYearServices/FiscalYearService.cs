using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.AccountsManagement.FiscalYearServices
{
    public class FiscalYearService : ServiceBase, IFiscalYearService, IService
    {
        public FiscalYearService(IClientManager clientManager) : base("api/FiscalYear/", clientManager)
        {}

        public async Task<AccFiscalYearDto> Get(string token,
                                                AccFiscalYearDto model)
        {
            var url = $"{Route}Get";
            var res = await Client.Post<Response>(url: url, model ?? new AccFiscalYearDto(), token: token);
            model ??= new AccFiscalYearDto();
            model.Response = res;
            if (res.Model != null)
                model.FiscalYears = JsonConvert.DeserializeObject<List<AccFiscalYearDto>>(value: res.Model.String());
            return model;
        }


        public async Task<AccFiscalYearDto> Create(string token, AccFiscalYearDto model)
        {
            var res = await Client.Post<Response>(url: $"{Route}Create", obj: model, token: token);
            model = JsonConvert.DeserializeObject<AccFiscalYearDto>(value: res.Model.String());
            model.Response = res;
            return model;
        }
    }
}
