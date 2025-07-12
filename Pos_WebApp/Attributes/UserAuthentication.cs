using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Newtonsoft.Json;
using Pos_WebApp.Controllers;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Attributes
{
    public class UserAuthentication : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Filters.OfType<SkipUserAuthentication>().Any()) return;
            base.OnActionExecuting(context: filterContext);
            
            if (((BaseController) filterContext.Controller).SessionExists)
            {
                if (!filterContext.Filters.OfType<JsonResponseAction>().Any() && !filterContext.Filters.OfType<SkipSideBar>().Any())
                {
                    var rightsString = filterContext.HttpContext.Session.GetString(key: "rightsList");
                    var rights = JsonConvert.DeserializeObject<List<RightModel>>(rightsString);
                    var rightsDict = new Dictionary<string, bool>();
                    foreach (var right in rights)
                        rightsDict.TryAdd(key: right.Name, value: true);

                    filterContext.HttpContext.Items.TryAdd(key: "rights", value: rightsDict);
                }
               
            }
            else
            {
                if (filterContext.Filters.OfType<JsonResponseAction>().Any())
                {
                    filterContext.Result = ((BaseController) filterContext.Controller)
                        .Json(new Response { ErrorMessage = "Session Expired.", ErrorCode = Convert.ToInt32(StatusCodesEnums.Session_Expired)});
                    return;
                }
                filterContext.Result = new RedirectToActionResult(actionName: "Login", controllerName: "Auth", routeValues: null);
            }
        }
    }
}