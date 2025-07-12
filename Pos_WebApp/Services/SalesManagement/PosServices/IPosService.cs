using System.Threading.Tasks;
using Models;

namespace Pos_WebApp.Services.SalesManagement.PosServices
{
    public interface IPosService
    {
        Task<Response> ApplyCategoryFilter(string token,
                                           int? categoryId =
                                               null /*, int? subcategoryId = null, string searchText = null*/);


        Task<Response> ApplySubCategoryFilter(string token,
                                              int? categoryId,
                                              int? subcategoryId = null /*, string searchText = null*/);


        Task<Response> AllDealsFilter(string token);


        Task<Response> SubCategoryDealsFilter(string token,
                                              int? subcategoryId);


        Task<Response> ApplySearchTextFilter(string token,
                                             string searchText);
    }
}