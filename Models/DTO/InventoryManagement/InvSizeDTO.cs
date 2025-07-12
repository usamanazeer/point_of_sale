using System.Collections.Generic;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvSizeDto : ExtendedBaseModel
    {
        public InvSizeDto()
        {
            InvItem = new List<InvItemDto>();
            Sizes = new List<InvSizeDto>();
        }

        public string Name { get; set; }

        public List<InvItemDto> InvItem { get; set; }
        //dto attribute
        public List<InvSizeDto> Sizes { get; set; }
    }
}
