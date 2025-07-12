namespace Models.DTO.ViewModels.SelectList.UserManagement
{
    // ReSharper disable once InconsistentNaming
    public class Branch_SLM : SelectListModel
    {
        //public string CompanyId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public bool? IsMainBranch { get; set; }
    }
}
