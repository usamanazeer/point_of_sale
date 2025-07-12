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
        public double Quantity { get; set; }
        public double? PurchaseRate { get; set; }
        public double SalesRate { get; set; }
        public double? DiscountAmount { get; set; }
        public bool? IsDiscountInPercent { get; set; }
        public int? TaxId { get; set; }
        public double? TaxAmount { get; set; }
        public bool? IsTaxInPercent { get; set; }
        public double FinalSalesRate { get; set; }
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