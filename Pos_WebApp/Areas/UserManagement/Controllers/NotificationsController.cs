using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using _StatusCodes = Models.Enums.StatusCodes;
using Pos_WebApp.Services.UserManagement.NotificationServices;
using Pos_WebApp.Controllers;
using Pos_WebApp.Attributes;

namespace Pos_WebApp.Areas.UserManagement.Controllers
{
    [Area("UserManagement"), Route("[controller]")]
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService) => _notificationService = notificationService;


        [HttpGet("GetUserNotifications")]
        public async Task<IActionResult> GetUserNotifications()=> await Task.FromResult(ViewComponent("NotificationsList"));

        [JsonResponseAction, HttpGet("SetNotificationToSeen/{notiId}")]
        public async Task<JsonResult> SetNotificationToSeen(int notiId)
        {
            var response = new Response();
            try
            {
                response = await _notificationService.SetNotificationToSeen(TOKEN, notiId);
                return Json(response);
            }
            catch (Exception)
            {
                response.ErrorCode = _StatusCodes.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while setting notification to seen.";
                return Json(response);
            }
        }
    }
}
