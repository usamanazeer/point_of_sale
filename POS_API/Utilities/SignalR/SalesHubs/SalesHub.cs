using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace POS_API.Utilities.SignalR.SalesHubs
{
    public class SalesHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            //var httpContext = Context.GetHttpContext();
            //var CompanyId = httpContext.Request.Query["CompanyId"];
            //var RoleId = httpContext.Request.Query["RoleId"];
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SalesOccured()
        {
            await Clients.All.SendAsync("SalesOccured");
        }
    }
}
