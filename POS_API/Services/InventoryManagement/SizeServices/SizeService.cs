using System.Linq;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Repositories.InventoryManagement.SizeRepos;
using System.Threading.Tasks;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Services.InventoryManagement.SizeServices
{
    public class SizeService : ISizeService, IService
    {
        private readonly ISizeRepository _sizeRepository;
        public SizeService(ISizeRepository sizeRepository) => _sizeRepository = sizeRepository;

        public async Task<Response> Create(InvSizeDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Size Already Exists.", model: model);
            var res = await _sizeRepository.Create(model);
            return Response.Message("Size Created Successfully.", StatusCodesEnums.Created,res);
        }

        public async Task<Response> Delete(InvSizeDto model)
        {
            return await _sizeRepository.Delete(model) ? Response.Message("Size Deleted Successfully.", model: true)
                : Response.Message("Size Not Found.", StatusCodesEnums.Not_Found, false);
        }

        public async Task<Response> Edit(InvSizeDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Size Already Exists.", model: model);

            var res = await _sizeRepository.Edit(model);
            return res != null ? Response.Message("Size Updated Successfully.",StatusCodesEnums.Updated,res) : Response.Message("Size Not Found.",StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetAll(InvSizeDto model)
        {
            var res = await _sizeRepository.GetAll(model);
            return res.Any() ? Response.Message(null, model:res) : Response.Message("No Size Found", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetDetails(InvSizeDto model)
        {
            var res = await _sizeRepository.GetDetails(model);
            return res != null ? Response.Message(null, model: res) : Response.Message("Size Not Found", StatusCodesEnums.Not_Found);
        }

        public async Task<bool> IsExist(InvSizeDto model) => await _sizeRepository.IsExist(model);
    }
}