//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Models;
//using Models.DTO.InventoryManagement;
//using Newtonsoft.Json;
//using Pos_WebApp.Attributes;
//using Pos_WebApp.Controllers;
//using Pos_WebApp.Services.InventoryManagement.CategoryServices;
//using Pos_WebApp.Services.RastaurantManagement.WaitersServices;
//using Pos_WebApp.Services.RestaurantManagement.DiningTableServices;
//using Pos_WebApp.Services.SalesManagement.PosServices;
//using StatusCodesEnums = Models.Enums.StatusCodes;

//namespace Pos_WebApp.Areas.SalesManagement.Controllers
//{
//    [Obsolete, Area("SalesManagement"), Route("[controller]")]
//    public class PosController : BaseController
//    {
//        private readonly IMainCategoryService _mainCategoryService;
//        private readonly IPosService _posService;
//        private readonly IDiningTableService _diningTableService;
//        private readonly IWaitersService _waitersService;
//        public PosController(IPosService posService, IMainCategoryService mainCategoryService, 
//            IDiningTableService diningTableService, IWaitersService waitersService)
//        {
//            _posService = posService;
//            _mainCategoryService = mainCategoryService;
//            _diningTableService = diningTableService;
//            _waitersService = waitersService;
//        }


//        [RightAuthorization, HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            try
//            {
//                ViewBag.Categories = await _mainCategoryService.Get(TOKEN, new InvCategoryDto() { DisplayOnPos = true });
//                ViewBag.Ser_Categories = JsonConvert.SerializeObject(ViewBag.Categories.MainCategories);

//                ViewBag.Tables = await _diningTableService.GetSelectList(TOKEN);
//                ViewBag.Ser_Tables = JsonConvert.SerializeObject(ViewBag.Tables);

//                ViewBag.Waiters = await _waitersService.GetSelectList(TOKEN);
//                ViewBag.Ser_Waiters = JsonConvert.SerializeObject(ViewBag.Waiters);
//            }

//            catch (Exception)
//            {
//                ViewBag.ErrorCode = StatusCodesEnums.Error_Occured;
//                ViewBag.ErrorMessage = "An Error Occured, while loading data.";
//            }
//            return View();
//        }


//        [JsonResponseAction, HttpGet("ApplyCategoryFilter")]
//        public async Task<JsonResult> ApplyCategoryFilter(int? categoryId)
//        {
//            var response = new Response();
//            try
//            {
//                response = await _posService.ApplyCategoryFilter(TOKEN, categoryId);
//                return Json(response);
//            }
//            catch (Exception)
//            {
//                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
//                response.ErrorMessage = "An Error Occured, while getting data.";
//                return Json(response);
//            }
//        }


//        [JsonResponseAction, HttpGet("ApplySubCategoryFilter")]
//        public async Task<JsonResult> ApplySubCategoryFilter(int? categoryId, int? subcategoryId)
//        {
            
//            var response = new Response();
//            try
//            {
//                response = await _posService.ApplySubCategoryFilter(TOKEN, categoryId, subcategoryId);
//                return Json(response);
//            }
//            catch (Exception)
//            {
//                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
//                response.ErrorMessage = "An Error Occured, while getting data.";
//                return Json(response);
//            }
//        }


//        [JsonResponseAction, HttpGet("AllDealsFilter")]
//        public async Task<JsonResult> AllDealsFilter()
//        {
//            var response = new Response();
//            try
//            {
//                response = await _posService.AllDealsFilter(TOKEN);
//                return Json(response);
//            }
//            catch (Exception)
//            {
//                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
//                response.ErrorMessage = "An Error Occured, while getting data.";
//                return Json(response);
//            }
//        }


//        [JsonResponseAction, HttpGet("SubCategoryDealsFilter")]
//        public async Task<JsonResult> SubCategoryDealsFilter(int? subcategoryId)
//        {
//            var response = new Response();
//            try
//            {
//                response = await _posService.SubCategoryDealsFilter(TOKEN, subcategoryId);
//                return Json(response);
//            }
//            catch (Exception)
//            {
//                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
//                response.ErrorMessage = "An Error Occured, while getting data.";
//                return Json(response);
//            }
//        }


//        [JsonResponseAction, HttpGet("ApplySearchTextFilter")]
//        public async Task<JsonResult> ApplySearchTextFilter(string searchText)
//        {
//            var response = new Response();
//            try
//            {
//                response = await _posService.ApplySearchTextFilter(TOKEN, searchText);
//                return Json(response);
//            }
//            catch (Exception)
//            {
//                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
//                response.ErrorMessage = "An Error Occured, while getting data.";
//                return Json(response);
//            }
//        }
//    }
//}