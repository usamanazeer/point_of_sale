using System.Threading.Tasks;
using Models.DTO.Accounts;

namespace Pos_WebApp.Services.AccountsManagement.FiscalYearServices
{
    public interface IFiscalYearService
    {
        Task<AccFiscalYearDto> Get(string token,
                                   AccFiscalYearDto model);


        Task<AccFiscalYearDto> Create(string token,
                                   AccFiscalYearDto model);
    }
}
