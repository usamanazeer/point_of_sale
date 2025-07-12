using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.DeliveryService
{
    public class DeliDeliveryBoyDto : ExtendedBaseModel
    {
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Cnic { get; set; }
        public string BikeNo { get; set; }
        public string Email { get; set; }
        [DisplayName(displayName: "Account No")]
        public string AccountNo { get; set; }

        public virtual IList<DeliDeliveryBoyDto> DeliveryBoys { get; set; }
    }
}