using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.RestaurantManagement.RestaurantFloorsRepos
{
    public interface IRestaurantFloorsRepository
    {
        Task<RestRestaurantFloorsDto> Create(RestRestaurantFloorsDto model);
        Task<RestRestaurantFloorsDto> Edit(RestRestaurantFloorsDto model);
        Task<List<RestRestaurantFloorsDto>> GetAll(RestRestaurantFloorsDto model);
        Task<bool> Delete(RestRestaurantFloorsDto model);
        Task<RestRestaurantFloorsDto> GetDetails(RestRestaurantFloorsDto model);
        Task<IList<RestRestaurantFloors_SLM>> GetSelectList(RestRestaurantFloorsDto model);
        Task<bool> IsExist(RestRestaurantFloorsDto model);
    }
}
