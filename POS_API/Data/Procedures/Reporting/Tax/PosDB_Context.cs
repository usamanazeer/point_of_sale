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
        public async Task<DataTable> Rpt_Tax_GetTaxReport(RptTaxCollectionDto rptTaxCollectionDto) 
        {
            const string COMPANY_ID = "@CompanyId";
            const string BRANCH_ID = "@BranchId";
            const string START_DATE = "@StartDate";
            const string END_DATE = "@EndDate";
            var queryString = $"dbo.Rpt_Tax_GetTaxReport";
            var dt = new DataTable();
            var parameters = new List<SqlParameter>
                             {
                                 new SqlParameter(COMPANY_ID,
                                                  rptTaxCollectionDto.CompanyId),
                                 new SqlParameter(BRANCH_ID,
                                                  rptTaxCollectionDto.BranchId),
                                 new SqlParameter(START_DATE,
                                                  rptTaxCollectionDto.FromDate),
                                 new SqlParameter(END_DATE,
                                                  rptTaxCollectionDto.ToDate)
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
