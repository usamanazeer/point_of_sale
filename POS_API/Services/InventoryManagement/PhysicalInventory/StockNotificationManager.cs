using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using Models.DTO.Notifications;
using Models.Enums;
using POS_API.Repositories.InventoryManagement.PhysicalInventoryRepos;
using POS_API.Services.NotificationsManagement;

namespace POS_API.Services.InventoryManagement.PhysicalInventory
{
    public class StockNotificationManager : IStockNotificationManager, IService
    {

        private readonly INotificationService _notificationService;
        private readonly IPhysicalInventoryRepository _physicalInventoryRepository;

        //private readonly INotificationHub _notificationHub;
        public StockNotificationManager(IPhysicalInventoryRepository _physicalInventoryRepository,
                                        INotificationService notificationService /*, INotificationHub notificationHub*/)
        {
            
            _notificationService = notificationService;
            //_notificationHub = notificationHub;
        }

        public async Task<bool> LowInventoryNotifications(IList<NotiNotificationDto> notificationList)
        {
            notificationList = notificationList.Select(selector: x =>
            {
                x.TypeId = NotificationTypes.Low_Inventory.ToInt();
                return x;
            }).ToList();
            var res = await _notificationService.Save(notificationList: notificationList);
            //await _notificationHub.SendNotification(noti);
            return res;
        }


        public async Task<bool> RemoveLowInventoryNotifications(IList<NotiNotificationDto> notificationList)
        {
            var lowInventory = await _physicalInventoryRepository.GetLowInventory(filters: new PhysicalInventoryViewFilter
                                                                             {
                                                                                 ItemIds = notificationList
                                                                                           .Select(selector: x =>
                                                                                                       x.ReferenceKey ??
                                                                                                       0).ToArray()
                                                                             });
            var lowInventoryItemKeys = lowInventory.Select(selector: x => x.ItemId).ToList();
            notificationList = notificationList
                       .Where(predicate: x =>
                                  x.ReferenceKey.HasValue && lowInventoryItemKeys.Contains(item: x.ReferenceKey.Value))
                       .Select(selector: x =>
                       {
                           x.TypeId = NotificationTypes.Low_Inventory.ToInt();
                           return x;
                       }).ToList();
            return await _notificationService.InActive(notificationList: notificationList);
        }
    }
}