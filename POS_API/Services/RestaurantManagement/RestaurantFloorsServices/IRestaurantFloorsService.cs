using Models;
using Models.DTO.RestaurantManagement;
using System.Threading.Tasks;

namespace POS_API.Services.RestaurantManagement.RestaurantFloorsServices
{
    public interface IRestaurantFloorsService
    {
        Task<Response> GetAll(RestRestaurantFloorsDto model);
        Task<Response> Create(RestRestaurantFloorsDto model);
        Task<Response> Edit(RestRestaurantFloorsDto model);
        Task<Response> Delete(RestRestaurantFloorsDto model);
        Task<Response> GetDetails(RestRestaurantFloorsDto model);
        Task<Response> GetSelectList(RestRestaurantFloorsDto model);
        Task<bool> IsExist(RestRestaurantFloorsDto model);
    }
}
