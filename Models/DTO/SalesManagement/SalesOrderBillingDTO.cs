using Models.DTO.GeneralSettings;
using System;
using Models.Enums;

namespace Models.DTO.SalesManagement
{
    public class SalesOrderBillingDto : BaseModel
    {
        public SalesOrderBillingDto(SalesOrderMasterDto order = null)
        {
            if (order != null)
            {
                MapOrder(order);
            }
        }
        public SalesOrderBillingDto()
        {

        }
        public int? OrderId { get; set; }
        public int? TaxId { get; set; }
        public decimal? TaxAmount { get; set; }
        public bool? IsTaxInPercent { get; set; }
        public decimal? TotalBillAmount { get; set; }
        public decimal? TotalAmountPaid { get; set; }
        public int? PaymentType { get; set; }
        public decimal? CashReceived { get; set; }
        public decimal? CashReturn { get; set; }

        public virtual SalesOrderMasterDto Order { get; set; }
        public virtual TaxDto Tax { get; set; }
        private void MapOrder(SalesOrderMasterDto model)
        {
            OrderId = model.Id;
            BranchId = model.BranchId;
            CompanyId = model.CompanyId;
            TaxId = model.TaxId;
            TaxAmount = model.TaxAmount;
            IsTaxInPercent = model.IsTaxInPercent;
            CompanyId = model.CompanyId;
            TotalBillAmount = model.GetOrderAmountPayable();
            CreatedOn = DateTime.Now;
            TotalAmountPaid = TotalBillAmount;
            CashReceived = TotalBillAmount;
            CashReturn = 0;
            CreatedBy = model.CreatedBy;
            CompanyId = model.CompanyId;
            BranchId = model.BranchId;
            
            if (model.SalesOrderBilling != null)
            {
                PaymentType = model.SalesOrderBilling.PaymentType;
                if (model.OrderTypeId != OrderTypes.Delivery.ToInt() && PaymentType != PaymentMode.Card.ToInt())
                {
                    CashReceived = model.SalesOrderBilling.CashReceived;
                    CashReturn = ((model.SalesOrderBilling.CashReceived ?? 0) - (TotalBillAmount ?? 0)).ToNDecimalPlaces(2);
                }
            }
        }
    }
}
