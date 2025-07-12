using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.VendorRepos
{
    public interface IVendorRepository
    {
        Task<bool> ChangeStatus(InvVendorDto model);
        Task<InvVendorDto> Create(InvVendorDto model);
        Task<InvVendorDto> Edit(InvVendorDto model);
        Task<List<InvVendorDto>> GetAll(InvVendorDto model);
        Task<bool> IsExist(InvVendorDto model);
        Task<bool> Delete(InvVendorDto model);
        Task<IList<InvVendor_SLM>> GetSelectList(InvVendorDto model);
        Task<InvVendorDto> GetDetails(InvVendorDto model);
    }
}