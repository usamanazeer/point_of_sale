//using Models.DTO.RestaurantManagement;
//using Models.DTO.ViewModels.SelectList.RestaurantManagement;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using POS_API.Repositories.RestaurantManagement.DiningTableRepos;
//using POS_API.Repositories.RestaurantManagement.RestaurantFloorsRepos;
//using POS_API.Repositories.RestaurantManagement.WaitersRepos;

//namespace POS_API.Repositories.RestaurantManagement
//{
//    public class RestaurantUnit : IRestaurantUnit
//    {

//        private readonly IRestaurantFloorsRepository _restaurantFloorsRepository;
//        private readonly IDiningTableRepository _diningTableRepository;
//        private readonly IWaitersRepository _waitersRepository;

//        public RestaurantUnit(IRestaurantFloorsRepository restaurantFloorsRepository, IDiningTableRepository diningTableRepository, IWaitersRepository waitersRepository) 
//        {
//            _restaurantFloorsRepository = restaurantFloorsRepository;
//            _diningTableRepository = diningTableRepository;
//            _waitersRepository = waitersRepository;
//        }


//        #region Floor
//        public async Task<RestRestaurantFloorsDto> CreateRestaurantFloor(RestRestaurantFloorsDto model)
//        {
//            return await _restaurantFloorsRepository.Create(model);
//        }
//        public async Task<bool> DeleteRestaurantFloors(RestRestaurantFloorsDto model)
//        {
//            return await _restaurantFloorsRepository.Delete(model);
//        }
//        public async Task<RestRestaurantFloorsDto> EditRestaurantFloor(RestRestaurantFloorsDto model)
//        {
//            return await _restaurantFloorsRepository.Edit(model);
//        }
//        public async Task<List<RestRestaurantFloorsDto>> GetAllRestaurantFloors(RestRestaurantFloorsDto model)
//        {
//            return await _restaurantFloorsRepository.GetAll(model);
//        }
//        public async Task<RestRestaurantFloorsDto> GetRestaurantFloorDetails(RestRestaurantFloorsDto model)
//        {
//            return await _restaurantFloorsRepository.GetDetails(model);
//        }
//        public async Task<IList<RestRestaurantFloors_SLM>> GetRestaurantFloorsSelectList(RestRestaurantFloorsDto model)
//        {
//            return await _restaurantFloorsRepository.GetSelectList(model);
//        }
//        public async Task<bool> IsRestaurantFloorExist(RestRestaurantFloorsDto model)
//        {
//            return await _restaurantFloorsRepository.IsExist(model);
//        }
//        #endregion

//        #region Dining Table
//        public async Task<RestDiningTableDto> CreateDiningTable(RestDiningTableDto model)
//        {
//            return await _diningTableRepository.Create(model);
//        }
//        public async Task<bool> DeleteDiningTable(RestDiningTableDto model)
//        {
//            return await _diningTableRepository.Delete(model);
//        }


//        public async Task<bool> ReleaseOrOccupyDiningTable(RestDiningTableDto model)
//        {
//            return await _diningTableRepository.ReleaseOrOccupy(model);
//        }


//        public async Task<RestDiningTableDto> EditDiningTable(RestDiningTableDto model)
//        {
//            return await _diningTableRepository.Edit(model);
//        }
//        public async Task<List<RestDiningTableDto>> GetAllDiningTables(RestDiningTableDto model)
//        {
//            return await _diningTableRepository.GetAll(model);
//        }
//        public async Task<RestDiningTableDto> GetDiningTableDetails(RestDiningTableDto model)
//        {
//            return await _diningTableRepository.GetDetails(model);
//        }
//        public async Task<IList<RestDiningTable_SLM>> GetDiningTablesSelectList(RestDiningTableDto model)
//        {
//            return await _diningTableRepository.GetSelectList(model);
//        }
//        public async Task<bool> IsDiningTableExist(RestDiningTableDto model)
//        {
//            return await _diningTableRepository.IsExist(model);
//        }
//        #endregion



//        #region Waiters
//        public async Task<RestWaiterDto> CreateWaiter(RestWaiterDto model)
//        {
//            return await _waitersRepository.Create(model);
//        }

//        public async Task<RestWaiterDto> EditWaiter(RestWaiterDto model)
//        {
//            return await _waitersRepository.Edit(model);
//        }

//        public async Task<List<RestWaiterDto>> GetAllWaiters(RestWaiterDto model)
//        {
//            return await _waitersRepository.GetAll(model);
//        }

//        public async Task<bool> DeleteWaiter(RestWaiterDto model)
//        {
//            return await _waitersRepository.Delete(model);
//        }

//        public async Task<RestWaiterDto> GetWaiterDetails(RestWaiterDto model)
//        {
//            return await _waitersRepository.GetDetails(model);
//        }

//        public async Task<IList<RestWaiter_SLM>> GetWaitersSelectList(RestWaiterDto model)
//        {
//            return await _waitersRepository.GetSelectList(model);
//        }

//        public async Task<bool> IsWaiterExist(RestWaiterDto model)
//        {
//            return await _waitersRepository.IsExist(model);
//        }
//        #endregion
//    }
//}