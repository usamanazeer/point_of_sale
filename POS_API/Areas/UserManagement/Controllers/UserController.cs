using System;
using Models;
using System.Linq;
using Utilities.SystemUtil;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.UserManagement;
using Utilities.LicenseValidation;
using Microsoft.Extensions.Logging;
using POS_API.Utilities.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using StatusCodesEnums = Models.Enums.StatusCodes;
using StatusTypesEnums = Models.Enums.StatusTypes;
using POS_API.Services.UserManagement.UserServices;

namespace POS_API.Areas.UserManagement.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILicenseValidator _licenseValidator;
        private readonly ISystemUtility _systemUtility;
        private readonly IUserService _userService;
        public UserController(IConfiguration configuration, ISystemUtility systemUtility, ILicenseValidator licenseValidator, ILogger<UserController> logger, 
                              IUserService userService, IAuthenticationUtilities authenticationService) :
            base(logger: logger, authenticationService: authenticationService)
        {
            _configuration = configuration;
            _systemUtility = systemUtility;
            _licenseValidator = licenseValidator;
            _userService = userService;
        }
        
        [HttpPost, Route(template: nameof(Login)), AllowAnonymous]
        public async Task<ActionResult> Login(AuthenticationDto model)
        {
            var response = new Response();
            try
            {
                var licenseKey = CheckForLicense(isLicenseValid: out var isLicenseValid, isExpired: out _, isInvalid: out _);
                //if (licenseKey != null && isLicenseValid)
                if (true)
                {
                    var data = await _userService.Login(user: model);
                    if (data is null)
                    {
                        response.SetError("invalid user.", StatusCodesEnums.UnAuthorized);
                        return Unauthorized(value: response);
                    }
                    data.RoleId ??= 0;
                    var accessToken = _authenticationService.GetAccessToken(Id: data.Id.ToString(), UserName: data.UserName,
                                                                            CompanyId: data.CompanyId.ToString(), RoleId: data.RoleId.ToString());
                    model.Token = accessToken;
                    model.User = data;
                    response.ResponseName = "OK";
                    response.SetMessage("success",model:model);
                    return Ok(value: response);
                }
                if (licenseKey == null) _logger.LogError(message: "Message: \t\tLicense  Not Found.\n\n\n\n");
                if (!isLicenseValid) _logger.LogError(message: "Message: \t\tLicense is Invalid Or Expired.\n\n\n\n");
                return Problem(detail: "License is Invalid Or Expired.");
            }
            catch (Exception ex)
            {
                _logger.LogError(message: $"Message: \t\t{ex.Message}./nStackTrace:\t\t{ex.StackTrace}\n\n\n\n");
                response.SetError(ex.Message);
                return BadRequest(error: response);
            }
        }

        private string CheckForLicense(out bool isLicenseValid, out bool isExpired, out bool isInvalid)
        {
            var licenseKey = _configuration.GetSection(key: "AppSettings:LicenseKey").Value;
            var systemMac = _systemUtility.GetMacAddress();
            _licenseValidator.ValidateLicense(key: licenseKey, systemMacAddress: systemMac, isValid: out isLicenseValid, isExpired: out isExpired, isInvalid: out isInvalid);
            return licenseKey;
        }

        [HttpGet, Route(template: "Get")]
        public async Task<ActionResult> GetAll(int? id,int? status, bool? getDeleted = null)
        {
            var response = new Response();
            try
            {
                var model = new UserDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var data = (await _userService.GetAll(model: model)).ToList();
                if (data.Any())
                {
                    response.Model = data;
                    return Ok(value: response);
                }
                response.SetMessage("No User Found", StatusCodesEnums.Not_Found); 
                return NotFound(value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Users Data.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(Create))]
        public async Task<ActionResult> Create(UserDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                model.Status =  StatusTypesEnums.Active.ToInt();
                var isExists = await _userService.IsExist(user: model);
                if (isExists)
                {
                    response.SetError("User Already Exists.", model: model);
                    return StatusCode(statusCode: StatusCodesEnums.Conflict.ToInt(), value: response);
                }

                var resData = await _userService.Create(user: model);
                response.SetMessage("User Created Successfully.", StatusCodesEnums.Created, resData);
                return StatusCode(statusCode: StatusCodesEnums.Created.ToInt(), value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Creating User.");
                return BadRequest(error: response);
            }
        }

        [HttpPost(template: nameof(Edit))]
        public async Task<ActionResult> Edit(UserDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var isExists = await _userService.IsExist(user: model);
                if (!isExists)
                {
                    var resData = await _userService.Edit(user: model);
                    if (resData != null) response.SetMessage("User Updated Successfully.", StatusCodesEnums.Updated, resData);
                    else response.SetError("User Not Found.", StatusCodesEnums.Not_Found);
                    return Ok(value: response);
                }
                response.SetError("User Already Exists.",model:model);
                return StatusCode(statusCode: StatusCodesEnums.Conflict.ToInt(), value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Updating User.");
                return StatusCode(statusCode:  StatusCodesEnums.Error_Occured.ToInt(), value: response);
            }
        }

        [HttpGet, Route(template: "Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = new Response();
            try
            {
                var model = new UserDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                if (await _userService.Delete(user: model)) response.SetMessage("Role Deleted Successfully.", model: true);
                else response.SetMessage("User Not Found.", StatusCodesEnums.Not_Found, false); 
                return Ok(value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while deleting user.", model:false);
                return StatusCode(statusCode:  StatusCodesEnums.Error_Occured.ToInt(), value: response);
            }
        }
    }
}