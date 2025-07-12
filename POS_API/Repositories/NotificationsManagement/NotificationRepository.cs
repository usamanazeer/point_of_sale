using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTO.Notifications;
using Models.DTO.Notifications.ViewDTO;
using Models.DTO.UserManagement;
using Models.Enums;
using POS_API.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace POS_API.Repositories.NotificationsManagement
{
    public class NotificationRepository : RepositoryBase, INotificationRepository, IRepository
    {
        public NotificationRepository(PosDB_Context dbContext, IMapper mapper) :base(dbContext, mapper) { }
        public async Task<bool> Save(IList<NotiNotificationDto> notificationList)
        {
            foreach (var notification in notificationList)
            {
                if (notification.TypeId.HasValue && notification.CreatedBy.HasValue && notification.CreatedOn.HasValue)
                    await _dbContext.Noti_SaveNotification(notification.TypeId.Value,
                                                           notification.Title, notification.Message, notification.Url, notification.ReferenceKey,
                                                           notification.CompanyId, notification.CreatedBy.Value,notification.CreatedOn.Value);
            }
            return true;
        }
        
        public async Task<bool> InActive(IList<NotiNotificationDto> notificationsList)
        {
            var notificationsToEdit = new List<NotiNotification>();
            foreach (var notification in notificationsList)
            {
                notificationsToEdit.AddRange(await _dbContext.NotiNotification.Where(x=>x.CompanyId == notification.CompanyId && x.TypeId == notification.TypeId && x.ReferenceKey == notification.ReferenceKey && x.Status == StatusTypes.Active.ToInt()).ToListAsync());
            }
            // ReSharper disable once RedundantAssignment
            notificationsToEdit = notificationsToEdit.Select(x => { x.Status = StatusTypes.InActive.ToInt(); return x; }).ToList();
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<NotiNotificationTypeDto> GetNotificationType(int typeId)
        {
            var notificationType = await _dbContext.NotiNotificationType.FindAsync(typeId);
            return Map<NotiNotificationTypeDto>(notificationType);
        }

        public async Task<IList<NotiUserNotificationsViewDto>> GetUserNotifications(UserDto model)
        {
            var notifications = await _dbContext.NotiUserNotificationsView.AsNoTracking().SingleOrDefaultAsync(x=>x.RecipientId == model.Id && x.CompanyId == model.CompanyId);
            return Map<IList<NotiUserNotificationsViewDto>>(notifications);
        }

        public async Task<bool> SetToSeen(NotiNotificationRecipientDto model)
        {
            var notification = await _dbContext.NotiNotificationRecipient
                .SingleOrDefaultAsync(x =>x.NotificationId == model.NotificationId && x.RecipientId == model.RecipientId && x.CompanyId == model.CompanyId);
            notification.IsSeen = true;
            await SaveChangesAsync();
            return true;
        }

        public async Task<IList<NotiNotificationTypeDto>> GetNotificationTypes()
        {
            var notificationTypes = await _dbContext.NotiNotificationType.AsNoTracking().OrderBy(x=>x.Sequence).ToListAsync();
            return Map<IList<NotiNotificationTypeDto>>(notificationTypes);
        }
    }
}
