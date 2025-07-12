using System;
using System.Collections.Generic;

namespace Models.DTO.Reporting.Sales
{
    public class RptSalesSalesReportRowDto
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public double Quantity { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double TotalSales { get; set; }
        public double TotalSalesRounded => TotalSales.ToNDecimalPlaces(2);
        public double TotalQuantity { get; set; }
        public double TotalQuantityRounded => TotalQuantity.ToNDecimalPlaces(2);
        public DateTime? SalesDate { get; set; }
        public string FormattedDate { get; set; }
        //public int SalesYear { get; set; }
        //public int SalesMonth { get; set; }
        //public string MonthName { get; set; }
        //public int SalesDay { get; set; }
        //public string DayName { get; set; }
        //public string Label { get; set; }

        public RptSalesSalesReportDto ReportParameters { get; set; }
        public IList<RptSalesSalesReportRowDto> SalesDataList { get; set; }
        public Response Response { get; set; }
    }
}
