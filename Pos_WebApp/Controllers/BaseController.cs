using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Pos_WebApp.Models;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected string IndexUrl;
        public BaseController()
        {}
        public BaseController(string indexUrl) => IndexUrl = indexUrl;

        public bool SessionExists => HttpContext.Session.TryGetValue(key: "id", value: out _);

        // ReSharper disable once InconsistentNaming
        public string TOKEN => GetSessionString(key: "token");
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public int COMPANY_ID => GetSessionInt(key: "company_id");
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public int USER_ID => GetSessionInt(key: "id");
        // ReSharper disable once InconsistentNaming
        public int ROLE_ID => GetSessionInt(key: "role_id");


        protected void ModalStateWatch()
        {
            //List<ModelStateEntry> invalidStates = ViewData.ModelState.Values.Where(x => x.ValidationState == ModelValidationState.Invalid).ToList();
            foreach (var modelState in ViewData.ModelState.Values)
            foreach (var error in modelState.Errors)
            {
                var error1 = error;
            }
        }


        private string GetSessionString(string key)
        {
            return HttpContext.Session.GetString(key: key);
        }


        private int GetSessionInt(string key)
        {
            return HttpContext.Session.GetInt32(key: key)??0;
        }


        protected IActionResult NotFound(Response response, string backUrl)
        {
            return View(viewName: "NotFound",
                        model: new ErrorViewModel
                               {
                                   Response = response,
                                   BackURL = backUrl
                        });
        }


        protected IActionResult Error(Response response = null,
                                      string backUrl = null,
                                      string requestID = null)
        {
            return View(viewName: "Error",
                        model: new ErrorViewModel
                               {
                                   RequestId = requestID,
                                   Response = response,
                                   BackURL = backUrl
                        });
        }


        protected IActionResult LicenseExpiredOrInvalid(bool isInvalid = false)
        {
            ViewBag.isLicenseInvalid = isInvalid;
            return View(viewName: "LicenseExpired");
        }

        /// <summary>
        /// Use only in Get=>Details and Get=>Edit Views
        /// </summary>
        /// <param name="model">model to bind with view, must have Response Property</param>
        /// <returns></returns>
        protected IActionResult GetView(dynamic model /*,string viewName*/)
        {
            if (model.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                return NotFound(model.Response, IndexUrl);

            if (model.Response.ErrorOccured)
                return Error(model.Response, IndexUrl);

            return View( model);
        }
    }
}