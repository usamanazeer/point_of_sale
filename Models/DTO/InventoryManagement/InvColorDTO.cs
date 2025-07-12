using System.Collections.Generic;

namespace Models.DTO.InventoryManagement
{
    public class InvColorDto : ExtendedBaseModel
    {
        public InvColorDto()
        {
            InvItem = new List<InvItemDto>();
            Colors = new List<InvColorDto>();
        }


        public string Name { get; set; }
        public string ColorValue { get; set; }

        public List<InvItemDto> InvItem { get; set; }
        public List<InvColorDto> Colors { get; set; }
    }
}
