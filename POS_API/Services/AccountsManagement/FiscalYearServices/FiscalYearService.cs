using System.Linq;
using Models.Enums;
using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;
using POS_API.Repositories.AccountsManagement.FiscalYearRepositories;

namespace POS_API.Services.AccountsManagement.FiscalYearServices
{
    public class FiscalYearService : IFiscalYearService, IService
    {
        private readonly IFiscalYearRepository _fiscalYearRepository;
        public FiscalYearService(IFiscalYearRepository fiscalYearRepository) => _fiscalYearRepository = fiscalYearRepository;

        public async Task<Response> GetAll(AccFiscalYearDto fiscalYearDto)
        {
            var response = new Response();
            var res = await _fiscalYearRepository.GetAll(fiscalYearDto);
            if (res.Any())
                response.SetMessage(null, model: res);
            else
                response.SetMessage("No Fiscal Year Found.", StatusCodes.Not_Found);
            return response;
        }

        public async Task<Response> Create(AccFiscalYearDto fiscalYearDto)
        {
            var response = new Response();
            var isExists = await IsExist(fiscalYearDto);
            if (!isExists)
            {
                var retRes = await _fiscalYearRepository.Create(fiscalYearDto);
                response.SetMessage("Fiscal Year Created Successfully.", StatusCodes.Created, retRes);
                return response;
            }
            response.SetError("Fiscal Year Already Exists.", model: fiscalYearDto);
            return response;
        }
        public async Task<bool> IsExist(AccFiscalYearDto fiscalYearDto) => await _fiscalYearRepository.IsExist(fiscalYearDto);
    }
}
