using System.Collections.Generic;

namespace Models.DTO.RestaurantManagement
{
    public class RestWaiterDto : ExtendedBaseModel
    {
        public string Name { get; set; }

        public IList<RestWaiterDto> Waiters { get; set; }
    }
}
