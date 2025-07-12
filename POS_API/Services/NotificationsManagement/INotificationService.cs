using Models;
using Models.DTO.Notifications;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Services.NotificationsManagement
{
    public interface INotificationService
    {
        Task<bool> Save(IList<NotiNotificationDto> notificationList);
        Task<bool> InActive(IList<NotiNotificationDto> notificationList);
        Task<Response> GetUserNotifications(UserDto model);
        Task<Response> SetToSeen(NotiNotificationRecipientDto model);
        Task<Response> GetNotificationTypes();
    }
}
