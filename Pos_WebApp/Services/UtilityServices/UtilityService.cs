using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using Pos_WebApp.Services.GeneralSettings.TaxServices.UtilityServices;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.UtilityServices
{
    public class UtilityService: ServiceBase, IUtilityService, IService
    {
        public UtilityService(IClientManager clientManager) : base("api/utility/", clientManager)
        {
        }
        public async Task<IList<PrinterInfo>> GetPrinters(string token)
        {
            var res = await GetPrintersResponse(token);
            return res.Model != null ? (IList<PrinterInfo>) res.Model : new List<PrinterInfo>();
        }


        public async Task<Response> GetPrintersResponse(string token)
        {
            var res = await Client.Get<Response>($"{Route}GetPrinters", token);
            if (res.Model != null) res.Model = JsonConvert.DeserializeObject<IList<PrinterInfo>>(res.Model.String());
            return res;
        }
    }
}
