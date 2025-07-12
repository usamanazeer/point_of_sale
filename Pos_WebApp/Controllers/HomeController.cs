using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Pos_WebApp.Attributes;
using Pos_WebApp.Models;

namespace Pos_WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Redirect(url: !SessionExists ? "/Auth/Login" : "/Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [Route(template: "/Home/Error",
            Name = "Error")]
        [ResponseCache(Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error(ErrorViewModel model)
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return View(model: model);
        }

        [HttpGet]
        [Route(template: "/Home/AccessDenied", Name = "AccessDenied")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult AccessDenied(string message,
                                         string backUrl)
        {
            var error = new ErrorViewModel
                        {
                            Response = new Response
                                       {
                                           ResponseMessage = message
                                       },
                            BackURL = backUrl
                        };
            return View(model: error);
        }


        [HttpPost]
        [Route(template: "/NotFound", Name = "NotFound")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NotFound(ErrorViewModel model)

        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return View(model: model);
        }


        [SkipUserAuthentication]
        [JsonResponseAction]
        [HttpGet(template: "/Home/Test")]
        public JsonResult Test()
        {
            return Json(data: new BaseModel
                              {
                                  CreatedOn = DateTime.UtcNow,
                                  ModifiedOn = DateTime.Now
                              });
        }
    }
}