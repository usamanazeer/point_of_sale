using System.Linq;
using Models;
using Models.DTO.RestaurantManagement;
using System.Threading.Tasks;
using Models.Enums;
using POS_API.Repositories.RestaurantManagement.WaitersRepos;

namespace POS_API.Services.RestaurantManagement.WaitersServices
{
    public class WaitersService: IWaitersService, IService
    {
        private readonly IWaitersRepository _waitersRepository;
        public WaitersService(IWaitersRepository waitersRepository) => _waitersRepository = waitersRepository;

        public async Task<Response> Create(RestWaiterDto model)
        {
            var isExist = await IsExist(model);
            return isExist ?
                Response.Error("Waiter Already Exists.", model: model)
                : Response.Message("Waiter Created Successfully.", StatusCodes.Created, model: await _waitersRepository.Create(model));
        }


        public async Task<Response> Delete(RestWaiterDto model)
        {
            return await _waitersRepository.Delete(model) ? Response.Message("Waiter Deleted Successfully.", model: true)
                : Response.Message("Waiter Not Found.", StatusCodes.Not_Found, false);
        }

        public async Task<Response> Edit(RestWaiterDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error("Waiter Already Exists.", model: model);

            var res = await _waitersRepository.Edit(model);
            return res != null ?
                Response.Message("Waiter Updated Successfully.", StatusCodes.Updated, model: res) :
                Response.Message("Waiter Not Found.", StatusCodes.Not_Found, model: model);
        }

        public async Task<Response> GetAll(RestWaiterDto model)
        {
            var res = await _waitersRepository.GetAll(model: model);
            return res.Any() ? Response.Message(null, model: res) : Response.Message("Waiter Not Found.", StatusCodes.Not_Found);
        }

        public async Task<Response> GetDetails(RestWaiterDto model)
        {
            var res = await _waitersRepository.GetDetails(model: model);
            return res != null ? Response.Message(null, model: res) : Response.Message("Waiter Not Found", StatusCodes.Not_Found);
        }

        public async Task<Response> GetSelectList(RestWaiterDto model)
        {
            var itemsList = await _waitersRepository.GetSelectList(model);
            return itemsList != null ? Response.Message(null, model: itemsList) : Response.Message("Waiter Not Found.", StatusCodes.Not_Found);
        }

        public async Task<bool> IsExist(RestWaiterDto model) => await _waitersRepository.IsExist(model);
    }
}