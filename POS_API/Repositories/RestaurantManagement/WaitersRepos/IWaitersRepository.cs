using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.RestaurantManagement.WaitersRepos
{
    public interface IWaitersRepository
    {
        Task<RestWaiterDto> Create(RestWaiterDto model);
        Task<RestWaiterDto> Edit(RestWaiterDto model);
        Task<List<RestWaiterDto>> GetAll(RestWaiterDto model);
        Task<bool> Delete(RestWaiterDto model);
        Task<RestWaiterDto> GetDetails(RestWaiterDto model);
        Task<IList<RestWaiter_SLM>> GetSelectList(RestWaiterDto model);
        Task<bool> IsExist(RestWaiterDto model);
    }
}
