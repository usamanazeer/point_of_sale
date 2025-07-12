
using System.Collections.Generic;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvUnitDto : ExtendedBaseModel
    {
        public InvUnitDto()
        {
            InvItem = new List<InvItemDto>();
            Units = new List<InvUnitDto>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public List<InvItemDto> InvItem { get; set; }

        //dto property
        public IList<InvUnitDto> Units { get; set; }
    }
}
