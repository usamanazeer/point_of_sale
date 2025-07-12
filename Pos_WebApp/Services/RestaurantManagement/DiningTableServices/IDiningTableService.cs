using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;

namespace Pos_WebApp.Services.RestaurantManagement.DiningTableServices
{
    public interface IDiningTableService
    {
        Task<Response> Create(string token, RestDiningTableDto model);
        Task<Response> Delete(string token, int id);
        Task<Response> Edit(string token, RestDiningTableDto model);
        Task<RestDiningTableDto> Get(string token, RestDiningTableDto model = null);
        Task<RestDiningTableDto> Details(string token, int id);
        Task<IList<RestDiningTable_SLM>> GetSelectList(string tOKEN, RestDiningTableDto model = null);
        Task<Response> GetSelectListResponse(string tOKEN, RestDiningTableDto model = null);


        Task<Response> ReleaseOrOccupy(string token,
                                         RestDiningTableDto model);
    }
}
