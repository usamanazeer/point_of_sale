namespace Models.DTO.InventoryManagement
{
    public class InvGrnDetailsDto : ExtendedBaseModel
    {
        public int GrnId { get; set; }
        public int? PoId { get; set; }
        //public string PoNo { get; set; }
        public int ItemId { get; set; }
        public string BatchNo { get; set; }
        //public double? OrderedQuantity { get; set; }
        public double ReceivedQuantity { get; set; }
        public double Rate { get; set; }
        public double SubTotal => (ReceivedQuantity) * Rate;

        public virtual InvGrnMasterDto Grn { get; set; }
        public virtual InvItemDto Item { get; set; }
        public virtual InvPoMasterDto Po { get; set; }
    }
}
