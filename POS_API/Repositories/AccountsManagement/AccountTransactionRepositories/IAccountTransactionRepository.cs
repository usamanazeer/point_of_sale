using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.Accounts;

namespace POS_API.Repositories.AccountsManagement.AccountTransactionRepositories
{
    public interface IAccountTransactionRepository
    {
        Task<AccTransactionMasterDto> Create(AccTransactionMasterDto accTransactionMaster);
        Task<List<AccTransactionMasterDto>> GetAll(AccTransactionMasterDto accTransactionMaster);
        Task<bool> VerifyJournalEntry(AccTransactionMasterDto model);
    }
}