using Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.UserManagement;
using Microsoft.AspNetCore.Http ;
using System.Collections.Generic;
using StatusCodesEnums = Models.Enums.StatusCodes;
using Pos_WebApp.Services.UserManagement.RightsServices;

namespace Pos_WebApp.ViewComponents
{
    public class UserMenu : ViewComponent
    {
        private readonly IRightsService _rightsService;
        public UserMenu(IRightsService rightsService) => _rightsService = rightsService;
        public async Task<IViewComponentResult> InvokeAsync(IList<RightModel> rights)
        {
            if (rights != null) return View(model: rights);
            var token = HttpContext.Session.GetString(key: "token");
            // ReSharper disable once PossibleInvalidOperationException
            var roleId = HttpContext.Session.GetInt32(key: "role_id").Value;
            var right = HttpContext.Session.GetString(key: "rightName");
            ViewBag.ActiveRight = right;
            var model = await _rightsService.GetRightsByRole(token: token, roleId: roleId);
            rights = model.Rights ?? new List<RightModel>();
            return View(model: rights);
        }
    }
}