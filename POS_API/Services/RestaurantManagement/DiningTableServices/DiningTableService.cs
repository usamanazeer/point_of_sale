using System.Linq;
using Models;
using Models.Enums;
using System.Threading.Tasks;
using Models.DTO.RestaurantManagement;
using POS_API.Repositories.RestaurantManagement.DiningTableRepos;

namespace POS_API.Services.RestaurantManagement.DiningTableServices
{
    public class DiningTableService : IDiningTableService, IService
    {
        private readonly IDiningTableRepository _diningTableRepository;
        public DiningTableService(IDiningTableRepository diningTableRepository) => _diningTableRepository = diningTableRepository;

        public async Task<Response> Create(RestDiningTableDto model)
        {
            var isExist = await IsExist(model);
            return isExist ?
                Response.Error("Dining Table Already Exists.", model: model)
                : Response.Message("Dining Table Created Successfully.", StatusCodes.Created, model: await _diningTableRepository.Create(model));
        }


        public async Task<Response> Delete(RestDiningTableDto model)
        {
            return await _diningTableRepository.Delete(model) ? Response.Message("Dining Table Deleted Successfully.", model: true)
                : Response.Message("Dining Table Not Found.", StatusCodes.Not_Found, false);
        }


        public async Task<Response> Edit(RestDiningTableDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Dining Table Already Exists.", model: model);

            var res = await _diningTableRepository.Edit(model);
            return res != null ?
                Response.Message("Dining Table Updated Successfully.", StatusCodes.Updated, model: res) :
                Response.Message("Dining Table Not Found.", StatusCodes.Not_Found, model: model);
        }

        public async Task<Response> GetAll(RestDiningTableDto model)
        {
            var res = await _diningTableRepository.GetAll(model: model);
            return res.Any() ? Response.Message(null, model: res) : Response.Message("Dining Table Not Found.", StatusCodes.Not_Found);
        }

        public async Task<Response> GetDetails(RestDiningTableDto model)
        {
            var res = await _diningTableRepository.GetDetails(model: model);
            return res != null ? Response.Message(null, model: res) : Response.Message("Dining Table Not Found.", StatusCodes.Not_Found);
        }

        public async Task<Response> GetSelectList(RestDiningTableDto model)
        {
            var itemsList = await _diningTableRepository.GetSelectList(model);
            return itemsList != null ? Response.Message(null, model: itemsList) : Response.Message("Dining Tables Not Found.", StatusCodes.Not_Found);
        }

        public async Task<bool> IsExist(RestDiningTableDto model) => await _diningTableRepository.IsExist(model);


        public async Task<Response> ReleaseOrOccupy(RestDiningTableDto model)
        {
            return await _diningTableRepository.ReleaseOrOccupy(model: model)
                ? Response.Message($"Dining Table {(model.IsOccupied ? "Occupied" : "Released")} Successfully.", model: true)
                : Response.Message("Dining Table Not Found.", StatusCodes.Not_Found, false);
        }
    }
}
