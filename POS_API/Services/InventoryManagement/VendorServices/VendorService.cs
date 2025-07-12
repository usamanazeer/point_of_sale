using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Repositories.InventoryManagement.VendorRepos;
using static Models.Enums.StatusCodes;

namespace POS_API.Services.InventoryManagement.VendorServices
{
    public class VendorService : IVendorService, IService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorService(IVendorRepository vendorRepository) => _vendorRepository = vendorRepository;

        public async Task<Response> Create(InvVendorDto model)
        {
            var returnModel = await _vendorRepository.Create(model: model);
            var response = returnModel.Response;
            response.Model = model;
            return response;
        }
        
        public async Task<Response> Edit(InvVendorDto model)
        {
            var responseModel = await _vendorRepository.Edit(model: model);
            if (responseModel is not null)
            {
                var response = responseModel.Response;
                response.Model = responseModel;
                return response;
            }
            return Response.Message("Vendor Not Found.", Not_Found, model: model);
        }

        public async Task<Response> GetAll(InvVendorDto model)
        {
            var res = await _vendorRepository.GetAll(model: model);
            return res.Any() ? Response.Message(null, model:res) : Response.Message("No Vendor Found", Not_Found);
        }

        public async Task<Response> GetDetails(InvVendorDto invVendorDto)
        {
            var res = await _vendorRepository.GetDetails(model: invVendorDto);
            return res != null ? Response.Message(null, model:res) : Response.Message("Vendor Not Found", Not_Found);
        }

        public async Task<bool> ChangeStatus(InvVendorDto model) => await _vendorRepository.ChangeStatus(model: model);

        public async Task<bool> IsExist(InvVendorDto model) => await _vendorRepository.IsExist(model: model);


        public async Task<Response> Delete(InvVendorDto model)
        {
            return await _vendorRepository.Delete(model) ? Response.Message("Vendor Deleted Successfully.", model: true)
                : Response.Message("Vendor Not Found.", Not_Found, false);
        }

        public async Task<Response> GetSelectList(InvVendorDto model)
        {
            var itemsList = await _vendorRepository.GetSelectList(model: model);
            return itemsList != null ? Response.Message(null, model: itemsList) : Response.Message("Vendor Not Found", Not_Found);
        }
    }
}