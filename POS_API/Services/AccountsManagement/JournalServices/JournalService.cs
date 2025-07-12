using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO.Accounts;
using Models.Enums;
using POS_API.Repositories.AccountsManagement.AccountTransactionRepositories;

namespace POS_API.Services.AccountsManagement.JournalServices
{
    public class JournalService : IJournalService, IService
    {
        private readonly IAccountTransactionRepository _transactionRepository;

        public JournalService(IAccountTransactionRepository transactionRepository) => _transactionRepository = transactionRepository;

        public async Task<Response> GetAll(AccTransactionMasterDto transactionMasterDto)
        {
            var response = new Response();
            var res = await _transactionRepository.GetAll(accTransactionMaster: transactionMasterDto);
            if (res.Any())
            {
                response.Model = res;
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "No Transaction Found.";
            }
            return response;
        }

        public async Task<Response> AddTransaction(AccTransactionMasterDto transactionMasterDto)
        {
            var response = new Response();
            transactionMasterDto.AccTransactionDetail = transactionMasterDto.AccTransactionDetail.Where(x => x.Status != StatusTypes.Delete.ToInt()).ToList();
            
            var res = await _transactionRepository.Create(transactionMasterDto);
            //res.AccTransactionDetail = null;
            response.Model = res;
            response.ResponseCode = StatusCodes.Created.ToInt();
            response.ResponseMessage = "Transaction Saved Successfully.";
            return response;
        }

        public async Task<bool> VerifyJournalEntry(AccTransactionMasterDto model) => await _transactionRepository.VerifyJournalEntry(model);
    }
}