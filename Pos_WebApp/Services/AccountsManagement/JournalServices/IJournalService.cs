using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;

namespace Pos_WebApp.Services.AccountsManagement.JournalServices
{
    public interface IJournalService
    {
        Task<AccTransactionMasterDto> Get(string token, AccTransactionMasterDto model = null);
        Task<AccTransactionMasterDto> Details(string token, int id);
        Task<Response> AddTransaction(string token, AccTransactionMasterDto transactionMasterDto);
        Task<Response> VerifyJournalEntry(string token, int id);
    }
}
