using Models;
using Models.DTO.RestaurantManagement;
using System.Threading.Tasks;

namespace POS_API.Services.RestaurantManagement.WaitersServices
{
    public interface IWaitersService
    {
        Task<Response> GetAll(RestWaiterDto model);
        Task<Response> Create(RestWaiterDto model);
        Task<Response> Edit(RestWaiterDto model);
        Task<Response> Delete(RestWaiterDto model);
        Task<Response> GetDetails(RestWaiterDto model);
        Task<Response> GetSelectList(RestWaiterDto model);
        Task<bool> IsExist(RestWaiterDto model);
    }
}
