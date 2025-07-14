using Models.DTO.InventoryManagement;
using System;
using System.Collections.Generic;

namespace Models.DTO.SalesManagement
{
    public sealed class SalesOrderDetailsDto
    {
        public SalesOrderDetailsDto()
        {
            SalesOrderItemModifiers = new List<SalesOrderItemModifiersDto>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal SalesRate { get; set; }
        public decimal? DiscountAmount { get; set; }
        public bool? IsDiscountInPercent { get; set; }
        public int? TaxId { get; set; }
        public decimal? TaxAmount { get; set; }
        public bool? IsTaxInPercent { get; set; }
        public decimal FinalSalesRate { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public InvItemDto Item { get; set; }
        public SalesOrderMasterDto Order { get; set; }

        public IList<SalesOrderItemModifiersDto> SalesOrderItemModifiers { get; set; }
    }
}