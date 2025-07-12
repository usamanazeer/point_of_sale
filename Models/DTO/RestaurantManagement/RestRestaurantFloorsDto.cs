using System.Collections.Generic;

namespace Models.DTO.RestaurantManagement
{
    public sealed class RestRestaurantFloorsDto : ExtendedBaseModel
    {
        public RestRestaurantFloorsDto()
        {
            RestDiningTable = new List<RestDiningTableDto>();
        }


        public string Name { get; set; }

        public IList<RestDiningTableDto> RestDiningTable { get; set; }
        public IList<RestRestaurantFloorsDto> RestaurantFloors { get; set; }
    }
}