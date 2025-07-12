using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Notifications.ViewDTO;
using Pos_WebApp.Services.UserManagement.NotificationServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.ViewComponents
{
    public class NotificationsList:ViewComponent
    {
        private readonly INotificationService _notificationService;
        public NotificationsList(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IList<NotiUserNotificationsViewDto> notiList = new List<NotiUserNotificationsViewDto>();
            var token = HttpContext.Session.GetString("token");

            if (token != null)
            {
                notiList = await _notificationService.GetUserNotifications(token);
            }
            return View(notiList);
        }
    }
}
