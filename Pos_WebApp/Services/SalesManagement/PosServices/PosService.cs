using Models;
using Models.DTO.InventoryManagement.ViewDTO;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;
using System.Collections.Generic;
using System.Threading.Tasks;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Services.SalesManagement.PosServices
{
    public class PosService : ServiceBase, IPosService, IService
    {

        public PosService(IClientManager clientManager) : base("api/pos/", clientManager)
        {
        }
        public async Task<Response> ApplyCategoryFilter(string token, int? categoryId = null)
        {
            var res = await Client.Get<Response>($"{Route}ApplyCategoryFilter?categoryId={categoryId}", token);
            if (res.ResponseCode == StatusCodesEnums.OK.ToInt())
            {
                var dataList = JsonConvert.DeserializeObject<object[]>(res.Model.String());
                var subcategories = JsonConvert.DeserializeObject<IList<InvSubCategory_SLM>>(dataList[0].String());
                var items = JsonConvert.DeserializeObject<IList<InvItemViewDto>>(dataList[1].String());
                res.Model = new object[] { subcategories, items };
            }
            return res;
        }
        public async Task<Response> ApplySubCategoryFilter(string token, int? categoryId = null, int ? subcategoryId = null)
        {
            var res = await Client.Get<Response>($"{Route}ApplySubCategoryFilter?categoryId={categoryId}&subcategoryId={subcategoryId}", token);
            if (res.ResponseCode == StatusCodesEnums.OK.ToInt())
            {
                var itemsList = JsonConvert.DeserializeObject<IList<InvItemViewDto>>(res.Model.String());
                res.Model = itemsList;
            }
            return res;
        }

        public async Task<Response> AllDealsFilter(string token)
        {
            var res = await Client.Get<Response>($"{Route}AllDealsFilter", token);
            if (res.ResponseCode != StatusCodesEnums.OK.ToInt()) 
                return res;
            var dataList = JsonConvert.DeserializeObject<object[]>(res.Model.String());
            var subcategories = JsonConvert.DeserializeObject<IList<InvSubCategory_SLM>>(dataList[0].String());
            var items = JsonConvert.DeserializeObject<IList<InvItemViewDto>>(dataList[1].String());
            res.Model = new object[] { subcategories, items };
            return res;
        }

        public async Task<Response> SubCategoryDealsFilter(string token, int? subcategoryId)
        {
            var res = await Client.Get<Response>($"{Route}SubCategoryDealsFilter?subcategoryId={subcategoryId}", token);
            if (res.ResponseCode == StatusCodesEnums.OK.ToInt())
            {
                var itemsList = JsonConvert.DeserializeObject<IList<InvItemViewDto>>(res.Model.String());
                res.Model = itemsList;
            }
            return res;
        }

        public async Task<Response> ApplySearchTextFilter(string token, string searchText)
        {
            var res = await Client.Get<Response>($"{Route}ApplySearchTextFilter?searchText={searchText}", token);
            if (res.ResponseCode != StatusCodesEnums.OK.ToInt()) 
                return res;
            var itemsList = JsonConvert.DeserializeObject<IList<InvItemViewDto>>(res.Model.String());
            res.Model = itemsList;
            return res;
        }
    }
}
