using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.Accounts;

namespace POS_API.Repositories.AccountsManagement.FiscalYearRepositories
{
    public interface IFiscalYearRepository
    {
        Task<IList<AccFiscalYearDto>> GetAll(AccFiscalYearDto fiscalYearDto);
        Task<AccFiscalYearDto> Create(AccFiscalYearDto fiscalYearDto);
        Task<bool> IsExist(AccFiscalYearDto fiscalYearDto);
    }
}
