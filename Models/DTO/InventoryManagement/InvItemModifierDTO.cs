
namespace Models.DTO.InventoryManagement
{
    public class InvItemModifierDto : BaseModel
    {

        public int ItemId { get; set; }
        public int ModifierId { get; set; }
        public int Quantity { get; set; }
        public bool IsMandatory { get; set; }
        public string IsMandatoryText => IsMandatory == false ? "No" : "Yes";

        public virtual InvItemDto Item { get; set; }
        public virtual InvModifierDto Modifier { get; set; }
    }
}
