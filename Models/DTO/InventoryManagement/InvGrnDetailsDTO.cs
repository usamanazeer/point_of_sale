namespace Models.DTO.InventoryManagement
{
    public class InvGrnDetailsDto : ExtendedBaseModel
    {
        public int GrnId { get; set; }
        public int? PoId { get; set; }
        //public string PoNo { get; set; }
        public int ItemId { get; set; }
        public string BatchNo { get; set; }
        //public decimal? OrderedQuantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal Rate { get; set; }
        public decimal SubTotal => (ReceivedQuantity) * Rate;

        public virtual InvGrnMasterDto Grn { get; set; }
        public virtual InvItemDto Item { get; set; }
        public virtual InvPoMasterDto Po { get; set; }
    }
}
