using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.UserManagement;
using Newtonsoft.Json;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Models;
using Pos_WebApp.Services.UserManagement.RightsServices;
using Pos_WebApp.Services.UserManagement.UserServices;
using StatusCodeEnum = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.UserManagement.Controllers
{
    [Area(areaName: "UserManagement"), SkipUserAuthentication, Route(template: "[controller]")]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRightsService _rightsService;


        public AuthController(IUserService userService, IRightsService rightsService)
        {
            _userService = userService;
            _rightsService = rightsService;
        }


        [HttpGet, Route(template: "Login", Name = "Login")]
        public IActionResult Login()=> SessionExists ? (IActionResult)RedirectToAction(actionName: "Index", controllerName: "Home") : View(model: new AuthenticationDto());


        [HttpPost, Route(template: "Login", Name = "Login")]
        public async Task<IActionResult> LoginAsync(AuthenticationDto authModel)
        {
            try
            {
                var data = await _userService.Login(user: authModel);
                if (data.ResponseCode == StatusCodeEnum.OK.ToInt())
                {
                    await SetSession(data: (AuthenticationDto) data.Model);
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                }

                if (data.ErrorCode == StatusCodeEnum.Error_Occured.ToInt()) return Error(response: data);
                authModel.Response = data;
                // ReSharper disable once Mvc.ViewNotResolved
                return View(model: authModel);
            }
            catch (Exception ex)
            {
                return Error(requestID: ex.Message);
            }
        }


        [HttpGet, Route(template: "Logout"), ActionName(name: "Logout")]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(actionName: "Login");
        }


        private async Task SetSession(AuthenticationDto data)
        {
            var model = data.User;
            if (model.Id.HasValue)
                HttpContext.Session.SetInt32(key: "id", value:model.Id.Value);

            HttpContext.Session.SetString(key: "user_name", value: data.UserName);
            HttpContext.Session.SetString(key: "full_name", value: model.FirstName + " " + model.LastName);
            HttpContext.Session.SetString(key: "user_name", value: data.UserName);
            HttpContext.Session.SetInt32(key: "company_id", value: model.CompanyId);
            if (model.RoleId.HasValue)
                HttpContext.Session.SetInt32(key: "role_id", value:model.RoleId.Value);
            HttpContext.Session.SetString(key: "token", value: data.Token);



            //load menu
            var rights = (await _rightsService.GetRightsByRole(token: TOKEN, roleId: ROLE_ID)).Rights;
            string rightsString = JsonConvert.SerializeObject(rights);
            HttpContext.Session.SetString(key: "rightsList", value: rightsString);
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedMethodReturnValue.Local
        private IActionResult TestLogin()
        {
            var authModel = new AuthenticationDto
                            {
                                // ReSharper disable once StringLiteralTypo
                                UserName = "superadmin",
                                Password = "12345"
                            };
            return LoginAsync(authModel: authModel).Result;
        }
    }
}