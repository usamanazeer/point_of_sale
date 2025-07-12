using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.RestaurantManagement
{
    public class RestDiningTableDto : ExtendedBaseModel
    {
        [DisplayName("Table No")]
        public int? TableNo { get; set; }
        public int Capacity { get; set; }

        [DisplayName("Floor")]
        public int FloorId { get; set; }
        [DisplayName("Is Occupied?")]
        public bool IsOccupied { get; set; }

        public virtual RestRestaurantFloorsDto Floor { get; set; }
        public virtual IList<RestDiningTableDto> DiningTables { get; set; }
    }
}
