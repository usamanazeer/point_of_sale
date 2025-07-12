
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.InventoryManagement
{
    public class InvItemBarCodeDto : ExtendedBaseModel
    {
        [DisplayName("Bar Code")]
        public string BarCode { get; set; }
        public int? ItemId { get; set; }

        public virtual IList<InvItemRecipeDto> InvItemRecipe { get; set; }
        public virtual IList<InvModifierItemDto> InvModifierItems { get; set; }
        public virtual IList<InvPhysicalInventoryItemDto> InvPhysicalInventoryItem { get; set; }
        public virtual InvItemDto Item { get; set; }
        //dto prop
        public virtual IList<InvItemBarCodeDto> ItemBarCodes { get; set; }
    }
}