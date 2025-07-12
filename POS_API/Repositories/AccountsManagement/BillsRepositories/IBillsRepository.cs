using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.Accounts;

namespace POS_API.Repositories.AccountsManagement.BillsRepositories
{
    public interface IBillsRepository
    {
        Task<IList<BillDto>> GetAll(BillDto fiscalYearDto);
        Task<BillDto> GetDetails(BillDto model);
        Task<BillDto> PayBill(AccBillPaymentDto billPaymentDto);
    }
}
