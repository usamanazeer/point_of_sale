

namespace Models.DTO.InventoryManagement
{
    public class InvModifierItemDto : BaseModel
    {
        public int ModifierId { get; set; }
        public int ItemId { get; set; }
        public int? BarCodeId { get; set; }
        public double Quantity { get; set; }

        public virtual InvItemDto Item { get; set; }
        public virtual InvItemBarCodeDto BarCode { get; set; }
        public virtual InvModifierDto Modifier { get; set; }
    }
}
