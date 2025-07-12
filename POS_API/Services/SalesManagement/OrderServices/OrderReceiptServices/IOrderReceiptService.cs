using Models.DTO.SalesManagement;
using System.Threading.Tasks;

namespace POS_API.Services.SalesManagement.OrderServices.OrderReceiptServices
{
    public interface IOrderReceiptService
    {
        Task<bool> PrintOrder(SalesOrderMasterDto salesOrderMasterDto, short printQuantity = 1);
        Task<bool> PrintSalesReceipt(SalesOrderMasterDto salesOrderMasterDto, short printQuantity = 1);
    }
}
