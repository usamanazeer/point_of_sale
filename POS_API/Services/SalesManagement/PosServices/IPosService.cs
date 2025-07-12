using Models;
using System.Threading.Tasks;

namespace POS_API.Services.SalesManagement.PosServices
{
    public interface IPosService
    {
        Task<Response> ApplyCategoryFilter(int companyId, int? categoryId/*, int? subcategoryId, string searchText*/);
        Task<Response> ApplySubCategoryFilter(int companyId, int? categoryId, int? subcategoryId/*, string searchText*/);
        Task<Response> AllDealsFilter(int companyId);
        Task<Response> SubCategoryDealsFilter(int companyId, int? subcategoryId);
        Task<Response> ApplySearchTextFilter(int companyId, string searchText);
    }
}
