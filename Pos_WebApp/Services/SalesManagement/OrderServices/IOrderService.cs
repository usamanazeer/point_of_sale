using Models;
using Models.DTO.SalesManagement;
using Models.DTO.ViewModels.SelectList.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Pos_WebApp.Services.SalesManagement.OrderServices 
{ 
    public interface IOrderService 
    {
        Task<SalesOrderMasterDto> Get(string token, SalesOrderMasterDto model = null);
        Task<Response> GetResponse(string token, SalesOrderMasterDto model = null);
        Task<SalesOrderMasterDto> PlaceOrder(string token, SalesOrderMasterDto model);
        Task<Response> PlaceOrderResponse(string token, SalesOrderMasterDto model);
        Task<SalesOrderMasterDto> UpdateOrder(string token, SalesOrderMasterDto model);
        Task<Response> UpdateOrderResponse(string token, SalesOrderMasterDto model);
        Task<SalesOrderMasterDto> Details(string token, int id);
        Task<Response> DetailsResponse(string token, int id);
        Task<IList<SalesOrderMaster_SLM>> GetSelectList(string tOKEN, SalesOrderMasterDto model = null);
        Task<Response> GetSelectListResponse(string tOKEN, SalesOrderMasterDto model = null);
        [Obsolete]
        Task<Response> Delete(string token, int id);
        Task<Response> ChangeOrderStatus(string token, int id, int status);
        Task<Response> CancelOrder(string token, int id);
        Task<Response> Checkout(string token, SalesOrderMasterDto model);
        Task<Response> PrintReceipt(string tOKEN, int orderId);
        Task<IList<SalesOrderStatus_SLM>> GetOrderStatusSelectList(string tOKEN, SalesOrderStatusDto filter = null);
        Task<Response> GetOrderStatusSelectListResponse(string tOKEN, SalesOrderStatusDto filter = null);
    } 
}