using Models.DTO.InventoryManagement;
using System.Threading.Tasks;
using Models;

namespace Pos_WebApp.Services.InventoryManagement.PurchaseServices
{
    public interface IPurchaseService
    {
        Task<Response> Create(string token, InvPurchaseMasterDto purchaseMasterDto);


        Task<InvPurchaseMasterDto> Get(string token,
                              InvPurchaseMasterDto model);


        Task<InvPurchaseMasterDto> Details(string token,
                                           int id);

    }
}
