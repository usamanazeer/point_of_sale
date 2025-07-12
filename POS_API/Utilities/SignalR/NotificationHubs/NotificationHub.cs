using Microsoft.AspNetCore.SignalR;
using Models.DTO.Notifications;
using Models.DTO.UserManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS_API.Services.UserManagement.RolesServices;


namespace POS_API.Utilities.SignalR.NotificationHubs
{
    public class NotificationHub : Hub, INotificationHub
    {
        private readonly IRolesService _rolesService;
        public NotificationHub(IRolesService rolesService) {
            _rolesService = rolesService;
        }
        public async override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var CompanyId = httpContext.Request.Query["CompanyId"];
            var RoleId = httpContext.Request.Query["RoleId"];
            IList<NotiRoleNotificationDto> notiRoles = await _rolesService.GetRoleNotificationTypes(new RoleDto() { Id = Convert.ToInt32(RoleId), CompanyId = Convert.ToInt32(CompanyId) });
            foreach (var noti in notiRoles)
            {
                await AddToRoleGroup($"company:{CompanyId}__role:{RoleId}", Context.ConnectionId);
            }
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        private Task AddToRoleGroup(string groupName, string userId) 
        {
            return Groups.AddToGroupAsync(userId, groupName);
        }
        public async Task SendGroupNotification(string groupName, string sender, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGroupNotification", sender, message);
        }
    }
}
