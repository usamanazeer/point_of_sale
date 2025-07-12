using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Models.DTO.Accounts;
using POS_API.Data.TVPs;
using POS_API.Utilities.Mapper;

namespace POS_API.Data
{
    public partial class PosDB_Context
    {
        public async Task<int> Noti_SaveNotification(int TypeId, string Title, string Message, string Url, int? referenceKey, int CompanyId, int CreatedBy, DateTime CreatedOn) 
        {
            const string TYPE_ID = "@TypeId";
            const string TITLE = "@Title";
            const string MESASGE = "@Message";
            const string URL = "@Url";
            const string REFERENCE_KEY = "@ReferenceKey";
            const string COMPANY_ID = "@CompanyId";
            const string CREATED_BY = "@CreatedBy";
            const string CREATED_ON = "@CreatedOn";

            var parameters = new List<SqlParameter>
                             {
                                 new SqlParameter(TYPE_ID,
                                                  TypeId),
                                 new SqlParameter(TITLE,
                                                  Title),
                                 new SqlParameter(MESASGE,
                                                  Message),
                                 new SqlParameter(URL,
                                                  Url),
                                 new SqlParameter(REFERENCE_KEY,
                                                  (object) referenceKey ?? (object) DBNull.Value),
                                 new SqlParameter(COMPANY_ID,
                                                  CompanyId),
                                 new SqlParameter(CREATED_BY,
                                                  CreatedBy),
                                 new SqlParameter(CREATED_ON,
                                                  CreatedOn)
                             };

            var queryString = $"Noti_SaveNotification " +
                $"{TYPE_ID}, " +
                $"{TITLE}, " +
                $"{MESASGE}, " +
                $"{URL}, " +
                $"{REFERENCE_KEY}, " +
                $"{COMPANY_ID}, " +
                $"{CREATED_BY}, " +
                $"{CREATED_ON}";
            await Database.OpenConnectionAsync();
            
            var result =  await Database.ExecuteSqlRawAsync(
                queryString, parameters
                );
            await Database.CloseConnectionAsync();
            return result;
        }

        public async Task <DataTable> Sales_ConsumeInventory(DataTable orderItems, int OrderId)
        {
            try
            {
                const string Order_Items = "@OrderItems";
                const string Order_Id = "@OrderId";
                //const string Order_Items_Modifiers = "@OrderItemModifiers";
                string queryString = $"dbo.Sales_ConsumeInventory";
                var dt = new DataTable();
                var orderItemsP = new SqlParameter(Order_Items,
                                                   orderItems)
                                  {
                                      SqlDbType = SqlDbType.Structured,
                                      TypeName = "dbo.OrderItem"
                                  };
                var orderIdP = new SqlParameter(Order_Id, OrderId);


                //var orderItemsModifiersP = new SqlParameter(Order_Items_Modifiers, orderItemsModifiers);
                //orderItemsModifiersP.SqlDbType = SqlDbType.Structured;
                //orderItemsModifiersP.TypeName = "dbo.OrderItemModifier";
                var parameters = new List<SqlParameter>
                                 {
                                     orderItemsP,
                                     orderIdP
                                 };
                //parameters.Add(orderItemsModifiersP);

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Sales_RevertConsumedInventory_ByOrder(int orderId/*, bool revertOnlyReturnable*/)
        {
            try
            {
                const string Order_Id = "@OrderId";
                //const string RevertOnlyReturnable = "@RevertOnlyReturnable";
                string queryString = $"dbo.Sales_RevertConsumedInventory_ByOrder";
                var orderIdP = new SqlParameter(Order_Id, orderId);
                //var RevertOnlyReturnableP = new SqlParameter(RevertOnlyReturnable, revertOnlyReturnable);
                var parameters = new List<SqlParameter>
                                 {
                                     orderIdP
                                 };
                //parameters.Add(RevertOnlyReturnableP);
                await using var con = new SqlConnection(Database.GetDbConnection().ConnectionString);
                await using var cmd = new SqlCommand
                                      {
                                          Connection = con,
                                          CommandText = queryString,
                                          CommandType = CommandType.StoredProcedure
                                      };
                cmd.Parameters.AddRange(parameters.ToArray());
                con.Open();
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Sales_CancelOrder(int orderId, int cancelledBy)
        {
            try
            {
                const string Order_Id = "@OrderId";
                const string CancelledBy = "@CancelledBy";
                string queryString = $"dbo.Sales_CancelOrder";
                var orderIdP = new SqlParameter(Order_Id, orderId);
                var cancelledByP = new SqlParameter(CancelledBy, cancelledBy);
                var parameters = new List<SqlParameter>
                                 {
                                     orderIdP,
                                     cancelledByP
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
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataSet> Sales_GetConsumableInventory(DataTable orderItems, DataTable orderItemsModifiers)
        {
            try
            {
                // ReSharper disable once InconsistentNaming
                const string Order_Items = "@OrderItems";
                // ReSharper disable once InconsistentNaming
                const string Order_Items_Modifiers = "@OrderItemModifiers";
                string queryString = $"dbo.Sales_GetConsumableInventory";
                var ds = new DataSet();
                var orderItemsP = new SqlParameter(Order_Items,
                                                   orderItems)
                                  {
                                      SqlDbType = SqlDbType.Structured,
                                      TypeName = "dbo.OrderItem"
                                  };
                var orderItemsModifiersP = new SqlParameter(Order_Items_Modifiers,
                                                            orderItemsModifiers)
                                           {
                                               SqlDbType = SqlDbType.Structured,
                                               TypeName = "dbo.OrderItemModifier"
                                           };
                var parameters = new List<SqlParameter>
                                 {
                                     orderItemsP,
                                     orderItemsModifiersP
                                 };

                await using var con = new SqlConnection(Database.GetDbConnection().ConnectionString);
                await using var cmd = new SqlCommand
                                {
                                    Connection = con,
                                    CommandText = queryString,
                                    CommandType = CommandType.StoredProcedure
                                };
                cmd.Parameters.AddRange(parameters.ToArray());

                using var adapter = new SqlDataAdapter(cmd);
                await con.OpenAsync();
                adapter.Fill(ds);
                await con.CloseAsync();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Acc_PayVendorBill(AccBillPaymentDto billPaymentDto)
        {
            try
            {
                var queryString = $"dbo.Acc_PayVendorBill";
                var parameters = new List<SqlParameter>
                                 {
                                     new SqlParameter("@BillId", billPaymentDto.BillId),
                                     new SqlParameter("@PaymentTypeId", billPaymentDto.PaymentTypeId),
                                     new SqlParameter("@CashAmount", billPaymentDto.@CashAmount),
                                     new SqlParameter("@ChequeAmount", billPaymentDto.ChequeAmount),
                                     new SqlParameter("@BankAccountId", billPaymentDto.BankAccountId),
                                     new SqlParameter("@ChequeNo", billPaymentDto.ChequeNo),
                                     new SqlParameter("@Remarks", billPaymentDto.Remarks),
                                     new SqlParameter("@PaymentDate", billPaymentDto.PaymentDate),
                                     new SqlParameter("@CreatedBy", billPaymentDto.CreatedBy),
                                 };
                //parameters.Add(RevertOnlyReturnableP);
                await using var con = new SqlConnection(Database.GetDbConnection().ConnectionString);
                await using var cmd = new SqlCommand
                                      {
                                          Connection = con,
                                          CommandText = queryString,
                                          CommandType = CommandType.StoredProcedure
                                      };
                cmd.Parameters.AddRange(parameters.ToArray());
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataTable> Inv_ItemsBulkImport(List<BulkUploadItemsTvp> itemsData,int companyId, int createdBy)
        {
            const string ITEMS_DATA = "@ItemsData";
            const string COMPANY_ID = "@CompanyId";
            const string CREATED_BY = "@CreatedBy";
            var dt = itemsData.ToDataTable();
            var parameters = new SqlParameter[3];
            var itemsDataParameter = new SqlParameter(ITEMS_DATA,
                                                      dt)
                                     {
                                         SqlDbType = SqlDbType.Structured,
                                         TypeName = "dbo.BulkUploadItems"
                                     };
            parameters[0] = itemsDataParameter;
            parameters[1] = new SqlParameter(COMPANY_ID, companyId);
            parameters[2] = new SqlParameter(CREATED_BY, createdBy);

            var queryString = $"dbo.Inv_ItemsBulkImport";
            var responseTable = new DataTable();
            await using var con = new SqlConnection(Database.GetDbConnection().ConnectionString);
            await using var cmd = new SqlCommand
                                  {
                                      Connection = con,
                                      CommandText = queryString,
                                      CommandType = CommandType.StoredProcedure
                                  };
            cmd.Parameters.AddRange(parameters);
            await con.OpenAsync();
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(responseTable);
            await con.CloseAsync();
            return responseTable;
        }
    }
}
