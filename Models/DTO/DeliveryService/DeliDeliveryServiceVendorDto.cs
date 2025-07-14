using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.DeliveryService
{
    public class DeliDeliveryServiceVendorDto : ExtendedBaseModel
    {
        public string Name { get; set; }

        [DisplayName(displayName: "Service Charges")]
        public decimal ServiceDiscount { get; set; }

        [DisplayName(displayName: "Charges In Percent?")]
        public bool IsServiceDiscountInPercent { get; set; }

        public string IsServiceDiscountInPercentText => IsServiceDiscountInPercent ? "Yes" : "No";

        [DisplayName(displayName: "Is Self")] public bool IsSelf { get; set; }
        public string IsSelfText => IsSelf ? "Yes" : "No";

        [DisplayName(displayName: "Account No")]
        public string AccountNo { get; set; }

        public virtual IList<DeliDeliveryServiceVendorDto> DeliveryServiceVendors { get; set; }
    }
}