using Models.DTO.SalesManagement;
using Models.DTO.ViewModels.SelectList.SalesManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.SalesManagement.OrderRepos
{
    internal interface IOrderRepository
    {
        Task<SalesOrderMasterDto> PlaceOrder(SalesOrderMasterDto model);
        Task<SalesOrderMasterDto> Edit(SalesOrderMasterDto model);
        Task<List<SalesOrderMasterDto>> GetAll(SalesOrderMasterDto model);
        [Obsolete]
        Task<bool> Delete(SalesOrderMasterDto model);
        Task<bool> ChangeOrderStatus(SalesOrderMasterDto model);
        Task<SalesOrderMasterDto> GetDetails(SalesOrderMasterDto model);
        Task<IList<SalesOrderMaster_SLM>> GetSelectList(SalesOrderMasterDto model);
        Task<bool> IsExist(SalesOrderMasterDto model);
        Task<SalesOrderMasterDto> Checkout(SalesOrderMasterDto model);
        Task<IList<SalesOrderStatus_SLM>> GetOrderStatusSelectList(SalesOrderStatusDto filter);
        Task<bool> CancelOrder(SalesOrderMasterDto model);
    }
}
