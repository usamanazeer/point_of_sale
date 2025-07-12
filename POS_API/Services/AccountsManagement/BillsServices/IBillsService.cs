using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;

namespace POS_API.Services.AccountsManagement.BillsServices
{
    public interface IBillsService
    {
        Task<Response> GetAll(BillDto model);
        Task<Response> GetDetails(BillDto model);
        Task<Response> PayBill(AccBillPaymentDto billPaymentDto);
    }
}
