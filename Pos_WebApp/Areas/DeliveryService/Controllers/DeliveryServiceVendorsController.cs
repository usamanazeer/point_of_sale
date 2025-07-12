using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.DeliveryService;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using Pos_WebApp.Services.DeliveryService.DeliveryServiceVendorServices;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Areas.DeliveryService.Controllers
{
    [Area(areaName: "DeliveryService"), RightAuthorization, Route(template: "[controller]")]
    public class DeliveryServiceVendorsController : BaseController
    {
        private readonly IDeliveryServiceVendorService _deliveryServiceVendorService;

        public DeliveryServiceVendorsController(IDeliveryServiceVendorService deliveryServiceVendorService) => _deliveryServiceVendorService = deliveryServiceVendorService;

        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            var model = new DeliDeliveryServiceVendorDto();
            try
            {
                //filters
                model.Id = id;
                model.Status = status;
                if (getDeleted.HasValue) model.DisplayDeleted = getDeleted.Value;
                model = await _deliveryServiceVendorService.Get(token: TOKEN, model: model);
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading Delivery Service Vendors.");
            }

            return View(model: model);
        }

        [HttpGet(template:nameof(Create))]
        public async Task<IActionResult> Create()
        {
            try
            {
                var isSelExistResponse = await _deliveryServiceVendorService.IsSelfExist(token: TOKEN);
                ViewBag.IsSelfExist = (bool) isSelExistResponse.Model;
            }
            catch
            {
                // ignored
            }
            return View(model: new DeliDeliveryServiceVendorDto());
        }


        [HttpPost, Route(template: nameof(Create), Name = "CreateDeliveryServiceVendor")]
        public async Task<IActionResult> Create(DeliDeliveryServiceVendorDto model)
        {
            var response = new Response();
            try
            {
                //Save Vendor
                if (ModelState.IsValid)
                    response = await _deliveryServiceVendorService.Create(token: TOKEN, model: model);
                else
                    response.SetError("Please Fill the form carefully.", StatusCodesEnums.Invalid_State);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while creating Delivery Service Vendor.", StatusCodesEnums.Error_Occured);
            }
            return Json(data: response);
        }


        [HttpGet(template: "Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var model = new DeliDeliveryServiceVendorDto();
            try
            {
                //filters
                model.Id = id;
                var responseModel = await _deliveryServiceVendorService.Details(token: TOKEN, id: id);
                if (responseModel != null)
                {
                    model = responseModel;
                    model.Response = responseModel.Response;
                }
                else
                {
                    switch (model.Response.ResponseCode)
                    {
                        case (int)StatusCodesEnums.Not_Found: 
                            model.Response.ResponseMessage = "Delivery Service Vendor Not Found.";
                            return NotFound(response: model.Response, backUrl: "/DeliveryServiceVendors");
                        default: 
                            return Error(response: model.Response, backUrl: "/DeliveryServiceVendors");
                    }
                }
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading Delivery Service Vendor data.");
                return Error(response: model.Response, backUrl: "/DeliveryServiceVendors");
            }
            return View(model: model);
        }

        [HttpGet(template: "Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new DeliDeliveryServiceVendorDto();
            try
            {
                model.Id = id;
                var responseModel = _deliveryServiceVendorService.Details(token: TOKEN, id: id);
                var isSelExistResponse = _deliveryServiceVendorService.IsSelfExist(token: TOKEN);
                await Task.WhenAll(tasks: new List<Task> { responseModel, isSelExistResponse });
                model = responseModel.Result;
                ViewBag.IsSelfExist = (bool) isSelExistResponse.Result.Model;

                if (responseModel.Result.Id is null && model.Response.ResponseCode ==  StatusCodesEnums.Not_Found.ToInt())
                {
                    model.Response.ResponseMessage = "Delivery Service Vendor Not Found.";
                    return NotFound(response: model.Response,
                                    backUrl: "/DeliveryServiceVendors");
                }
                if (responseModel.Result.Id is null)
                    return Error(response: model.Response, backUrl: "/DeliveryServiceVendors");
            }
            catch (Exception)
            {
                model.Response.SetError("An Error Occurred, while loading Delivery Service Vendor data.");
                return Error(response: model.Response, backUrl: "/DeliveryServiceVendors");
            }
            return View(model: model);
        }


        [HttpPost(template: nameof(Edit))]
        public async Task<IActionResult> Edit(DeliDeliveryServiceVendorDto model)
        {
            var response = new Response();
            try
            {
                if (ModelState.IsValid && model.Id > 0)
                {
                    response = await _deliveryServiceVendorService.Edit(token: TOKEN, model: model);
                    if (response.ResponseCode == StatusCodes.Status404NotFound)
                        response.SetError("Delivery Service Vendor Not Found.");
                }
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while updating Delivery Service Vendor data.", StatusCodesEnums.Error_Occured);
            }
            return Json(data: response);
        }


        [JsonResponseAction, HttpGet(template: "Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            var response = new Response();
            try
            {
                return Json(data: await _deliveryServiceVendorService.Delete(token: TOKEN, id: id));
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while deleting Delivery Service Vendor.");
                return Json(data: response);
            }
        }
    }
}