using System.Linq;
using Models;
using Models.DTO.RestaurantManagement;
using Models.Enums;
using POS_API.Repositories.RestaurantManagement.RestaurantFloorsRepos;
using System.Threading.Tasks;

namespace POS_API.Services.RestaurantManagement.RestaurantFloorsServices
{
    public class RestaurantFloorsService:IRestaurantFloorsService, IService
    {
        private readonly IRestaurantFloorsRepository _restaurantFloorsRepository;
        
        public RestaurantFloorsService(IRestaurantFloorsRepository restaurantFloorsRepository) => _restaurantFloorsRepository = restaurantFloorsRepository;

        public async Task<Response> Create(RestRestaurantFloorsDto model)
        {
            var isExist = await IsExist(model);
            return isExist ?
                Response.Error("Floor Already Exists.", model: model)
                : Response.Message("Floor Created Successfully.", StatusCodes.Created, model: await _restaurantFloorsRepository.Create(model));
        }

        public async Task<Response> Delete(RestRestaurantFloorsDto model)
        {
            return await _restaurantFloorsRepository.Delete(model) ? Response.Message("Floor Deleted Successfully.", model: true)
                : Response.Message("Floor Not Found.", StatusCodes.Not_Found, false);
        }

        public async Task<Response> Edit(RestRestaurantFloorsDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Floor Already Exists.", model: model);

            var res = await _restaurantFloorsRepository.Edit(model);
            return res != null ?
                Response.Message("Floor Updated Successfully.", StatusCodes.Updated, model: res) :
                Response.Message("Floor Not Found.", StatusCodes.Not_Found, model: model);
        }

        public async Task<Response> GetAll(RestRestaurantFloorsDto model)
        {
            var res = await _restaurantFloorsRepository.GetAll(model: model);
            return res.Any() ? Response.Message(null, model: res) : Response.Message("Floor Not Found.", StatusCodes.Not_Found);
        }

        public async Task<Response> GetDetails(RestRestaurantFloorsDto model)
        {
            var res = await _restaurantFloorsRepository.GetDetails(model: model);
            return res != null ? Response.Message(null, model: res) : Response.Message("Floor Not Found", StatusCodes.Not_Found);
        }

        public async Task<Response> GetSelectList(RestRestaurantFloorsDto model)
        {
            var itemsList = await _restaurantFloorsRepository.GetSelectList(model);
            return itemsList != null ? Response.Message(null, model: itemsList) : Response.Message("Floor Not Found.", StatusCodes.Not_Found);
        }

        public async Task<bool> IsExist(RestRestaurantFloorsDto model) => await _restaurantFloorsRepository.IsExist(model);
    }
}