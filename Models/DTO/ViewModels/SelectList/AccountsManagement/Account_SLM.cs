namespace Models.DTO.ViewModels.SelectList.AccountsManagement
{
    // ReSharper disable once InconsistentNaming
    public class Account_SLM : SelectListModel
    {
        public string Code { get; set; }
        public string AccNo { get; set; }
        public int AccountTypeId { get; set; }
        public int? ParentId { get; set; }
        public bool IsEditable { get; set; }
        public bool HasParentChild { get; set; }
        public bool HasNonParentChild { get; set; }
        public bool HasNoChild { get; set; }
    }
}
