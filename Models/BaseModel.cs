using System;

namespace Models
{
    public class BaseModel: JqueryDatatableParam
    {
        public int? Id { get; set; }
        public int? Status { get; set; }
        public int CompanyId { get; set; }
        public int? BranchId { get; set; }
        public string StatusText => GetValueText(Status ?? 0);
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Response Response { get; set; }
        public bool DisplayDeleted { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        //public ReadOnlyDictionary<int, string> StatusVals;


        public BaseModel()
        {
            //StatusVals = Models.Values.Status.StatusValues;
            Response = new Response();
        }
        private string GetValueText(int status) => ValuesHelper.Get_StatusValue(status);
    }
    public abstract class JqueryDatatableParam
    {
        public string sEcho { get; set; }
        public string sSearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iColumns { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
        public int totalRecords { get; set; }
    }
}
