using Microsoft.AspNetCore.Mvc;

namespace Pos_WebApp.Areas.GeneralSettings.Controllers
{
    public class PrintersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
