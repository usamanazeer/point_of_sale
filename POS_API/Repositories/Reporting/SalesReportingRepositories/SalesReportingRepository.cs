using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Reporting.Sales;
using Models.Enums;
using POS_API.Data;

namespace POS_API.Repositories.Reporting.SalesReportingRepositories
{
    public class SalesReportingRepository : RepositoryBase, ISalesReportingRepository, IRepository
    {
        public SalesReportingRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext: dbContext, mapper: mapper)
        {
        }


        public async Task<List<RptSalesSalesReportRowDto>> GetItemSales(RptSalesSalesReportDto filters)
        {
            var salesReportDataTable = await _dbContext.Rpt_Sales_GetSalesReport(filters).ConfigureAwait(continueOnCapturedContext: true);
            return DataTableToTSalesSalesReportRowList(salesReportDataTable);
        }


        public async Task<List<RptSalesSalesReportRowDto>> GetItemSales_ByItems(RptSalesSalesReportDto filters)
        {
            if (!(filters.ReportQuantityValue ?? false))
            {
                var salesReportDataTable = await _dbContext.Rpt_Sales_GetSalesAmountReport_ByItems(param: filters)
                                                 .ConfigureAwait(continueOnCapturedContext: true);
                return DataTableToTSalesSalesReportRowList(salesReportDataTable);
            }
            else
            {
                var salesReportDataTable = await _dbContext.Rpt_Sales_GetSalesQuantityReport_ByItems(param: filters).ConfigureAwait(continueOnCapturedContext: true);
                return DataTableToTSalesSalesReportRowList(salesReportDataTable);
            }
        }


        public async Task<decimal> GetSalesAmount(RptSalesSalesReportDto filters)
        {
            
            var query = _dbContext.SalesOrderBilling.AsNoTracking().Include(navigationPropertyPath: x => x.Order)
                .Where(predicate: x => x.CreatedOn >= filters.StartDate.Date && x.CreatedOn <= filters.EndDate &&
                x.Status == StatusTypes.Active.ToInt() &&
                x.CompanyId == filters.CompanyId &&
                x.Order.OrderStatusId == OrderStatus.Billed.ToInt());
            if (filters.OrderType.HasValue) query = query.Where(predicate: x => x.Order.OrderTypeId == filters.OrderType);
            if (filters.PaymentType.HasValue) query = query.Where(predicate: x => x.PaymentType == filters.PaymentType);
            return await query.Select(selector: x => x.TotalBillAmount ?? 0).SumAsync(selector: x => x);
        }
        public async Task<List<RptSalesSalesReportRowDto>> GetSales_ByDeliveryServices(RptSalesSalesReportDto filters)
        {
            var salesReportDataTable = await _dbContext.Rpt_Sales_GetSalesReport_ByDeliveryServices(filters).ConfigureAwait(continueOnCapturedContext: true);
            return DataTableToTSalesSalesReportRowList(salesReportDataTable);
        }
        private List<RptSalesSalesReportRowDto> DataTableToTSalesSalesReportRowList(DataTable dt)
        {
            DataColumnCollection cols = dt.Columns;
            return (from DataRow dr in dt.Rows
                    select new RptSalesSalesReportRowDto
                    {
                        ItemId = cols.Contains("ItemId") ? Convert.ToInt32(dr["ItemId"]) :0 ,
                        ItemName = cols.Contains("ItemName") ? Convert.ToString(dr["ItemName"]): null,
                        TotalSales = cols.Contains("TotalSales") ? Convert.ToDecimal(dr["TotalSales"]):0,
                        TotalQuantity = cols.Contains("TotalQuantity") ? Convert.ToDecimal(dr["TotalQuantity"]):0,
                        SalesDate = Convert.ToDateTime(dr["SalesDate"]),
                        FormattedDate = cols.Contains("FormattedDate") ? Convert.ToString(dr["FormattedDate"]):"",
                    }).ToList();
        }
    }
}