using Microsoft.EntityFrameworkCore;
using System;
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

        
        public async Task<DataTable> Rpt_Acc_TrialBalanceReport(DateTime onDate, int companyId) 
        {
            const string COMPANY_ID = "@CompanyId";
            const string ON_DATE = "@OnDate";
            var queryString = $"dbo.Rpt_Acc_TrialBalanceReport";
            var dt = new DataTable();
            var parameters = new List<SqlParameter>
                             {
                                 new SqlParameter(COMPANY_ID,
                                                  companyId),
                                 new SqlParameter(ON_DATE,
                                                  onDate)
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
        public async Task<DataTable> Rpt_Acc_IncomeStatementReport(DateTime fromDate, DateTime toDate, int companyId)
        {
            const string COMPANY_ID = "@CompanyId";
            const string FROM_DATE = "@FromDate";
            const string TO_DATE = "@ToDate";
            var queryString = $"dbo.Rpt_Acc_IncomeStatementReport";
            var dt = new DataTable();
            var parameters = new List<SqlParameter>
                             {
                                 new SqlParameter(COMPANY_ID,
                                                  companyId),
                                 new SqlParameter(FROM_DATE,
                                                  fromDate),
                                 new SqlParameter(TO_DATE,
                                                  toDate)
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
