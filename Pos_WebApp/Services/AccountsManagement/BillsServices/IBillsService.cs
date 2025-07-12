using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;

namespace Pos_WebApp.Services.AccountsManagement.BillsServices
{
    public interface IBillsService
    {
        Task<BillDto> Get(string token,
                          BillDto billDto = null);


        Task<BillDto> Details(string token,
                             int id);


        Task<BillDto> PayBill(string token,
                              AccBillPaymentDto model);


        Task<Response> GetBillsByFilters(string token,
                                         BillDto model);
    }
}
