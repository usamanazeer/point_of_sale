using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.VendorServices
{
    public interface IVendorService
    {
        Task<bool> IsExist(string token , InvVendorDto vendor);
        Task<InvVendorDto> Get(string token, int? id = null, int? status = null, bool? getDeleted = null);
        Task<InvVendorDto> Details(string token, int id);
        Task<Response> Edit( string token, InvVendorDto vendor);
        Task<Response> Create(string token, InvVendorDto vendor);
        Task<Response> Delete(string token, int id);
        Task<Response> GetSelectListResponse(string token, InvVendorDto model = null);
        Task<List<InvVendor_SLM>> GetSelectList(string token, InvVendorDto model = null);

    }
}
