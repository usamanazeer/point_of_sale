using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using POS_API.Utilities.Authentication;
using Microsoft.AspNetCore.Authorization;
using StatusCodesEnums = Models.Enums.StatusCodes;
using POS_API.Services.UserManagement.RolesServices;
using POS_API.Services.UserManagement.RightsServices;

namespace POS_API.Areas.UserManagement.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class RolesController : BaseController
    {
        private readonly IRolesService _rolesService;
        private readonly IRightsService _rightsService;
        public RolesController(ILogger<RolesController> logger, IRolesService rolesService, IRightsService rightsService, IAuthenticationUtilities authenticationService) 
            :base(logger, authenticationService) { _rolesService = rolesService; _rightsService = rightsService; }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(RoleDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.CreatedBy = USER_ID;
                model.CreatedOn = DateTime.Now;
                var check = await _rolesService.IsExists(model);
                if (!check)
                {
                    var resData = await _rolesService.Create(model);
                    response.SetMessage("Role Created Successfully.", StatusCodesEnums.Created, resData);
                    return StatusCode(StatusCodesEnums.Created.ToInt(), response);
                }
                response.SetError("Role Already Exists.", model:model);
                return StatusCode(StatusCodesEnums.Conflict.ToInt(), response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Creating Role.");
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }
        [HttpPost(nameof(Edit))]
        public async Task<ActionResult> Edit(RoleDto model)
        {
            var response = new Response();
            try
            {
                model.CompanyId = COMPANY_ID;
                model.ModifiedBy = USER_ID;
                model.ModifiedOn = DateTime.Now;
                var check = await _rolesService.IsExists(model);
                if (!check)
                {
                    //save role data
                    response.Model = await _rolesService.Edit(model);
                    if (response.Model != null)
                        response.SetMessage("Role Updated Successfully.", StatusCodesEnums.Updated);
                    else 
                        response.SetError("Role Not Found.");
                    return Ok(response);
                }
                response.SetError("Role Already Exists.", model:model);
                return StatusCode(StatusCodesEnums.Conflict.ToInt(), response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Updating Role.");
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }
        
        [HttpGet, Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = new Response();
            try
            {
                var model = new RoleDto { Id = id, CompanyId = COMPANY_ID, ModifiedBy = USER_ID, ModifiedOn = DateTime.Now };
                if (await _rolesService.Delete(model))
                    response.SetMessage("Role Deleted Successfully.", StatusCodesEnums.OK, true);
                else
                    response.SetMessage("Role Not Found.", StatusCodesEnums.Not_Found, false);
                return Ok(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Deleting Role.", model:false);
                return StatusCode(StatusCodesEnums.Error_Occured.ToInt(), response);
            }
        }

        [Route("Active/{id}"), HttpGet]
        public async Task<ActionResult> Active(int id)
        {
            try
            {
                return Ok(await _rolesService.Active(id));
            }
            catch (Exception)
            {
                return BadRequest(id);
                //_logger.LogError(new EventId() { Name = "Register" });
            }
        }

        [Route("InActive/{id}"), HttpGet]
        public async Task<ActionResult> InActive(int id)
        {
            try
            {
                return Ok(await _rolesService.InActive(id));
            }
            catch (Exception)
            {
                return BadRequest(id);
                //_logger.LogError(new EventId() { Name = "Register" });
            }
        }
        [HttpGet(nameof(Get))]
        public async Task<ActionResult> Get(int? id, int? status = null, bool? getDeleted = null)
        {
            var response = new Response();
            try
            {
                var model = new RoleDto { Id = id, Status = status, DisplayDeleted = getDeleted ?? false, CompanyId = COMPANY_ID };
                var data = (await _rolesService.GetAll(model)).ToList();
                if (data.Any())
                {
                    response.Model = data;
                    return Ok(response);
                }
                response.SetMessage("No Role Found", StatusCodesEnums.Not_Found);
                return NotFound(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting roles Data.");
                return BadRequest(response);
            }
        }
        [HttpGet("GetDetails/{id}")]
        public async Task<ActionResult> GetDetails(int id)
        {
            var response = new Response();
            try
            {
                var model = new RoleDto { Id = id, CompanyId = COMPANY_ID };
                var rolesData = (await _rolesService.GetAll(model)).ToList().FirstOrDefault() ;
                var rightsModel = new CompanyModulesDto {CompanyId = COMPANY_ID};
                rightsModel.CompanyModules = (await _rightsService.GetAll(rightsModel)).ToList();
                //mark which are assigned.
                if (rolesData!=null)
                {
                    for (int i = 0; i < rolesData.RoleRights.Count; i++)
                    {
                        var markedCheck = false;
                        foreach (var module in rightsModel.CompanyModules)
                        {
                            foreach (var right in module.Module.Rights)
                                if (right.Id == rolesData.RoleRights.ToList()[i].RightId)
                                {
                                    right.IsSelected = true;
                                    module.Module.IsSelected = true;
                                    markedCheck = true;
                                    break;
                                }

                            if (markedCheck)
                                break;
                        }
                    }
                }
                if (rolesData!= null)
                {
                    response.Model = new List<object> { rolesData, rightsModel.CompanyModules };
                    return Ok(response);
                }
                response.SetError("No Role Found", StatusCodesEnums.Not_Found);
                return NotFound(response);
            }
            catch (Exception)
            {
                response.SetError("Api Error while Getting roles Data.");
                return BadRequest(response);
            }
        }
    }
}