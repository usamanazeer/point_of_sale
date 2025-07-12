using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using POS_API.Repositories.InventoryManagement.ModifierRepos;
using System.Linq;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.ModifierServices
{
    public class ModifierService : IModifierService, IService
    {
        private readonly IModifierRepository _modifierRepository;
        public ModifierService(IModifierRepository modifierRepository) => _modifierRepository = modifierRepository;

        public async Task<Response> Create(InvModifierDto model)
        {
            var isExists = await IsExist(model);
            if (isExists) return Response.Error("Modifier Already Exists.", model: model);
            if (model.InvModifierItems != null)
            {
                //removing extra entries from recipe items
                model.InvModifierItems = model.InvModifierItems.Where(x => x.Status == StatusTypes.Active.ToInt() && x.ItemId > 0 && x.Quantity > 0)
                  .GroupBy(x => new { x.ItemId, x.BarCodeId }).Select(y => new InvModifierItemDto
                  {
                      ItemId = y.First().ItemId, BarCodeId = y.First().BarCodeId <= 0 ? null : y.First().BarCodeId,
                      Status = y.First().Status, Quantity = y.Sum(x => x.Quantity)
                  }).ToList();
            }
            var retRes = await _modifierRepository.Create(model);
            return Response.Message("Modifier Created Successfully.", StatusCodes.Created, retRes);
        }


        public async Task<Response> Delete(InvModifierDto model)
        {
            return await _modifierRepository.Delete(model) ? Response.Message("Modifier Deleted Successfully.", model: true)
                : Response.Message("Modifier Not Found.", StatusCodes.Not_Found, false);
        }


        public async Task<Response> Edit(InvModifierDto model)
        {
            var isExists = await IsExist(model);
            if (isExists) return Response.Error("Modifier Already Exists.",  model: model);

            if (model.InvModifierItems != null)
            {
                //removing extra entries from recipe items
                model.InvModifierItems = model.InvModifierItems.Where(x => x.Status == StatusTypes.Active.ToInt() && x.ItemId > 0 && x.Quantity > 0)
                  .GroupBy(x => new { x.ItemId, x.BarCodeId }).Select(y => new InvModifierItemDto()
                      { ItemId = y.First().ItemId, BarCodeId = y.First().BarCodeId <= 0 ? null: y.First().BarCodeId,
                        Status = y.First().Status, Quantity = y.Sum(x => x.Quantity),
                      }).ToList();
            }
            var retRes = await _modifierRepository.Edit(model);
            return retRes != null ? Response.Message("Modifier Updated Successfully.", StatusCodes.Updated, retRes) : Response.Message("Modifier Not Found.", StatusCodes.Not_Found);
        }

        public async Task<Response> GetAll(InvModifierDto model)
        {
            var res = await _modifierRepository.GetAll(model);
            return res.Any() ? Response.Message(null, model:res) : Response.Message("No Modifier Found.", StatusCodes.Not_Found);
        }

        public async Task<Response> GetDetails(InvModifierDto model)
        {
            var res = await _modifierRepository.GetDetails(model);
            return res != null ? Response.Message(null, model:res) : Response.Message("Modifier Not Found", StatusCodes.Not_Found);
        }

        public async Task<Response> GetSelectList(InvModifierDto model)
        {
            var itemsList = await _modifierRepository.GetSelectList(model);
            return itemsList != null ? Response.Message(null,model:itemsList) : Response.Message("Modifiers Not Found", StatusCodes.Not_Found);

        }

        public async Task<bool> IsExist(InvModifierDto model) => await _modifierRepository.IsExist(model);
    }
}