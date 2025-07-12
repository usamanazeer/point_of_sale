
namespace Models.DTO.InventoryManagement
{
    public class InvPoDetailsDto : ExtendedBaseModel
    {
        public int PoId { get; set; }
        public int ItemId { get; set; }
        public double RequestedQuantity { get; set; }
        public double Rate { get; set; }

        public virtual InvItemDto Item { get; set; }
        public virtual InvPoMasterDto Po { get; set; }


        //dto
        public double SubTotal => RequestedQuantity * Rate;
    }
}
