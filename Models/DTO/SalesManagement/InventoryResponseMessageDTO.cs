namespace Models.DTO.SalesManagement
{
    public class InventoryResponseMessageDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int? BarCodeId { get; set; }
        public float RequestedQuantity { get; set; }
        //public float AvailableQuantity { get; set; }

    }
}
