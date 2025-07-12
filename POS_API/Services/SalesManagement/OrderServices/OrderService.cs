using System;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO.SalesManagement;
using Models.Enums;
using POS_API.Repositories.SalesManagement.OrderRepos;
using POS_API.Services.SalesManagement.OrderServices.OrderReceiptServices;

namespace POS_API.Services.SalesManagement.OrderServices
{
    internal class OrderService : IOrderService, IService
    {
        private readonly IOrderReceiptService _orderReceiptService;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository,
                            IOrderReceiptService orderReceiptService)
        {
            _orderRepository = orderRepository;
            _orderReceiptService = orderReceiptService;
        }

        public async Task<bool> ChangeOrderStatus(SalesOrderMasterDto model) => await _orderRepository.ChangeOrderStatus(model: model);

        public async Task<Response> PlaceOrder(SalesOrderMasterDto model)
        {
            var response = new Response();
            var isExists = await IsExist(model: model);
            if (!isExists)  
            {
                model.SalesOrderDetails = model.SalesOrderDetails.Select(selector: i =>
                {
                    i.CompanyId =  model.CompanyId;
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    i.SalesOrderItemModifiers.Select(m =>
                    {
                        m.CompanyId = i.CompanyId;
                        return m;
                    });
                    return i;
                }).ToList();
                var res = await _orderRepository.PlaceOrder(model: model);
                if (res.Response != null && res.Response.ErrorCode == StatusCodes.Quantity_Not_Available.ToInt())
                {
                    response.ErrorCode = res.Response.ErrorCode;
                    response.ErrorMessage = res.Response.ErrorMessage;
                }
                else
                {
                    response.ResponseCode = StatusCodes.Created.ToInt();
                    response.ResponseMessage = "Order Placed Successfully.";
                    //print receipt
                    await _orderReceiptService.PrintOrder(salesOrderMasterDto: res);
                    if (res.OrderTypeId != OrderTypes.DineIn.ToInt() && res.OrderTypeId != OrderTypes.Delivery.ToInt())
                        await _orderReceiptService.PrintSalesReceipt(salesOrderMasterDto: res);
                }

                response.Model = res;
                return response;
            }
            response.Model = model;
            response.ErrorCode = StatusCodes.Error_Occured.ToInt();
            response.ErrorMessage = "Order Already Exists.";
            return response;
        }


        public async Task<Response> Edit(SalesOrderMasterDto model)
        {
            var response = new Response();
            var isExists = await IsExist(model: model);
            if (!isExists)
            {
                model.SalesOrderDetails = model.SalesOrderDetails.Select(selector: i =>
                {
                    i.CompanyId = model.CompanyId;
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    i.SalesOrderItemModifiers.Select(selector: m =>
                    {
                        m.CompanyId = i.CompanyId;
                        return m;
                    });
                    return i;
                }).ToList();
                var res = await _orderRepository.Edit(model: model);

                if (res != null)
                {
                    if (res.Response != null && res.Response.ErrorCode == StatusCodes.Quantity_Not_Available.ToInt())
                    {
                        response.ErrorCode = res.Response.ErrorCode;
                        response.ErrorMessage = res.Response.ErrorMessage;
                    }
                    else
                    {
                        response.ResponseCode = StatusCodes.Created.ToInt();
                        response.ResponseMessage = "Order Updated Successfully.";
                    }
                }
                else
                {
                    response.ResponseCode = StatusCodes.Not_Found.ToInt();
                    response.ResponseMessage = "Order Not Found.";
                }

                response.Model = res;
            }
            else
            {
                response.Model = model;
                response.ErrorCode = StatusCodes.Error_Occured.ToInt();
                response.ErrorMessage = "Order Already Exists.";
            }
            return response;
        }


        [Obsolete]
        public async Task<bool> Delete(SalesOrderMasterDto model) => await _orderRepository.Delete(model: model);


        public async Task<Response> GetAll(SalesOrderMasterDto model)
        {
            var response = new Response();
            var res = await _orderRepository.GetAll(model: model);
            if (res.Any())
            {
                response.Model = res;
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "No Order Found.";
            }
            return response;
        }


        public async Task<Response> GetDetails(SalesOrderMasterDto model)
        {
            var response = new Response();
            var res = await _orderRepository.GetDetails(model: model);
            if (res != null)
            {
                response.Model = res;
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "Order Not Found";
            }
            return response;
        }

        public async Task<Response> GetSelectList(SalesOrderMasterDto model)
        {
            var response = new Response();
            var itemsList = await _orderRepository.GetSelectList(model: model);
            if (itemsList != null)
            {
                response.Model = itemsList;
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "Orders Not Found.";
            }

            return response;
        }

        public async Task<bool> IsExist(SalesOrderMasterDto model) => await _orderRepository.IsExist(model: model);

        public async Task<Response> Checkout(SalesOrderMasterDto model)
        {
            var response = new Response();
            var res = await _orderRepository.Checkout(model: model);

            if (res != null)
            {
                response.Model = res;
                response.ResponseCode = StatusCodes.OK.ToInt();
                response.ResponseMessage = "CheckedOut Successfully";
                //print receipt
                if (res.OrderTypeId == OrderTypes.DineIn.ToInt())
                    await _orderReceiptService.PrintSalesReceipt(salesOrderMasterDto: res);
            }
            else
            {
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "Order Not Found";
            }

            return response;
        }


        public async Task<bool> PrintReceipt(SalesOrderMasterDto model)
        {
            var data = await _orderRepository.GetDetails(model: model);
            return await _orderReceiptService.PrintSalesReceipt(salesOrderMasterDto: data);
        }

        public async Task<Response> GetOrderStatusSelectList(SalesOrderStatusDto filter)
        {
            var response = new Response();
            var itemsList = await _orderRepository.GetOrderStatusSelectList(filter: filter);
            if (itemsList != null)
            {
                response.Model = itemsList;
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "OrdersStatus List Not Found.";
            }

            return response;
        }

        public async Task<bool> CancelOrder(SalesOrderMasterDto model) => await _orderRepository.CancelOrder(model: model);
    }
}