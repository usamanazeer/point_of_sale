using Models;
using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.RastaurantManagement.WaitersServices
{
    public interface IWaitersService
    {
        Task<Response> Create(string token, RestWaiterDto model);
        Task<Response> Delete(string token, int id);
        Task<Response> Edit(string token, RestWaiterDto model);
        Task<RestWaiterDto> Get(string token, RestWaiterDto model = null);
        Task<RestWaiterDto> Details(string token, int id);
        Task<IList<RestWaiter_SLM>> GetSelectList(string tOKEN, RestWaiterDto model = null);
        Task<Response> GetSelectListResponse(string tOKEN, RestWaiterDto model = null);
    }
}