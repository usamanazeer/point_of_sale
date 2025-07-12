using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models.DTO.Notifications
{
    public sealed class NotiNotificationDto : BaseModel
    {
        //WILL USE TO INACTIVE NOTIFICATION
        public NotiNotificationDto(int referenceKey, int companyId)
        {
            ReferenceKey = referenceKey;
            CompanyId = companyId;
        }
        //WILL USE TO CREATE NOTIFICATION
        public NotiNotificationDto(string title, string message, string url, int referenceKey, int companyId, int createdBy, DateTime? createdOn = null)
        {
            Title = title;
            Message = message;
            Url = url;
            CompanyId = companyId;
            ReferenceKey = referenceKey;
            Status = StatusTypes.Active.ToInt();
            CreatedBy = createdBy;
            CreatedOn = createdOn ?? DateTime.Now;
        }
        public NotiNotificationDto()
        {
            NotiNotificationRecipient = new HashSet<NotiNotificationRecipientDto>();
        }


        public int? TypeId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public int? ReferenceKey { get; set; }

        public NotiNotificationTypeDto Type { get; set; }
        public ICollection<NotiNotificationRecipientDto> NotiNotificationRecipient { get; set; }
    }
}
