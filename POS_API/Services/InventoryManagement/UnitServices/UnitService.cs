using System.Linq;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Repositories.InventoryManagement.UnitRepos;
using System.Threading.Tasks;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Services.InventoryManagement.UnitServices
{
    public class UnitService : IUnitService, IService
    {
        private readonly IUnitRepository _unitRepository;
        public UnitService(IUnitRepository unitRepository) => _unitRepository = unitRepository;

        public async Task<Response> Create(InvUnitDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Unit Already Exists.", model: model);
            var createResponse = await _unitRepository.Create(model);
            return Response.Message("Unit Created Successfully.", StatusCodesEnums.Created, createResponse);
        }


        public async Task<Response> Delete(InvUnitDto model)
        {
            return await _unitRepository.Delete(model) ? Response.Message("Unit Deleted Successfully.", model: true)
                : Response.Message("Unit Not Found.", StatusCodesEnums.Not_Found, false);
        }

        public async Task<Response> Edit(InvUnitDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Unit Already Exists.", model: model);

            var editResponse = await _unitRepository.Edit(model);
            return editResponse != null ? Response.Message("Unit Updated Successfully.", StatusCodesEnums.Updated, editResponse)
                : Response.Message("Unit Not Found.", StatusCodesEnums.Not_Found,  model);
        }

        public async Task<Response> GetAll(InvUnitDto model)
        {
            var res = await _unitRepository.GetAll(model);
            return res.Any() ? Response.Message(null, model:res) : Response.Message("No Unit Found.", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetDetails(InvUnitDto model)
        {
            var res = await _unitRepository.GetDetails(model);
            return res != null ? Response.Message(null,model:res) : Response.Message("Unit Not Found.", StatusCodesEnums.Not_Found);
        }

        public async Task<bool> IsExist(InvUnitDto model) => await _unitRepository.IsExist(model);
    }
}