using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Models.DTO.InventoryManagement
{
    public sealed class InvPhysicalInventoryDto : ExtendedBaseModel
    {
        public InvPhysicalInventoryDto()
        {
            InvPhysicalInventoryItem = new List<InvPhysicalInventoryItemDto>();
        }

        [DisplayName("Bill Date")]
        public DateTime? BillDate { get; set; }

        [DisplayName("Bill No")]
        public string BillNo { get; set; }

        [DisplayName("Branch")]
        public IList<InvPhysicalInventoryItemDto> InvPhysicalInventoryItem { get; set; }
        //dto props
        public IList<InvPhysicalInventoryDto> InvPhysicalInventories { get; set; }
    }
}
