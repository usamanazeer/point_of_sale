using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.UserManagement;
using Pos_WebApp.Services.UserManagement.RightsServices;
using Pos_WebApp.Services.UserManagement.RoleServices;
using Pos_WebApp.Services.UserManagement.NotificationServices;
using StatusCodesEnums = Models.Enums.StatusCodes;
using Models.DTO.Notifications;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;

namespace Pos_WebApp.Areas.UserManagement.Controllers
{
    [Area("UserManagement"), RightAuthorization, Route("[controller]")]
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IRightsService _rightsService;
        private readonly INotificationService _notificationService;
        public RolesController(IRoleService roleService, IRightsService rightsService, INotificationService notificationService)
        {
            _roleService = roleService;
            _rightsService = rightsService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            var model = new RoleDto();
            try
            {
                //filters
                model.Id = id;
                model.Status = status;
                model.DisplayDeleted = getDeleted??false;

                var response = await _roleService.Get(TOKEN, id, status, getDeleted);

                model.Roles = (List<RoleDto>)response.Model;
                model.Response = response;
            }
            catch (Exception)
            {
                model.Response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                model.Response.ErrorMessage = "An Error Occurred, while loading roles data.";
            }
            return View(model);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var model = new RoleDto();
            try
            {
                var response = await _roleService.GetDetails(TOKEN, id);
                if (response.Model != null)
                {
                    model = (RoleDto)((List<object>)response.Model)[0];
                    model.CompanyModules = (List<CompanyModulesDto>)((List<object>)response.Model)[1];
                }
                model.Response = response;
            }
            catch (Exception)
            {
                model.Response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                model.Response.ErrorMessage = "An Error Occurred, while loading users data.";
            }
            return View(model);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new RoleDto();
            try
            {
                var response = await _rightsService.Get(TOKEN);
                if (response.ResponseCode == StatusCodesEnums.OK.ToInt())
                    model.CompanyModules = (List<CompanyModulesDto>)response.Model;
                ViewBag.NotificationTypes = new SelectList(((await _notificationService.GetNotificationTypes(TOKEN))?? new NotiNotificationTypeDto()).NotificationTypes, "Id", "Name");
            }
            catch (Exception )
            {
                model.Response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                model.Response.ErrorMessage = "An Error Occurred.";
            }
            return View(model);
        }

        [JsonResponseAction, HttpPost("Create")]
        public async Task<JsonResult> Create(RoleDto model)
        {
            var response = new Response();
            try
            {
                //Save Role
                if (ModelState.IsValid)
                {
                    response = await _roleService.Create(model, TOKEN);
                }
                else
                {
                    response.ErrorCode = StatusCodesEnums.Invalid_State.ToInt();
                    response.ErrorMessage = "Please Fill the form care fully.";
                }
                
                response.Model = null;
                return Json(response);
            }
            catch (Exception )
            {
                model.Response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                model.Response.ErrorMessage = "An Error Occurred, while creating role.";
                return Json(response);
            }

        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new RoleDto();
            try
            {
                var response = await _roleService.GetDetails(TOKEN, id);
                if (response.Model != null)
                {
                    //Roles
                    model = (RoleDto)((List<object>)response.Model)[0];
                    model.CompanyModules = (List<CompanyModulesDto>)((List<object>)response.Model)[1];
                    ViewBag.NotificationTypesVals = model.NotiRoleNotification.Select(x => x.NotificationTypeId ?? 0).ToArray();
                    ViewBag.NotificationTypes = new SelectList(((await _notificationService.GetNotificationTypes(TOKEN)) ?? new NotiNotificationTypeDto()).NotificationTypes, "Id", "Name");
                    //get all rights
                }
                
                model.Response = response;
            }
            catch (Exception)
            {
                model.Response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                model.Response.ErrorMessage = "An Error Occurred, while loading role data.";
            }
            return View(model);
        }


        [JsonResponseAction, HttpPost("Edit/{id}")]
        public async Task<JsonResult> Edit(RoleDto model)
        {
            var response = new Response();
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    response = await _roleService.Edit(TOKEN, model);
                return Json(response);
            }
            catch (Exception)
            {
                model.Response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                model.Response.ErrorMessage = "An Error Occurred, while updating role data.";
                return Json(model);
            }
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            var response = new Response();
            try
            {
                response = await _roleService.Delete(TOKEN, id);
                return Json(response);
            }
            catch (Exception)
            {
                response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                response.ErrorMessage = "An Error Occurred, while deleting role.";
                return Json(response);
            }
        }
    }
}