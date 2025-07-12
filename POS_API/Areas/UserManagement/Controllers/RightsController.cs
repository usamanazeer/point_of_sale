using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.UserManagement;
using Microsoft.Extensions.Logging;
using POS_API.Utilities.Authentication;
using Microsoft.AspNetCore.Authorization;
using StatusCodesEnums = Models.Enums.StatusCodes;
using POS_API.Services.UserManagement.RightsServices;

namespace POS_API.Areas.UserManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class RightsController : BaseController
    {
        private readonly IRightsService _rightsService;
        public RightsController(ILogger<RightsController> logger, IAuthenticationUtilities authenticationService, IRightsService rightsService)
            :base( logger, authenticationService) => _rightsService = rightsService;

        [HttpPost(nameof(ClaimRight))]
        public async Task<ActionResult> ClaimRight(RoleRightsDto model)
        {
            var response = new Response();
            try
            {
                response = await _rightsService.ClaimRight1(model);
                if (response.Model != null)
                    response.SetMessage("Access Allowed");
                else
                    response.SetMessage("Access Denied", StatusCodesEnums.Method_NotAllowed);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("An error Occurred.");
                return Ok(model);
            }
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(RightsDto model) 
        {
            try
            {
                model.CreatedBy = USER_ID;
                return Ok(await _rightsService.Create(model));
            }
            catch (Exception)
            {
                return BadRequest(model);
            }
        }

        [HttpGet(nameof(GetRightsByRole))]
        public async Task<ActionResult> GetRightsByRole(int? id)
        {
            var response = new Response();
            
            try
            {
                var model = new RoleDto { Id = id, CompanyId = COMPANY_ID };
                var data = (await _rightsService.GetRightsByRole(model)).ToList();
                if (data.Any())
                {
                    response.SetMessage(null,StatusCodesEnums.OK, model:data);
                    return Ok(response);
                }
                response.SetError("No Rights Found");
                return NotFound(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting rights Data.");
                return BadRequest(response);
            }
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? parentId, int? status)
        {
            var response = new Response();
            try
            {
                var model = new CompanyModulesDto { Id = id, Status = status, CompanyId = COMPANY_ID };
                var data = (await _rightsService.GetAll(model)).ToList();
                if (data.Any())
                {
                    response.SetMessage(null,responseCode: StatusCodesEnums.OK, model:data);
                    return Ok(response);
                }
                response.SetMessage("No Rights Found",StatusCodesEnums.Not_Found);
                return NotFound(response);
            }
            catch (Exception )
            {
                response.SetError("Api Error while Getting rights Data.");
                return BadRequest(response);
            }
        }

        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(RightsDto model)
        {
            try
            {
                model.ModifiedBy = USER_ID;
                var res = await _rightsService.Edit(model);
                // ReSharper disable once RedundantCast
                return res != null ? (ActionResult) Ok(res) : NotFound("Role Not Found");
            }
            catch (Exception)
            {
                return BadRequest("An Error Occurred!");
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = new RightsDto { Id = id, ModifiedBy = USER_ID, Status = Models.Enums.StatusTypes.Delete.ToInt() };
                var res = await _rightsService.ChangeStatus(model);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                // ReSharper disable once RedundantCast
                return res ? (ActionResult) Ok(res) : BadRequest("Role Not Found!");
            }
            catch (Exception)
            {
                return BadRequest("An Error Occurred!");
            }
        }

        [HttpPost("Active/{id}")]
        public async Task<ActionResult> Active(int id)
        {
            try
            {
                var model = new RightsDto { Id = id, ModifiedBy = USER_ID, Status = Models.Enums.StatusTypes.Active.ToInt() };
                var res = await _rightsService.ChangeStatus(model);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                // ReSharper disable once RedundantCast
                return res ? (ActionResult) Ok(res) : BadRequest("Role Not Found!");
            }
            catch (Exception)
            {
                return BadRequest("An Error Occurred!");
            }
        }

        [HttpPost("Inactive/{id}")]
        public async Task<ActionResult> InActive(int id)
        {
            try
            {
                var model = new RightsDto{ Id = id, ModifiedBy = USER_ID, Status = Models.Enums.StatusTypes.InActive.ToInt() };
                var res = await _rightsService.ChangeStatus(model);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                // ReSharper disable once RedundantCast
                return res ? (ActionResult) Ok(res) : BadRequest("Role Not Found!");
            }
            catch (Exception)
            {
                return BadRequest("An Error Occurred!");
            }
        }
    }
}
