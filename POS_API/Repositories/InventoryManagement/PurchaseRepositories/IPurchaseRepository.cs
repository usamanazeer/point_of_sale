using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO.InventoryManagement;

namespace POS_API.Repositories.InventoryManagement.PurchaseRepositories
{
    public interface IPurchaseRepository
    {
        Task<InvPurchaseMasterDto> Create(InvPurchaseMasterDto purchaseMasterDto);
        Task<bool> IsExist(InvPurchaseMasterDto purchaseMasterDto);
        Task<List<InvPurchaseMasterDto>> GetAll(InvPurchaseMasterDto purchaseMasterDto,/* bool includeDetails = false,bool includePayments = false,*/  bool excludePaidBills = false);
        Task<InvPurchaseMasterDto> GetDetails(InvPurchaseMasterDto purchaseMasterDto,
                                              bool includePayments = false);
    }
}
