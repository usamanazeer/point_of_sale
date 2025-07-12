using Models;
using System;
using System.Linq;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using StatusCodesEnums = Models.Enums.StatusCodes;
using Pos_WebApp.Services.UserManagement.RoleServices;
using Pos_WebApp.Services.UserManagement.UserServices;


namespace Pos_WebApp.Areas.UserManagement.Controllers
{
    [Area("UserManagement"), RightAuthorization, Route("[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UsersController(IUserService userService, IRoleService roleService) 
        {
            _userService = userService;
            _roleService = roleService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            var model = new UserDto();
            try
            {
                //filters
                model.Id = id;
                model.Status = status;
                model.DisplayDeleted = getDeleted??false;
                var response = await _userService.Get(TOKEN, id, status, getDeleted);
                model.Users = (List<UserDto>)response.Model;
                model.Response = response;
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading users data.");
            }
            return View(model);
        }

        [HttpGet(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            var model = new UserDto();
            try
            {
                var response = await _roleService.Get(TOKEN);
                model.Role.Roles = (List<RoleDto>)response.Model;
                model.Role.Response = response;
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred.");
            }
            return View(model);
        }

        [JsonResponseAction, HttpPost, Route("Create", Name = "CreateUser")]
        public async Task<JsonResult> Create(UserDto model)
        {
            var response1 = new Response();
            try
            {
                //Save User
                if (ModelState.IsValid)
                {
                    response1 = await _userService.Create(model, TOKEN);
                    // ReSharper disable once RedundantAssignment
                    model = (UserDto)response1.Model;
                }
                else
                {
                    response1.SetError("Please Fill the form care fully.", StatusCodesEnums.Invalid_State);
                }
            }
            catch (Exception)
            {
                response1.SetError("An Error Occurred, while creating user.");
            }
            return Json(response1);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var model = new UserDto();
            try
            {
                var response = await _userService.Get(TOKEN, id);
                if (response.Model!=null) model = ((List<UserDto>) response.Model).FirstOrDefault();
                if (model != null) model.Response = response;
            }
            catch (Exception)
            {
                if (model != null) model.Response.SetError("An Error Occurred, while loading user data.");
            }
            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new UserDto();
            try
            {
                var response1 = await _userService.Get(TOKEN, id);
                if (response1.Model != null) model = ((List<UserDto>) response1.Model).FirstOrDefault();
                if (model != null)
                {
                    model.Response = response1;
                    model.Role = new RoleDto();
                    var response2 = await _roleService.Get(TOKEN);
                    model.Role.Roles = (List<RoleDto>) response2.Model;
                    model.Role.Response = response2;
                }
            }
            catch (Exception)
            {
                if (model != null) model.Response.SetError("An Error Occurred, while loading user data.");
            }
            return View(model);
        }

        [JsonResponseAction, HttpPost(nameof(Edit))]
        public async Task<IActionResult> Edit(UserDto model)
        {
            var response1 = new Response();
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                    response1 = await _userService.Edit(TOKEN, model);
                model.Response = response1;
            }
            catch (Exception)
            {
                response1.SetError("An Error Occurred, while updating user data.");
            }
            return Json(response1);
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            var response = new Response();
            try
            {
                response = await _userService.Delete(TOKEN, id);
                return Json(response);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while deleting vendor.");
                return Json(response);
            }

        }
    }
}
