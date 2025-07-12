using System.Threading.Tasks;

namespace POS_API.Utilities.SignalR.SalesHubs
{
    public interface ISalesHub
    {
        Task SalesOccured();
    }
}
