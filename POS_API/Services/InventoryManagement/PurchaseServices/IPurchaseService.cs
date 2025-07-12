using System.Threading.Tasks;
using Models;
using Models.DTO.InventoryManagement;

namespace POS_API.Services.InventoryManagement.PurchaseServices
{
    public interface IPurchaseService
    {
        Task<Response> Create(InvPurchaseMasterDto purchaseMasterDto);
        Task<Response> GetAll(InvPurchaseMasterDto model);
        Task<Response> GetDetails(InvPurchaseMasterDto model);
    }
}
