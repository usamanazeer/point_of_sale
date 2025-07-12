
using System.Threading.Tasks;

namespace POS_API.Utilities.SignalR.NotificationHubs
{
    public interface INotificationHub
    {
        Task SendGroupNotification(string groupName, string sender, string message);
    }
}
