using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.UserManagement;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.UserManagement.BranchServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.UserManagement.Controllers
{
    [Area("UserManagement"), RightAuthorization, Route("[controller]")]
    public class BranchesController : BaseController
    {
        private readonly IBranchService _branchService;
        public BranchesController(IBranchService branchService) => _branchService = branchService;

        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            var model = new BranchDto();
            try
            {
                //filters
                model.Id = id;
                model.Status = status;
                model.DisplayDeleted = getDeleted??false;
                model = await _branchService.Get(TOKEN, model);
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading branches.");
                return Error(model.Response, "/Branches");
            }
            return View(model);
        }

        [HttpGet(nameof(Create))]
        public IActionResult Create() => View(new BranchDto());


        [HttpPost, Route("Create", Name = "CreateBranch")]
        public async Task<IActionResult> Create(BranchDto model)
        {
            try
            {
                //Save branch
                if (ModelState.IsValid)
                    model = await _branchService.Create(TOKEN, model);
                else
                    model.Response.SetError("Please Fill the form care fully.", StatusCodesEnums.Invalid_State);
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while creating branch.");
                return Error(model.Response, "/Branches");
            }
            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new BranchDto();
            try
            {
                model.Id = id;
                var responseModel = await _branchService.Details(TOKEN, id);
                model = responseModel;
                if (!responseModel.Id.HasValue)
                {
                    if (model.Response.ResponseCode == StatusCodes.Status404NotFound)
                    {
                        model.Response.ResponseMessage = "Branch Not Found.";
                        return NotFound(model.Response, "/Branches");
                    }
                    return Error(model.Response, "/Branches");
                }
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading branch data."); 
                return Error(model.Response, "/Branches");
            }
            return View(model);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(BranchDto model)
        {
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                {
                    model = await _branchService.Edit(TOKEN, model);
                    if (model.Response.ResponseCode == StatusCodes.Status404NotFound)
                    {
                        model.Response.ResponseMessage = "Branch Not Found.";
                        return NotFound(model.Response, "/Branches");
                    }
                    if (model.Response.ErrorCode == StatusCodesEnums.Error_Occured.ToInt())
                        return Error(model.Response, "/Branches");
                }
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while updating branch data.");
                return Error(model.Response, "/Branches");
            }
            ModelState.Clear();
            return View(model);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var model = new BranchDto();
            try
            {
                var responseModel = await _branchService.Details(TOKEN, id);
                model = responseModel;
                if (responseModel.Id is null)
                {
                    if (model.Response.ResponseCode == StatusCodes.Status404NotFound)
                    {
                        model.Response.ResponseMessage = "Branch Not Found.";
                        return NotFound(model.Response, "/Branches");
                    }
                    return Error(model.Response, "/Branches");
                }
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading modifier data.");
                return Error(model.Response, "/Branches");
            }
            return View(model);
        }

        [JsonResponseAction, HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            var response = new Response();
            try
            {
                response = await _branchService.Delete(TOKEN, id);
                return Json(response);
            }
            catch (Exception )
            {
                response.SetError("An Error Occurred, while deleting branch.");
                return Json(response);
            }
        }
    }
}