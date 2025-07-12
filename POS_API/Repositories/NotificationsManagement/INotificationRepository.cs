using Models.DTO.Notifications;
using Models.DTO.Notifications.ViewDTO;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.NotificationsManagement
{
    public interface INotificationRepository
    {
        Task<bool> Save(IList<NotiNotificationDto> notificationList);
        Task<bool> InActive(IList<NotiNotificationDto> notificationList);
        Task<NotiNotificationTypeDto> GetNotificationType(int typeId);
        Task<IList<NotiUserNotificationsViewDto>> GetUserNotifications(UserDto model);
        Task<bool> SetToSeen(NotiNotificationRecipientDto model);
        Task<IList<NotiNotificationTypeDto>> GetNotificationTypes();
    }
}
