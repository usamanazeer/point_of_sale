using System;
using System.Collections.Generic;

namespace Models.DTO.Reporting.Sales
{
    public class RptSalesSalesReportDto
    {
        public RptSalesSalesReportDto()
        {
            ChartType = "bar";
            ReportId = "sales-report" + (new Random()).Next(0, 1000);
        }
        public int CompanyId { get; set; }
        public int? BranchId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? OrderType { get; set; }
        public int? PaymentType { get; set; }
        public string DateGroupByFilter { get; set; }
        public int? TopSalesFilter { get; set; }
        public bool? ReportQuantityValue { get; set; }
        public string ItemIds { get; set; }
        public int? WaiterId { get; set; }
        public string WaiterName { get; set; }
        public string DeliveryServiceIds { get; set; }
        public string ReportId { get; set; }
        public string ReportTitle { get; set; }
        public string ChartType { get; set; }
        public List<string> Labels { get; set; }
        public List<List<RptSalesSalesReportRowDto>> SalesDataList { get; set; }
    }
}
