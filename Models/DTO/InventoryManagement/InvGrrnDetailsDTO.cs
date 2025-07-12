
namespace Models.DTO.InventoryManagement
{
    public class InvGrrnDetailsDto : ExtendedBaseModel
    {
        public int GrrnId { get; set; }
        public int ItemId { get; set; }
        public string BatchNo { get; set; }
        public double ReturnQuantity { get; set; }
        public double Rate { get; set; }
        public virtual InvGrrnMasterDto Grrn { get; set; }
        public virtual InvItemDto Item { get; set; }
        public double SubTotal => (ReturnQuantity) * Rate;
    }
}
