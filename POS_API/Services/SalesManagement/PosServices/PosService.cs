using System.Threading.Tasks;
using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using POS_API.Services.InventoryManagement.CategoryServices;
using POS_API.Services.InventoryManagement.ItemServices;

namespace POS_API.Services.SalesManagement.PosServices
{
    public class PosService : IPosService, IService
    {
        private readonly IItemService _itemService;

        private readonly ISubCategoryService _subCategoryService;


        public PosService(ISubCategoryService subCategoryService,
                          IItemService itemService)
        {
            _subCategoryService = subCategoryService;
            _itemService = itemService;
        }


        public async Task<Response> ApplyCategoryFilter(int companyId, int? categoryId)
        {
            var r = new Response();
            var subcategories = (await _subCategoryService.GetSelectList(model: new InvSubCategoryDto
                                                                                {
                                                                                    CategoryId = categoryId,
                                                                                    DisplayOnPos = true,
                                                                                    CompanyId = companyId
                                                                                })).Model;
            var items = (await _itemService.GetAll(model: new InvItemDto
              {
                  CategoryId = categoryId,
                  DisplayOnPos = true,
                  ItemTypesFilter = new[] { ItemTypes.NormalItem.ToInt(), ItemTypes.DealItem.ToInt(), ItemTypes.RecipeItem.ToInt() },
                  CompanyId = companyId
              }, forPos: true, withModifiers: true/*,fromCache:true*/)).Model;
            r.Model = new[] { subcategories, items };
            r.ResponseCode = StatusCodes.OK.ToInt();
            return r;
        }


        public async Task<Response> ApplySubCategoryFilter(int companyId, int? categoryId, int? subcategoryId)
        {
            var r = new Response();
            var items = (await _itemService.GetAll(model: new InvItemDto
              {
                  CategoryId = categoryId,
                  SubCategoryId = subcategoryId,
                  DisplayOnPos = true,
                  ItemTypesFilter = new[] { ItemTypes.NormalItem.ToInt(), ItemTypes.DealItem.ToInt(), ItemTypes.RecipeItem.ToInt() },
                  CompanyId = companyId
              }, forPos: true, withModifiers: true/*, fromCache: true*/)).Model;

            r.Model = items;
            r.ResponseCode = StatusCodes.OK.ToInt();
            return r;
        }


        public async Task<Response> AllDealsFilter(int companyId)
        {
            var r = new Response();
            var subcategories = (await _subCategoryService.GetSelectList(model: new InvSubCategoryDto
                                                                                {
                                                                                    DisplayOnPos = true,
                                                                                    CompanyId = companyId
                                                                                })).Model;
            var items = (await _itemService.GetAll(model: new InvItemDto
                                                          {
                                                              DisplayOnPos = true,
                                                              ItemTypesFilter = new[]
                                                                                {
                                                                                    ItemTypes.DealItem.ToInt()
                                                                                },
                                                              CompanyId = companyId
                                                          },
                                                   forPos: true,
                                                   withModifiers: true/*, fromCache: true*/)).Model;
            r.Model = new[]
                      {
                          subcategories,
                          items
                      };
            r.ResponseCode = StatusCodes.OK.ToInt();
            return r;
        }


        public async Task<Response> SubCategoryDealsFilter(int companyId,
                                                           int? subcategoryId)
        {
            var r = new Response();
            var items = (await _itemService.GetAll(model: new InvItemDto
                                                          {
                                                              SubCategoryId = subcategoryId,
                                                              DisplayOnPos = true,
                                                              ItemTypesFilter = new[]
                                                                                {
                                                                                    ItemTypes.DealItem.ToInt()
                                                                                },
                                                              CompanyId = companyId
                                                          },
                                                   forPos: true,
                                                   withModifiers: true/*, fromCache: true*/)).Model;

            r.Model = items;
            r.ResponseCode = StatusCodes.OK.ToInt();
            return r;
        }


        public async Task<Response> ApplySearchTextFilter(int companyId,
                                                          string searchText)
        {
            var r = new Response();
            var items = (await _itemService.GetAll(model: new InvItemDto
                                                          {
                                                              DisplayOnPos = true,
                                                              ItemTypesFilter = new[]
                                                                                {
                                                                                    ItemTypes.DealItem.ToInt(),
                                                                                    ItemTypes.RecipeItem.ToInt(),
                                                                                    ItemTypes.NormalItem.ToInt()
                                                                                },
                                                              SearchText = searchText,
                                                              CompanyId = companyId
                                                          },
                                                   forPos: true,
                                                   withModifiers: true/*, fromCache: true*/)).Model;

            r.Model = items;
            r.ResponseCode = StatusCodes.OK.ToInt();
            return r;
        }
    }
}