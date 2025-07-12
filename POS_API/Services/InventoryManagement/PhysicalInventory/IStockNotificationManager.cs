using Models.DTO.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.PhysicalInventory
{
    public interface IStockNotificationManager
    {
        Task<bool> LowInventoryNotifications(IList<NotiNotificationDto> notiList);
        Task<bool> RemoveLowInventoryNotifications(IList<NotiNotificationDto> notiList);
    }
}
