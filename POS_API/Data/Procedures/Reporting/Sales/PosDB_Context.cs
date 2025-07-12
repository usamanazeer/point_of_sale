using Microsoft.EntityFrameworkCore;
using Models.DTO.Reporting.Sales;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace POS_API.Data
{
    
    // ReSharper disable once InconsistentNaming
    public partial class PosDB_Context
    {

        
        public async Task<DataTable> Rpt_Sales_GetSalesReport(RptSalesSalesReportDto param) 
        {
            const string COMPANY_ID = "@CompanyId";
            const string ORDER_TYPE = "@OrderType";
            const string PAYMENT_TYPE = "@PaymentType";
            const string BRANCH_ID = "@BranchId";
            const string START_DATE = "@StartDate";
            const string END_DATE = "@EndDate";
            const string DATE_GROUP_BY_FILTER = "@DateGroupByFilter";
            var queryString = $"dbo.Rpt_Sales_GetSalesReport";
            var dt = new DataTable();
            var parameters = new List<SqlParameter>
                             {
                                 new SqlParameter(ORDER_TYPE,
                                                  param.OrderType),
                                 new SqlParameter(PAYMENT_TYPE,
                                                  param.PaymentType),
                                 new SqlParameter(COMPANY_ID,
                                                  param.CompanyId),
                                 new SqlParameter(BRANCH_ID,
                                                  param.BranchId),
                                 new SqlParameter(START_DATE,
                                                  param.StartDate),
                                 new SqlParameter(END_DATE,
                                                  param.EndDate),
                                 new SqlParameter(DATE_GROUP_BY_FILTER,
                                                  param.DateGroupByFilter)
                             };

            await using var con = new SqlConnection(Database.GetDbConnection().ConnectionString);
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = queryString;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters.ToArray());
                await con.OpenAsync();
                using var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                await con.CloseAsync();
                return dt;
            }
        }
        public async Task<DataTable> Rpt_Sales_GetSalesReport_ByDeliveryServices(RptSalesSalesReportDto param)
        {
            const string COMPANY_ID = "@CompanyId";
            const string BRANCH_ID = "@BranchId";
            const string START_DATE = "@StartDate";
            const string END_DATE = "@EndDate";
            const string DELIVERY_SERVICE_IDS = "@DeliveryServiceIds";
            const string DATE_GROUP_BY_FILTER = "@DateGroupByFilter";
            const string TOP_SALES_FILTER = "@TopSalesFilter";
            var queryString = $"dbo.Rpt_Sales_GetSalesAmountReport_ByDeliveryServices";
            var dt = new DataTable();
            var parameters = new List<SqlParameter>
                             {
                                 new SqlParameter(COMPANY_ID,
                                                  param.CompanyId),
                                 new SqlParameter(BRANCH_ID,
                                                  param.BranchId),
                                 new SqlParameter(START_DATE,
                                                  param.StartDate),
                                 new SqlParameter(END_DATE,
                                                  param.EndDate),
                                 new SqlParameter(TOP_SALES_FILTER,
                                                  param.TopSalesFilter),
                                 new SqlParameter(DELIVERY_SERVICE_IDS,
                                                  param.DeliveryServiceIds),
                                 new SqlParameter(DATE_GROUP_BY_FILTER,
                                                  param.DateGroupByFilter)
                             };

            await using var con = new SqlConnection(Database.GetDbConnection().ConnectionString);
            await using var cmd = new SqlCommand
                                  {
                                      Connection = con,
                                      CommandText = queryString,
                                      CommandType = CommandType.StoredProcedure
                                  };
            cmd.Parameters.AddRange(parameters.ToArray());
            await con.OpenAsync();
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            await con.CloseAsync();
            return dt;
        }
        public async Task<DataTable> Rpt_Sales_GetSalesAmountReport_ByItems(RptSalesSalesReportDto param)
        {
            const string COMPANY_ID = "@CompanyId";
            const string ORDER_TYPE = "@OrderType";
            const string BRANCH_ID = "@BranchId";
            const string START_DATE = "@StartDate";
            const string END_DATE = "@EndDate";
            const string DATE_GROUP_BY_FILTER = "@DateGroupByFilter";
            const string ITEM_IDS = "@ItemIds";
            const string WAITER_ID = "@WaiterId";
            const string TOP_SALES_FILTER = "@TopSalesFilter";
            var queryString = $"dbo.Rpt_Sales_GetSalesAmountReport_ByItems";
            var dt = new DataTable();
            var parameters = new List<SqlParameter>
                             {
                                 new SqlParameter(COMPANY_ID,
                                                  param.CompanyId),
                                 new SqlParameter(ORDER_TYPE,
                                                  param.OrderType),
                                 new SqlParameter(BRANCH_ID,
                                                  param.BranchId),
                                 new SqlParameter(START_DATE,
                                                  param.StartDate),
                                 new SqlParameter(END_DATE,
                                                  param.EndDate),
                                 new SqlParameter(DATE_GROUP_BY_FILTER,
                                                  param.DateGroupByFilter),
                                 new SqlParameter(ITEM_IDS,
                                                  param.ItemIds),
                                 new SqlParameter(WAITER_ID,
                                                  param.WaiterId),
                                 new SqlParameter(TOP_SALES_FILTER,
                                                  param.TopSalesFilter),
                             };

            await using var con = new SqlConnection(Database.GetDbConnection().ConnectionString);
            await using var cmd = new SqlCommand
                                  {
                                      Connection = con,
                                      CommandText = queryString,
                                      CommandType = CommandType.StoredProcedure
                                  };
            cmd.Parameters.AddRange(parameters.ToArray());
            await con.OpenAsync();
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            await con.CloseAsync();
            return dt;
        }
        public async Task<DataTable> Rpt_Sales_GetSalesQuantityReport_ByItems(RptSalesSalesReportDto param)
        {
            const string COMPANY_ID = "@CompanyId";
            const string ORDER_TYPE = "@OrderType";
            const string BRANCH_ID = "@BranchId";
            const string START_DATE = "@StartDate";
            const string END_DATE = "@EndDate";
            const string DATE_GROUP_BY_FILTER = "@DateGroupByFilter";
            const string ITEM_IDS = "@ItemIds";
            const string TOP_SALES_FILTER = "@TopSalesFilter";
            var queryString = $"dbo.Rpt_Sales_GetSalesQuantityReport_ByItems";
            var dt = new DataTable();
            var parameters = new List<SqlParameter>
                             {
                                 new SqlParameter(COMPANY_ID,
                                                  param.CompanyId),
                                 new SqlParameter(ORDER_TYPE,
                                                  param.OrderType),
                                 new SqlParameter(BRANCH_ID,
                                                  param.BranchId),
                                 new SqlParameter(START_DATE,
                                                  param.StartDate),
                                 new SqlParameter(END_DATE,
                                                  param.EndDate),
                                 new SqlParameter(DATE_GROUP_BY_FILTER,
                                                  param.DateGroupByFilter),
                                 new SqlParameter(ITEM_IDS,
                                                  param.ItemIds),
                                 new SqlParameter(TOP_SALES_FILTER,
                                                  param.TopSalesFilter)
                             };

            await using var con = new SqlConnection(Database.GetDbConnection().ConnectionString);
            await using var cmd = new SqlCommand
                                  {
                                      Connection = con,
                                      CommandText = queryString,
                                      CommandType = CommandType.StoredProcedure
                                  };
            cmd.Parameters.AddRange(parameters.ToArray());
            await con.OpenAsync();
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            await con.CloseAsync();
            return dt;
        }
    }
}
