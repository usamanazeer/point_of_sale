using Models;
using Models.DTO.Notifications;
using Models.DTO.Notifications.ViewDTO;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.UserManagement.NotificationServices
{
    public class NotificationService : ServiceBase, INotificationService, IService
    {
        public NotificationService(IClientManager clientManager) : base("api/notification/", clientManager)
        {
        }
        public async Task<IList<NotiUserNotificationsViewDto>> GetUserNotifications(string token)
        {
            var res = await Client.Get<Response>($"{Route}Get", token);
            var notificationModel = JsonConvert.DeserializeObject<IList<NotiUserNotificationsViewDto>>(res.Model.String()) ??
                            new List<NotiUserNotificationsViewDto>();
            return notificationModel;
        }

        public async Task<Response> SetNotificationToSeen(string token, int notiId) => await Client.Get<Response>($"{Route}SetNotificationToSeen/{notiId}", token);


        public async Task<NotiNotificationTypeDto> GetNotificationTypes(string token)
        {
            var res = await Client.Get<Response>($"{Route}GetTypes", token);
            var notificationModel = JsonConvert.DeserializeObject<NotiNotificationTypeDto>(res.Model.String()) ?? new NotiNotificationTypeDto();
            notificationModel.Response = res;
            return notificationModel;
        }

        public async Task<Response> DropOnLoginNotifications(string token) => await Client.Get<Response>($"{Route}DropOnLoginNotifications", token);
    }
}
