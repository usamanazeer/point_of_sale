using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using POS_API.Utilities.Authentication;
using POS_API.Utilities.ReceiptPrinterUtilities;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Areas.GeneralSettings.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class UtilityController : BaseController
    {
        private readonly IReceiptPrinterUtility _printerUtility;

        public UtilityController(ILogger<UtilityController> logger, IAuthenticationUtilities authenticationService, IReceiptPrinterUtility printerUtility)
            : base(logger: logger, authenticationService: authenticationService) 
            => _printerUtility = printerUtility;

        [HttpGet(template: nameof(GetPrinters))]
        public ActionResult GetPrinters()
        {
            var response = new Response();
            try
            {
                var printers = _printerUtility.GetPrintersList();
                response.Model = printers;
                response.ResponseCode = StatusCodesEnums.OK.ToInt();
                return Ok(value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Printers.");
                return BadRequest(error: response);
            }
        }
    }
}