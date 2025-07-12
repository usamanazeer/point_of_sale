using Models;
using System.Linq;
using Models.DTO.Accounts;
using System.Threading.Tasks;
using StatusCodesEnums = Models.Enums.StatusCodes;
using POS_API.Repositories.AccountsManagement.BillsRepositories;

namespace POS_API.Services.AccountsManagement.BillsServices
{
    public class BillsService:IBillsService, IService
    {
        private readonly IBillsRepository _billsRepository;

        public BillsService(IBillsRepository billsRepository) => _billsRepository = billsRepository;

        public async Task<Response> GetAll(BillDto model)
        {
            var response = new Response();
            var res = await _billsRepository.GetAll(model);
            if (res.Any())
                response.SetMessage(null, model: res);
            else
                response.SetMessage("No Bill Found", StatusCodesEnums.Not_Found);
            return response;
        }

        public async Task<Response> GetDetails(BillDto model)
        {
            var response = new Response();
            var res = await _billsRepository.GetDetails(model);
            if (res != null)
            {
                response.SetMessage(null, model: res);
            }
            else
            {
                response.SetError("Bill Not Found", StatusCodesEnums.Not_Found);
                response.SetMessage("Bill Not Found", StatusCodesEnums.Not_Found);
            }
            return response;
        }

        public async Task<Response> PayBill(AccBillPaymentDto billPaymentDto) =>
            Response.Error("Bill Paid Successfully.", StatusCodesEnums.Created, await _billsRepository.PayBill(billPaymentDto));
    }
}
