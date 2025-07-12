using Models;
using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.RestaurantManagement.RestaurantFloorsServices
{
    public interface IRestaurantFloorsService
    {
        Task<Response> Create(string token, RestRestaurantFloorsDto model);
        Task<Response> Delete(string token, int id);
        Task<Response> Edit(string token, RestRestaurantFloorsDto model);
        Task<RestRestaurantFloorsDto> Get(string token, RestRestaurantFloorsDto model = null);
        Task<RestRestaurantFloorsDto> Details(string token, int id);
        Task<IList<RestRestaurantFloors_SLM>> GetSelectList(string token, RestRestaurantFloorsDto model = null);
        Task<Response> GetSelectListResponse(string token, RestRestaurantFloorsDto model = null);
    }
}
