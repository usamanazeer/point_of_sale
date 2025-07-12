using System.Collections.Generic;

namespace Models.DTO.Notifications
{
    public sealed class NotiNotificationTypeDto : BaseModel
    {
        public NotiNotificationTypeDto()
        {
            NotiNotification = new List<NotiNotificationDto>();
            NotiRoleNotification = new List<NotiRoleNotificationDto>();
            NotificationTypes = new List<NotiNotificationTypeDto>();
            Response = new Response();
        }

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int? Sequence { get; set; }
        public string ReferenceTable { get; set; }
        public string ReferenceColumn { get; set; }
        public IList<NotiNotificationDto> NotiNotification { get; set; }
        public IList<NotiRoleNotificationDto> NotiRoleNotification { get; set; }
        //dto props
        public IList<NotiNotificationTypeDto> NotificationTypes { get; set; }
    }
}
