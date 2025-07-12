using System;

namespace Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory
{
    public class PhysicalInventoryViewFilter : BaseModel
    {
        public int[] BillIds { get; set; }
        public DateTime? BillDateStart { get; set; }
        public DateTime? BillDateEnd { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int[] ItemIds { get; set; }
        public int[] ItemBarCodeIds { get; set; }
        public int[] VendorIds { get; set; }
        public bool OnlyIfRemaining { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public bool? ItemDisplayOnPos { get; set; }
        public int[] ItemTypes { get; set; }
        public string SearchText { get; set; }
    }
}
