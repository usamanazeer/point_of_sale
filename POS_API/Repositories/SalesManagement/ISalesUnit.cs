//using Models.DTO.SalesManagement;
//using Models.DTO.ViewModels.SelectList.SalesManagement;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace POS_API.Repositories.SalesManagement
//{
//    internal interface ISalesUnit
//    {
//        #region Order
//        Task<SalesOrderMasterDto> PlaceOrder(SalesOrderMasterDto model);
//        [Obsolete]
//        Task<bool> DeleteOrders(SalesOrderMasterDto model);
//        Task<bool> ChangeOrderStatus(SalesOrderMasterDto model);
//        Task<SalesOrderMasterDto> EditOrder(SalesOrderMasterDto model);
//        Task<List<SalesOrderMasterDto>> GetAllOrders(SalesOrderMasterDto model);
//        Task<SalesOrderMasterDto> GetOrderDetails(SalesOrderMasterDto model);
//        Task<IList<SalesOrderMaster_SLM>> GetSelectList(SalesOrderMasterDto model);
//        Task<bool> IsOrderExist(SalesOrderMasterDto model);
//        Task<SalesOrderMasterDto> Checkout(SalesOrderMasterDto model);
//        Task<IList<SalesOrderStatus_SLM>> GetOrderStatusSelectList(SalesOrderStatusDto filter);
//        Task<bool> CancelOrder(SalesOrderMasterDto model);
//        #endregion
//    }
//}
