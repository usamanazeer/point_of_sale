using Models;
using Models.DTO.InventoryManagement;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.VendorServices
{
    public interface IVendorService
    {
        Task<Response> Create(InvVendorDto model);
        Task<bool> ChangeStatus(InvVendorDto model);
        Task<Response> Edit(InvVendorDto model);
        Task<Response> GetAll(InvVendorDto model);
        Task<Response> GetDetails(InvVendorDto invVendorDto);
        Task<bool> IsExist(InvVendorDto model);
        Task<Response> Delete(InvVendorDto model);

        Task<Response> GetSelectList(InvVendorDto model);
    }
}