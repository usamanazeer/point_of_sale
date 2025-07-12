namespace Models.DTO.ViewModels.SelectList.RestaurantManagement
{
    // ReSharper disable once InconsistentNaming
    public class RestDiningTable_SLM : SelectListModel
    {
        public int FloorId { get; set; }
        public int Capacity { get; set; }
        public string FloorName { get; set; }
        public bool? IsOccupied { get; set; }
        public int? BranchId { get; set; }
        //public int? CompanyId { get; set; }
    }
}
