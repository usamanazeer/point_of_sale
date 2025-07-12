using System;

namespace Models.DTO.InventoryManagement.ViewDTO
{
    public class InvLatestItemBarCodeViewDto
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public int? ItemId { get; set; }
        public int? CompanyId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
