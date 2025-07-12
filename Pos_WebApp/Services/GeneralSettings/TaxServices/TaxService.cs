using Models;
using Models.DTO.GeneralSettings;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DTO.Reporting.Sales;

namespace Pos_WebApp.Services.GeneralSettings.TaxServices
{
    public class TaxService : ServiceBase, ITaxService, IService
    {
        public TaxService(IClientManager clientManager) : base("api/tax/", clientManager)
        {
        }
        public async Task<Response> Create(string token, TaxDto model)
        {
            var res = await Client.Post<Response>($"{Route}Create", model, token: token);
            if (res.Model != null)
            {
                res.Model = JsonConvert.DeserializeObject<TaxDto>(res.Model.String());
            }
            return res;
        }

        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>($"{Route}Delete/{id}", token);
            response.Model = (bool)response.Model;
            return response;
        }

        public async Task<Response> Edit(string token, TaxDto model)
        {
            var response = await Client.Post<Response>($"{Route}Edit", model, token: token);
            if (response.Model != null)
            {
                response.Model = JsonConvert.DeserializeObject<TaxDto>(response.Model.String());
            }
            return response;
        }

        public async Task<TaxDto> Get(string token, TaxDto model = null)
        {
            var url = new StringBuilder($"{Route}Get");
            if (model != null)
            {
                url.Append($"?id={model.Id}&status={model.Status}&getDeleted={model.DisplayDeleted}");
            }
            var res = await Client.Get<Response>(url.ToString(), token);
            model ??= new TaxDto();
            model.Response = res;
            model.Taxes = JsonConvert.DeserializeObject<List<TaxDto>>(res.Model.String());
            return model;
        }

        public async Task<TaxDto> Details(string token, int id)
        {
            var model = new TaxDto();
            var response = await Client.Get<Response>($"{Route}Details?id={id}", token);
            if (response.Model != null)
            {
                model = JsonConvert.DeserializeObject<TaxDto>(response.Model.String());
            }
            model.Response = response;
            return model;
        }


        public async Task<Response> GetEnabledForPos(string token)
        {
            var res = await Client.Get<Response>($"{Route}GetEnabledForPos", token);
            if (res.Model != null)
            {
                res.Model = JsonConvert.DeserializeObject<TaxDto>(res.Model.String());
            }
            return res;
        }


        public async Task<Response> GetTaxCollectionReportResponse(string token, RptTaxCollectionDto rptTaxCollectionDto)
        {
            var res = await Client.Post<Response>($"{Route}GetTaxCollectionReport", rptTaxCollectionDto, token: token);
            return res;
        }


        public async Task<RptTaxCollectionDto> GetTaxCollectionReport(string token, RptTaxCollectionDto rptTaxCollectionDto)
        {
            var response = await GetTaxCollectionReportResponse(token, rptTaxCollectionDto);
            if (response.Model != null)
            {
                rptTaxCollectionDto = JsonConvert.DeserializeObject<RptTaxCollectionDto>(response.Model.String());
            }
            rptTaxCollectionDto.Response = response;
            return rptTaxCollectionDto;
        }
    }
}
