//using Models.DTO.RestaurantManagement;
//using Models.DTO.ViewModels.SelectList.RestaurantManagement;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace POS_API.Repositories.RestaurantManagement
//{
//    public interface IRestaurantUnit
//    {
//        #region Floor
//        Task<RestRestaurantFloorsDto> CreateRestaurantFloor (RestRestaurantFloorsDto model);
//        Task<bool> DeleteRestaurantFloors(RestRestaurantFloorsDto model);
//        Task<RestRestaurantFloorsDto> EditRestaurantFloor(RestRestaurantFloorsDto model);
//        Task<List<RestRestaurantFloorsDto>> GetAllRestaurantFloors(RestRestaurantFloorsDto model);
//        Task<RestRestaurantFloorsDto> GetRestaurantFloorDetails(RestRestaurantFloorsDto model);
//        Task<IList<RestRestaurantFloors_SLM>> GetRestaurantFloorsSelectList(RestRestaurantFloorsDto model);
//        Task<bool> IsRestaurantFloorExist(RestRestaurantFloorsDto model);
//        #endregion


//        #region Dining Table
//        Task<RestDiningTableDto> CreateDiningTable(RestDiningTableDto model);
//        Task<bool> DeleteDiningTable(RestDiningTableDto model);
//        Task<bool> ReleaseOrOccupyDiningTable(RestDiningTableDto model);
//        Task<RestDiningTableDto> EditDiningTable(RestDiningTableDto model);
//        Task<List<RestDiningTableDto>> GetAllDiningTables(RestDiningTableDto model);
//        Task<RestDiningTableDto> GetDiningTableDetails(RestDiningTableDto model);
//        Task<IList<RestDiningTable_SLM>> GetDiningTablesSelectList(RestDiningTableDto model);
//        Task<bool> IsDiningTableExist(RestDiningTableDto model);
//        #endregion

//        #region Waiters
//        Task<RestWaiterDto> CreateWaiter(RestWaiterDto model);
//        Task<RestWaiterDto> EditWaiter(RestWaiterDto model);
//        Task<List<RestWaiterDto>> GetAllWaiters(RestWaiterDto model);
//        Task<bool> DeleteWaiter(RestWaiterDto model);
//        Task<RestWaiterDto> GetWaiterDetails(RestWaiterDto model);
//        Task<IList<RestWaiter_SLM>> GetWaitersSelectList(RestWaiterDto model);
//        Task<bool> IsWaiterExist(RestWaiterDto model);
//        #endregion

        
//    }
//}
