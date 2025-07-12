using Models;
using Models.DTO.RestaurantManagement;
using System.Threading.Tasks;

namespace POS_API.Services.RestaurantManagement.DiningTableServices
{
    public interface IDiningTableService
    {
        Task<Response> GetAll(RestDiningTableDto model);
        Task<Response> Create(RestDiningTableDto model);
        Task<Response> Edit(RestDiningTableDto model);
        Task<Response> Delete(RestDiningTableDto model);
        Task<Response> GetDetails(RestDiningTableDto model);
        Task<Response> GetSelectList(RestDiningTableDto model);
        Task<bool> IsExist(RestDiningTableDto model);
        Task<Response> ReleaseOrOccupy(RestDiningTableDto model);
    }
}   