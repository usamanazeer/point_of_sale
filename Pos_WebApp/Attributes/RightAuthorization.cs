using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pos_WebApp.Services.UserManagement.RightsServices;
using Models.DTO.UserManagement;
using Pos_WebApp.Models;
using Pos_WebApp.Controllers;

namespace Pos_WebApp.Attributes
{
    public class RightAuthorization : ActionFilterAttribute
    {
        public string RightName { get; set; }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (((BaseController) filterContext.Controller).SessionExists)
            {
                var rightsService = (IRightsService)filterContext.HttpContext.RequestServices.GetService(typeof(IRightsService));
                var token = filterContext.HttpContext.Session.GetString("token");
                var companyId = filterContext.HttpContext.Session.GetInt32("company_id");
                var roleId = filterContext.HttpContext.Session.GetInt32("role_id");
                
                var model = new RoleRightsDto()
                {
                    Role = new RoleDto(),
                    Right = new RightsDto()
                };
                if (RightName != null)
                {
                    model.Right.Name = RightName;
                }
                else
                {
                    model.Right.Area = filterContext.RouteData.Values["area"].ToString();
                    model.Right.Controller = filterContext.RouteData.Values["controller"].ToString();
                    model.Right.Action = filterContext.RouteData.Values["action"].ToString();
                }

                if (roleId != null) model.RoleId = roleId.Value;
                if (companyId != null) model.CompanyId = companyId.Value;

                var response1 =  rightsService.ClaimRight(token, model);
                if (response1.Model == null)
                {
                    filterContext.Result = new RedirectToActionResult("AccessDenied", "Home",new { message = "Access Denied", backUrl = "" });
                    return;
                }
                if (response1.ErrorOccured)
                {
                    filterContext.Result = new RedirectToActionResult("Error", "Home", new ErrorViewModel() { Response = response1 });
                    return;
                    
                }
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (response1.Model is null)
                {
                    filterContext.Result = new RedirectToActionResult("AccessDenied", "Home", new ErrorViewModel() { Response = response1 });
                    return;
                }
            }
            else
            {
                filterContext.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }
        }

    }
}
