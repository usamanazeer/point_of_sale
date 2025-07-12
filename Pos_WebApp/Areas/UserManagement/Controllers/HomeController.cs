using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO.UserManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.UserManagement.CompanyServices;
using StatusCodesEnum = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.UserManagement.Controllers
{
    [Area(areaName: "UserManagement"), Route(template: "[controller]")]
    public class HomeController : BaseController
    {
        private readonly ICompanyService _companyService;

        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger,
                              ICompanyService companyService
        )
        {
            _companyService = companyService;
            _logger = logger;
        }


        public IActionResult Index()=>View();


        [RightAuthorization, HttpGet, Route(template: "/Account/Setup/{x?}", Name = "Setup")]
        public async Task<IActionResult> Setup()
        {
            try
            {
                var errors = new List<string>();
                var response = await _companyService.GetCompanyById(token: TOKEN);
                var company = ((CompanyDto) response.Model)?? new CompanyDto();
                if (response.ErrorOccured)
                {
                    if (response.ErrorCode == StatusCodes.Status404NotFound) 
                        errors.Add(item: "Company Not Found!");
                    ViewBag.ErrorsList = errors;
                }
                return View(model: company);
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex.StackTrace);
                return Error(requestID: ex.Message);
            }

            
        }
        [JsonResponseAction, RightAuthorization, HttpPost("/Account/CompanySetup")]
        public async Task<JsonResult> CompanySetup(CompanyDto model, IFormFile companyLogo)
        {
            try
            {
                if (model?.Id != 0)
                {
                    var res = await _companyService.Edit(token: TOKEN,  user: model, logoFile: companyLogo);
                    res.Model = (CompanyDto)res.Model;
                    return Json(res);
                }
                return Json(new Response()
                {
                    Model = model,
                    ErrorCode = StatusCodesEnum.Error_Occured.ToInt(),
                    ErrorMessage = "CompanyId can not be null."
                });
            }
            catch (Exception)
            {
                return Json(new Response()
                {
                    Model = model,
                    ErrorCode = StatusCodesEnum.Error_Occured.ToInt(), ErrorMessage = "An Error Occurred."
                });
            }
        }

        //[RightAuthorization, HttpPost, Route(template: "/Account/Setup/c",
        //     Name = "CompanySetup")]
        //public async Task<IActionResult> CompanySetupAsync(SetupViewModel model,
        //                                                   IFormFile companyLogo)
        //{
        //    try
        //    {
        //        if (model.Company.Id != 0)
        //        {
        //            var res = await _companyService.Edit(token: TOKEN,
        //                                                 user: model.Company,
        //                                                 logoFile: companyLogo);
        //            model.Company = (CompanyDto) res.Model;
        //            model.Company.Response = res;
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        model.Company.Response.ErrorMessage = "An Error Occurred.";
        //    }

        //    return View(viewName: "Setup",
        //                model: model);
        //}


        [JsonResponseAction, RightAuthorization, HttpPost, Route(template: "/Setup/SetupPrinters", Name = "SetupPrinters")]
        public async Task<JsonResult> SetupPrinters(CompanyDto model)
        {
            try
            {
                if (model.Id.HasValue && model.Id != 0)
                {
                    var res = await _companyService.SetupPrinter(token: TOKEN, modelCompany: model);
                    return Json(res);
                }
                return Json(new Response()
                            {
                                ErrorCode = StatusCodesEnum.Error_Occured.ToInt(),ErrorMessage = "CompanyId Can not be null."
                });
            }
            catch (Exception)
            {
                return Json(new Response{ Model = model, ErrorCode = StatusCodesEnum.Error_Occured.ToInt(), ErrorMessage = "An Error Occurred." });
            }
        }
    }
}