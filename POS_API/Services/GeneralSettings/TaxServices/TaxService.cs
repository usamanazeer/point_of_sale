using System.Linq;
using Models;
using Models.DTO.GeneralSettings;
using System.Threading.Tasks;
using Models.DTO.Reporting.Sales;
using StatusCodesEnums = Models.Enums.StatusCodes;
using POS_API.Repositories.GeneralSettings.TaxRepos;

namespace POS_API.Services.GeneralSettings.TaxServices
{
    public class TaxService: ITaxService, IService
    {
        private readonly ITaxRepository _taxRepository;
        public TaxService(ITaxRepository taxRepository) => _taxRepository = taxRepository;

        public async Task<Response> Create(TaxDto model)
        {
            var returnModel = await _taxRepository.Create(model: model);
            var response = returnModel.Response;
            response.Model = model;
            return response;
        }

        public async Task<bool> Delete(TaxDto model) => await _taxRepository.Delete(model);


        public async Task<Response> Edit(TaxDto model)
        {
            var response = new Response();
            var responseModel = await _taxRepository.Edit(model: model);
            if (responseModel is null)
            {
                response.SetMessage("Tax Not Found.", StatusCodesEnums.Not_Found);
            }
            else
            {
                response = responseModel.Response;
                response.Model = responseModel;
            }
            return response;
        }

        public async Task<Response> GetAll(TaxDto model)
        {
            var res = await _taxRepository.GetAll(model);
            return res.Any() ? Response.Message(null, model: res) : Response.Message("No Tax Found", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetDetails(TaxDto model)
        {
            var res = await _taxRepository.GetDetails(model);
            return res != null ? Response.Message(null, model: res) : Response.Message("Tax Not Found", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetEnabledForPos(int companyId)
        {
            var res = await _taxRepository.GetEnabledForPos(companyId);
            return res != null ? Response.Message(null,model:res) : Response.Message("Tax Not Found", StatusCodesEnums.Not_Found);
        }
        public async Task<bool> IsExist(TaxDto model) => await _taxRepository.IsExist(model);

        public async Task<Response> GetTaxCollectionReport(RptTaxCollectionDto rptTaxCollectionDto)
        {
            rptTaxCollectionDto = await _taxRepository.TaxCollectionReport(rptTaxCollectionDto);
            return Response.Message(null,model: rptTaxCollectionDto);
        }
    }
}
