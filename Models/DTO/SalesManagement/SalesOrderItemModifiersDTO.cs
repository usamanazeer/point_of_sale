using Models.DTO.InventoryManagement;
using System;

namespace Models.DTO.SalesManagement
{
    public class SalesOrderItemModifiersDto
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int? ModifierId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Charges { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual InvModifierDto Modifier { get; set; }
        public virtual SalesOrderDetailsDto OrderItem { get; set; }
    }
}
