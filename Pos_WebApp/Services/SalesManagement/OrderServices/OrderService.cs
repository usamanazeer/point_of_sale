using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.SalesManagement;
using Models.DTO.ViewModels.SelectList.SalesManagement;
using Models.Enums;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services.SalesManagement.OrderServices
{
    // ReSharper disable once InconsistentNaming
    internal class OrderService : ServiceBase, IOrderService, IService
    {
        public OrderService(IClientManager clientManager) : base("api/order/", clientManager)
        {}


        public async Task<Response> PlaceOrderResponse(string token, SalesOrderMasterDto model)
        {
            var res = await Client.Post<Response>(url: $"{Route}PlaceOrder", obj: model, token: token);
            var resModel = JsonConvert.DeserializeObject<SalesOrderMasterDto>(value: res.Model.String()) ?? new SalesOrderMasterDto();
            resModel.Response ??= new Response();
            if (resModel.Response != null)
                resModel.Response.Model = JsonConvert.DeserializeObject<IList<InventoryResponseMessageDto>>(value: resModel.Response.Model.String());
            res.Model = resModel;
            return res;
        }


        public async Task<SalesOrderMasterDto> PlaceOrder(string token, SalesOrderMasterDto model)
        {
            var res = await PlaceOrderResponse(token: token, model: model);
            if (res.Model != null) 
                model = (SalesOrderMasterDto) res.Model;
            model.Response = res;
            return model;
        }


        public async Task<Response> UpdateOrderResponse(string token, SalesOrderMasterDto model)
        {
            var res = await Client.Post<Response>(url: $"{Route}Edit", obj: model, token: token);
            var resModel = JsonConvert.DeserializeObject<SalesOrderMasterDto>(value: res.Model.String());
            if (resModel.Response != null)
                resModel.Response.Model = JsonConvert.DeserializeObject<IList<InventoryResponseMessageDto>>(value: resModel.Response.Model.String());
            res.Model = resModel;
            return res;
        }


        public async Task<SalesOrderMasterDto> UpdateOrder(string token, SalesOrderMasterDto model)
        {
            var res = await UpdateOrderResponse(token: token,  model: model);
            if (res.Model != null) 
                model = (SalesOrderMasterDto) res.Model;
            model.Response = res;
            return model;
        }

        [Obsolete]
        public async Task<Response> Delete(string token, int id)
        {
            var response = await Client.Get<Response>(url: $"{Route}Delete/{id}", token: token);
            response.Model = (bool) response.Model;
            return response;
        }

        public async Task<Response> ChangeOrderStatus(string token, int id, int status)
        {
            var response = await Client.Get<Response>(url: $"{Route}ChangeOrderStatus?id={id}&status={status}", token: token);
            response.Model = (bool) response.Model;
            return response;
        }

        public async Task<Response> CancelOrder(string token, int id)
        {
            var response = await Client.Get<Response>(url: $"{Route}CancelOrder?id={id}", token: token);
            response.Model = (bool) response.Model;
            return response;
        }

        public async Task<Response> DetailsResponse(string token, int id)
        {
            var response = await Client.Get<Response>(url: $"{Route}Details?id={id}", token: token);
            if (response.Model != null)
                response.Model = JsonConvert.DeserializeObject<SalesOrderMasterDto>(value: response.Model.String());
            return response;
        }

        public async Task<SalesOrderMasterDto> Details(string token,
                                                       int id)
        {
            var model = new SalesOrderMasterDto();
            var response = await DetailsResponse(token: token, id: id);
            if (response.Model != null) model = (SalesOrderMasterDto) response.Model;
            model.Response = response;
            return model;
        }

        public async Task<SalesOrderMasterDto> Get(string token,
                                                   SalesOrderMasterDto model = null)
        {
            var res = await GetResponse(token: token,
                                        model: model);
            model ??= new SalesOrderMasterDto();
            model.Response = res;
            model.Orders = res.Model != null ?  (List<SalesOrderMasterDto>) res.Model : new List<SalesOrderMasterDto>();
            return model;
        }

        public async Task<Response> GetResponse(string token, SalesOrderMasterDto model = null)
        {
            var res = await Client.Post<Response>(url: $"{Route}Get", obj: model ?? new SalesOrderMasterDto(), token: token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<List<SalesOrderMasterDto>>(value: res.Model.String());
            return res;
        }

        public async Task<IList<SalesOrderMaster_SLM>> GetSelectList(string token, SalesOrderMasterDto model = null)
        {
            var res = await GetSelectListResponse(token: token, model: model);
            return res.Model != null ? (IList<SalesOrderMaster_SLM>) res.Model : new List<SalesOrderMaster_SLM>();
        }

        public async Task<Response> GetSelectListResponse(string token, SalesOrderMasterDto model = null)
        {
            var res = await Client.Post<Response>(url: $"{Route}GetSelectList", obj: model ?? new SalesOrderMasterDto(), token: token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<SalesOrderMaster_SLM>>(value: res.Model.String());
            return res;
        }


        public async Task<Response> Checkout(string token,
                                             SalesOrderMasterDto model)
        {
            var res = await Client.Post<Response>(url: $"{Route}Checkout", obj: model, token: token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<SalesOrderMasterDto>(value: res.Model.String());
            return res;
        }


        public async Task<Response> PrintReceipt(string token, int orderId)
        {
            var response = await Client.Get<Response>(url: $"{Route}PrintReceipt/{orderId}", token: token);
            response.Model = (bool) response.Model;
            return response;
        }


        public async Task<IList<SalesOrderStatus_SLM>> GetOrderStatusSelectList(string token, SalesOrderStatusDto filter = null)
        {
            var res = await GetOrderStatusSelectListResponse(token: token, filter: filter);
            return res.Model != null ? (IList<SalesOrderStatus_SLM>) res.Model : new List<SalesOrderStatus_SLM>();
        }


        public async Task<Response> GetOrderStatusSelectListResponse(string token, SalesOrderStatusDto filter = null)
        {
            var res = await Client.Post<Response>(url: $"{Route}GetOrderStatusSelectList",
                                                   obj: filter ?? new SalesOrderStatusDto
                                                                  {
                                                                      Status = StatusTypes.Active.ToInt()
                                                                  },
                                                   token: token);
            if (res.Model != null)
                res.Model = JsonConvert.DeserializeObject<IList<SalesOrderStatus_SLM>>(value: res.Model.String());
            return res;
        }
    }
}