namespace Models.DTO.InventoryManagement
{
    public class InvItemRecipeDto : BaseModel
    {
        public int ParentId { get; set; }
        public int ItemId { get; set; }
        public int? BarCodeId { get; set; }
        public double Quantity { get; set; }

        public virtual InvItemBarCodeDto BarCode { get; set; }
        public virtual InvItemDto Item { get; set; }
        public virtual InvItemDto Parent { get; set; }
    }
}
