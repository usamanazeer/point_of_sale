using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;

namespace POS_API.Services.AccountsManagement.FiscalYearServices
{
    public interface IFiscalYearService
    {
        Task<Response> GetAll(AccFiscalYearDto model);
        Task<Response> Create(AccFiscalYearDto fiscalYearDto);
        Task<bool> IsExist(AccFiscalYearDto fiscalYearDto);
    }
}
