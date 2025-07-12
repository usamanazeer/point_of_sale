using System.Linq;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Repositories.InventoryManagement.BrandRepos;
using System.Threading.Tasks;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Services.InventoryManagement.BrandServices
{
    public class BrandService : IBrandService, IService
    {
        private readonly IBrandRepository _brandRepository;
        public BrandService(IBrandRepository brandRepository) => _brandRepository = brandRepository;

        public async Task<Response> Create(InvBrandDto model)
        {
            var isExist = await IsExist(model);
            return isExist ? 
                Response.Error("Brand Already Exists.", model: model) 
                : Response.Message("Brand Created Successfully.", StatusCodesEnums.Created, model: await _brandRepository.Create(model));
        }

        public async Task<Response> Delete(InvBrandDto model)
        {
            return await _brandRepository.Delete(model) ? Response.Message("Brand Deleted Successfully.", model: true)
                : Response.Message("Brand Not Found.", StatusCodesEnums.Not_Found, false);
        }
        
        public async Task<Response> Edit(InvBrandDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Brand Already Exists.", model: model);
            
            var res = await _brandRepository.Edit(model);
            return res != null ? 
                Response.Message("Brand Updated Successfully.", StatusCodesEnums.Updated, model: res) : 
                Response.Message("Brand Not Found.", StatusCodesEnums.Not_Found, model: model);
        }

        public async Task<Response> GetAll(InvBrandDto model)
        {
            var res = await _brandRepository.GetAll(model: model);
            return res.Any() ? Response.Message(null, model: res) : Response.Message("Brand Not Found.", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetDetails(InvBrandDto model)
        {
            var res = await _brandRepository.GetDetails(model: model);
            return res != null ? Response.Message(null, model: res) : Response.Message("Brand Not Found", StatusCodesEnums.Not_Found);
        }

        public async Task<bool> IsExist(InvBrandDto model) => await _brandRepository.IsExist(model);
    }
}
