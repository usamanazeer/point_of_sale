using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;

namespace POS_API.Services.AccountsManagement.JournalServices
{
    public interface IJournalService
    {
        Task<Response> GetAll(AccTransactionMasterDto model);
        Task<Response> AddTransaction(AccTransactionMasterDto transactionMasterDto);
        Task<bool> VerifyJournalEntry(AccTransactionMasterDto model);
    }
}
