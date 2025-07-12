using System.Linq;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Repositories.InventoryManagement.ColorRepos;
using System.Threading.Tasks;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Services.InventoryManagement.ColorServices
{
    public class ColorService : IColorService, IService
    {
        private readonly IColorRepository _colorRepository;
        public ColorService(IColorRepository colorRepository) => _colorRepository = colorRepository;


        public async Task<Response> Create(InvColorDto model)
        {
            var isExist = await IsExist(model);
            return isExist ?
                Response.Error("Color Already Exists.", model: model)
                : Response.Message("Color Created Successfully.", StatusCodesEnums.Created, model: await _colorRepository.Create(model));
        }

        public async Task<Response> Delete(InvColorDto model)
        {
            return await _colorRepository.Delete(model) ? Response.Message("Color Deleted Successfully.", model: true)
                : Response.Message("Color Not Found.", StatusCodesEnums.Not_Found, false);
        }

        public async Task<Response> Edit(InvColorDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Color Already Exists.", model: model);

            var res = await _colorRepository.Edit(model);
            return res != null ?
                Response.Message("Color Updated Successfully.", StatusCodesEnums.Updated, model: res) :
                Response.Message("Color Not Found.", StatusCodesEnums.Not_Found, model: model);
        }

        public async Task<Response> GetAll(InvColorDto model)
        {
            var res = await _colorRepository.GetAll(model: model);
            return res.Any() ? Response.Message(null, model: res) : Response.Message("Color Not Found.", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetDetails(InvColorDto model)
        {
            var res = await _colorRepository.GetDetails(model: model);
            return res != null ? Response.Message(null, model: res) : Response.Message("Color Not Found", StatusCodesEnums.Not_Found);
        }

        public async Task<bool> IsExist(InvColorDto model) => await _colorRepository.IsExist(model);
    }
}
