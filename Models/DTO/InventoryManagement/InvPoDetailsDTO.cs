
namespace Models.DTO.InventoryManagement
{
    public class InvPoDetailsDto : ExtendedBaseModel
    {
        public int PoId { get; set; }
        public int ItemId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal Rate { get; set; }

        public virtual InvItemDto Item { get; set; }
        public virtual InvPoMasterDto Po { get; set; }


        //dto
        public decimal SubTotal => RequestedQuantity * Rate;
    }
}
