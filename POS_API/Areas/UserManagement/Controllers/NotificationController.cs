using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.Notifications;
using Models.DTO.UserManagement;
using POS_API.Services.NotificationsManagement;
using POS_API.Utilities.Authentication;

namespace POS_API.Areas.UserManagement.Controllers
{
    [Route(template: "api/[controller]"), ApiController, Authorize]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(ILogger<NotificationController> logger, IAuthenticationUtilities authenticationService, INotificationService notificationService) 
            : base(logger: logger,authenticationService:authenticationService) => _notificationService = notificationService;

        [HttpGet(template: "Get")]
        public async Task<ActionResult> GetUserNotifications()
        {
            var response = new Response();
            var model = new UserDto();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.Id = USER_ID;
                response = await _notificationService.GetUserNotifications(model: model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting Notifications.");
                return BadRequest(error: response);
            }
        }

        [HttpGet(template: "SetNotificationToSeen/{notiId}")]
        public async Task<ActionResult> SetNotificationToSeen(int notiId)
        {
            var response = new Response();
            var model = new NotiNotificationRecipientDto();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.RecipientId = USER_ID;
                model.NotificationId = notiId;
                response = await _notificationService.SetToSeen(model: model);
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred while viewing notification to seen.");
                return BadRequest(error: response);
            }
        }

        [HttpGet(template: "GetTypes")]
        public async Task<ActionResult> GetNotificationTypes(int notiId)
        {
            var response = new Response();
            try
            {
                response = await _notificationService.GetNotificationTypes();
                return !response.ErrorOccured ? Ok(value: response) : StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred while getting notification types.");
                return BadRequest(error: response);
            }
        }

        [HttpGet(template: nameof(DropOnLoginNotifications))]
        public async Task<ActionResult> DropOnLoginNotifications()
        {
            var response = new Response();
            var model = new NotiNotificationRecipientDto();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.RecipientId = USER_ID;
                response = await _notificationService.SetToSeen(model: model);
                return !response.ErrorOccured ? Ok(value: response): StatusCode(statusCode: response.ErrorCode, value: response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred while setting notification to seen.");
                return BadRequest(error: response);
            }
        }
    }
}