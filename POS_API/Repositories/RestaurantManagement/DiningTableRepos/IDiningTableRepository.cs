using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.RestaurantManagement.DiningTableRepos
{
    public interface IDiningTableRepository
    {
        Task<RestDiningTableDto> Create(RestDiningTableDto model);
        Task<RestDiningTableDto> Edit(RestDiningTableDto model);
        Task<List<RestDiningTableDto>> GetAll(RestDiningTableDto model);
        Task<bool> Delete(RestDiningTableDto model);
        Task<RestDiningTableDto> GetDetails(RestDiningTableDto model);
        Task<IList<RestDiningTable_SLM>> GetSelectList(RestDiningTableDto model);
        Task<bool> IsExist(RestDiningTableDto model);
        Task<bool> ReleaseOrOccupy(RestDiningTableDto model);
    }
}
