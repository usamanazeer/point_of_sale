//using Models.DTO.SalesManagement;
//using Models.DTO.ViewModels.SelectList.SalesManagement;
//using POS_API.Repositories.SalesManagement.OrderRepos;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace POS_API.Repositories.SalesManagement
//{
//    internal class SalesUnit:ISalesUnit
//    {
//        private readonly IOrderRepository _orderRepository;
//        public SalesUnit(IOrderRepository orderRepository)
//        {
//            _orderRepository = orderRepository;
//        }

//        #region Order
//        public async Task<bool> ChangeOrderStatus(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.ChangeOrderStatus(model);
//        }

//        public async Task<SalesOrderMasterDto> PlaceOrder(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.PlaceOrder(model);
//        }

//        [Obsolete]
//        public async Task<bool> DeleteOrders(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.Delete(model);
//        }

//        public async Task<SalesOrderMasterDto> EditOrder(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.Edit(model);
//        }

//        public async Task<List<SalesOrderMasterDto>> GetAllOrders(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.GetAll(model);
//        }

//        public async Task<SalesOrderMasterDto> GetOrderDetails(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.GetDetails(model);
//        }

//        public async Task<IList<SalesOrderMaster_SLM>> GetSelectList(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.GetSelectList(model);
//        }

//        public async Task<bool> IsOrderExist(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.IsExist(model);
//        }

//        public async Task<SalesOrderMasterDto> Checkout(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.Checkout(model);
//        }

//        public async Task<IList<SalesOrderStatus_SLM>> GetOrderStatusSelectList(SalesOrderStatusDto filter)
//        {
//            return await _orderRepository.GetOrderStatusSelectList(filter);
//        }

//        public async Task<bool> CancelOrder(SalesOrderMasterDto model)
//        {
//            return await _orderRepository.CancelOrder(model);
//        }

//        #endregion
//    }
//}
