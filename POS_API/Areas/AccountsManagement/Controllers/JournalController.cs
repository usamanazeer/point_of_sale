using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.Accounts;
using Models.Enums;
using POS_API.Services.AccountsManagement.JournalServices;
using POS_API.Utilities.Authentication;
using StatusCodes = Models.Enums.StatusCodes;

namespace POS_API.Areas.AccountsManagement.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class JournalController : BaseController
    {
        private readonly IJournalService _journalService;
        public JournalController(ILogger<JournalController> logger,
                 IAuthenticationUtilities authenticationService
        , IJournalService journalService) : base(logger,authenticationService) =>
            _journalService = journalService;


        [HttpPost(template: nameof(Get))]
        public async Task<ActionResult> Get(AccTransactionMasterDto transactionMasterDto)
        {
            var response = new Response();
            try
            {
                transactionMasterDto.CompanyId = COMPANY_ID;
                response = await _journalService.GetAll(model: transactionMasterDto);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception )
            {
                response.SetError($"Api Error while Getting Journal Entries."); 
                return BadRequest(error: response);
            }
        }

        [HttpPost("AddTransaction")]
        public async Task<ActionResult> AddTransaction(AccTransactionMasterDto transactionMasterDto)
        {
            var response = new Response();
            try
            {
                transactionMasterDto.CompanyId = COMPANY_ID;
                transactionMasterDto.CreatedBy = USER_ID;
                transactionMasterDto.CreatedOn = DateTime.Now;
                transactionMasterDto.Status = StatusTypes.Active.ToInt();
                transactionMasterDto.SystemMade = false;
                transactionMasterDto.AccTransactionDetail = transactionMasterDto.AccTransactionDetail.Where(x => x.Status != StatusTypes.Delete.ToInt()).ToList();
                response = await _journalService.AddTransaction(transactionMasterDto);
                transactionMasterDto = (AccTransactionMasterDto)response.Model;
                return StatusCode(response.ErrorOccured != true ? response.ResponseCode : response.ErrorCode, response);
            }
            catch(Exception )
            {
                response.SetMessage("Api Error while Saving Transaction.");
                response.Model = transactionMasterDto;
                return BadRequest(response);
            }
        }

        [HttpGet(template: "VerifyJournalEntry/{id}")]
        public async Task<ActionResult> VerifyJournalEntry(int id)
        {
            var response = new Response();
            var model = new AccTransactionMasterDto();
            try
            {
                model.Id = id;
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                if (await _journalService.VerifyJournalEntry(model: model))
                    response.SetMessage("Journal Entry Verified.", model: true);
                else
                    response.SetMessage("Journal Entry Not Found.", StatusCodes.Not_Found, false);
                return Ok(value: response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while verifying journal entry.", model:false);
                return StatusCode(statusCode: StatusCodes.Error_Occured.ToInt(), value: response);
            }
        }
    }
}
