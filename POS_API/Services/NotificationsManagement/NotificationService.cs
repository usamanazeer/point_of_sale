using Models;
using Models.DTO.Notifications;
using POS_API.Repositories.NotificationsManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.DTO.UserManagement;
using Models.Enums;

namespace POS_API.Services.NotificationsManagement
{
    public class NotificationService : INotificationService,IService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository) => _notificationRepository = notificationRepository;

        public async Task<bool> Save(IList<NotiNotificationDto> notificationList) => await _notificationRepository.Save(notificationList);

        public async Task<bool> InActive(IList<NotiNotificationDto> notificationList) => await _notificationRepository.InActive(notificationList);

        public async Task<Response> GetUserNotifications(UserDto model)
        {
            var response = new Response();
            var res =  await _notificationRepository.GetUserNotifications(model);
            if (res.Any())
            {
                 response.Model = res;
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "No Notification Found";
            }
            model.Response = response;
            return response;
            
        }

        public async Task<Response> SetToSeen(NotiNotificationRecipientDto model)
        {
            var response = new Response();
            var res = await _notificationRepository.SetToSeen(model);
            if (res)
            {
                response.Model = true;
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode = StatusCodes.Error_Occured.ToInt();
                response.ResponseMessage = "An error Occurred";
            }
            return response;
        }

        public async Task<Response> GetNotificationTypes()
        {
            var response = new Response();
            var model = new NotiNotificationTypeDto
            {
                NotificationTypes = await _notificationRepository.GetNotificationTypes()
            };
            if (model.NotificationTypes.Any())
            {
                response.Model = model;
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "No Notification Type Found";
            }
            return response;
        }
    }
}
