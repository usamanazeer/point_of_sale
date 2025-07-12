using System.Linq;
using Models;
using Models.DTO.InventoryManagement;
using POS_API.Repositories.InventoryManagement.ItemBarCodeRepos;
using System.Threading.Tasks;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Services.InventoryManagement.ItemBarCodeServices
{
    public class ItemBarCodeService : IItemBarCodeService, IService
    {
        private readonly IItemBarCodeRepository _itemBarCodeRepository;
        public ItemBarCodeService(IItemBarCodeRepository itemBarCodeRepository) => _itemBarCodeRepository = itemBarCodeRepository;

        public async Task<Response> Create(InvItemBarCodeDto model)
        {
            var isExists = await IsExist(model);
            return !isExists ? Response.Message("BarCode Created Successfully.", StatusCodesEnums.Created, await _itemBarCodeRepository.Create(model)) 
                : Response.Error("BarCode Already Exists.", model: model);
        }

        public async Task<Response> Delete(InvItemBarCodeDto model)
        {
            return await _itemBarCodeRepository.Delete(model) ? Response.Message("BarCode Deleted Successfully.", model: true)
                : Response.Message("BarCode Not Found.", StatusCodesEnums.Not_Found, false);
        }

        public async Task<Response> Edit(InvItemBarCodeDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("BarCode Already Exists.",  model: model);

            var retRes = await _itemBarCodeRepository.Edit(model);
            return retRes != null ? Response.Message("BarCode Updated Successfully.", StatusCodesEnums.Updated, retRes) : Response.Message("BarCode Not Found.", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetAll(InvItemBarCodeDto model)
        {
            var res = await _itemBarCodeRepository.GetAll(model);
            return res.Any() ? Response.Message(null, model:res) : Response.Message("No BarCode Found", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetDetails(InvItemBarCodeDto model)
        {
            var res = await _itemBarCodeRepository.GetDetails(model);
            return res != null ? Response.Message(null, model:res) : Response.Message("BarCode Not Found", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetSelectList(InvItemBarCodeDto model)
        {
            var itemsList = await _itemBarCodeRepository.GetSelectList(model);
            return itemsList != null ? Response.Message(null, model:itemsList) : Response.Message("Item Not Found", StatusCodesEnums.Not_Found);
        }

        public async Task<bool> IsExist(InvItemBarCodeDto model) => await _itemBarCodeRepository.IsExist(model);
    }
}