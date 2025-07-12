using Models;
using Models.DTO.SalesManagement;
using System;
using System.Threading.Tasks;

namespace POS_API.Services.SalesManagement.OrderServices
{
    public interface IOrderService
    {
        Task<Response> GetAll(SalesOrderMasterDto model);
        Task<Response> PlaceOrder(SalesOrderMasterDto model);
        Task<Response> Edit(SalesOrderMasterDto model);
        [Obsolete]
        Task<bool> Delete(SalesOrderMasterDto model);
        Task<Response> Checkout(SalesOrderMasterDto model);
        Task<bool> ChangeOrderStatus(SalesOrderMasterDto model);
        Task<Response> GetDetails(SalesOrderMasterDto model);
        Task<Response> GetSelectList(SalesOrderMasterDto model);
        Task<bool> IsExist(SalesOrderMasterDto model);
        Task<bool> PrintReceipt(SalesOrderMasterDto model);
        Task<Response> GetOrderStatusSelectList(SalesOrderStatusDto filter);
        Task<bool> CancelOrder(SalesOrderMasterDto model);
    }
}
