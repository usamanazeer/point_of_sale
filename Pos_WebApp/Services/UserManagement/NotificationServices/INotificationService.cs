using Models;
using Models.DTO.Notifications;
using Models.DTO.Notifications.ViewDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.UserManagement.NotificationServices
{
    public interface INotificationService
    {
        Task<IList<NotiUserNotificationsViewDto>> GetUserNotifications(string token);
        Task<Response> SetNotificationToSeen(string token, int notiId);
        Task<NotiNotificationTypeDto> GetNotificationTypes(string token);
        Task<Response> DropOnLoginNotifications(string token);
        
    }
}
