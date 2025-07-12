using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace POS_API.Data
{
    public partial class PosDB_Context
    {
        
        public async Task<float> Inv_GetRemaingInventory(int ItemId)
        {
            const string Item_Id = "@ItemId";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter(Item_Id, ItemId)
            };

            var queryString = $"dbo.Inv_GetRemaingInventory({Item_Id})";
            await this.Database.OpenConnectionAsync();

            var result = await this.Database.ExecuteSqlRawAsync(
                queryString, parameters
                );
            await this.Database.CloseConnectionAsync();
            return result;
        }
    }
}
