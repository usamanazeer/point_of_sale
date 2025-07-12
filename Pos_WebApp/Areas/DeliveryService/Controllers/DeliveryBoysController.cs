using System;
using Models;
using Pos_WebApp.Attributes;
using Pos_WebApp.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.DeliveryService;
using StatusCodesEnums = Models.Enums.StatusCodes;
using Pos_WebApp.Services.DeliveryService.DeliveryBoyServices;

namespace Pos_WebApp.Areas.DeliveryService.Controllers
{
    [Area(areaName: "DeliveryService"), RightAuthorization, Route(template: "[controller]")]
    public class DeliveryBoysController : BaseController
    {
        private readonly IDeliveryBoyService _deliveryBoyService;

        public DeliveryBoysController(IDeliveryBoyService deliveryBoyService) => _deliveryBoyService = deliveryBoyService;

        public async Task<IActionResult> Index(int? id, int? status, bool? getDeleted = false)
        {
            var deliveryBoyDto = new DeliDeliveryBoyDto();
            try
            {
                //filters
                deliveryBoyDto.Id = id;
                deliveryBoyDto.Status = status;
                if (getDeleted != null) deliveryBoyDto.DisplayDeleted = (bool) getDeleted;
                deliveryBoyDto = await _deliveryBoyService.Get(token: TOKEN, model: deliveryBoyDto);
            }
            catch (Exception)
            {
                deliveryBoyDto.Response.ErrorCode = StatusCodesEnums.Error_Occured.ToInt();
                deliveryBoyDto.Response.ErrorMessage = "An Error Occurred, while loading Delivery Boys.";
            }

            return View(model: deliveryBoyDto);
        }


        [HttpGet(template: nameof(Create))]
        public IActionResult Create()=> View(model: new DeliDeliveryBoyDto());


        [JsonResponseAction, HttpPost, Route(template: nameof(Create), Name = "CreateDeliveryBoy")]
        public async Task<IActionResult> Create(DeliDeliveryBoyDto deliveryBoyDto)
        {
            var response = new Response();
            try
            {
                //Save Vendor
                if (ModelState.IsValid)
                    response = await _deliveryBoyService.Create(token: TOKEN, model: deliveryBoyDto);
                else
                    response.SetError("Please Fill the form carefully.", StatusCodesEnums.Invalid_State);
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while creating Delivery Boy.");
            }
            return Json(response);
        }

        [HttpGet(template: "Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var deliveryBoyDto = new DeliDeliveryBoyDto();
            try
            {
                //filters
                deliveryBoyDto.Id = id;
                var responseModel = await _deliveryBoyService.Details(token: TOKEN, id: id);
                if (responseModel != null)
                {
                    deliveryBoyDto = responseModel;
                    deliveryBoyDto.Response = responseModel.Response;
                }
                else
                {
                    if (deliveryBoyDto.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                    {
                        deliveryBoyDto.Response.ResponseMessage = "Delivery Boy Not Found.";
                        return NotFound(response: deliveryBoyDto.Response, backUrl: "/DeliveryBoys");
                    }
                    return Error(response: deliveryBoyDto.Response, backUrl: "/DeliveryBoys");
                }
            }
            catch (Exception)
            {
                deliveryBoyDto.Response.SetError("An Error Occurred, while loading Delivery Boy data.");
                return Error(response: deliveryBoyDto.Response, backUrl: "/DeliveryBoys");
            }
            return View(model: deliveryBoyDto);
        }

        [HttpGet(template: "Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var deliveryBoyDto = new DeliDeliveryBoyDto();
            try
            {
                deliveryBoyDto.Id = id;
                deliveryBoyDto = await _deliveryBoyService.Details(token: TOKEN,id: id);
                if (deliveryBoyDto.Id is null && deliveryBoyDto.Response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                {
                    deliveryBoyDto.Response.ResponseMessage = "Delivery Boy Not Found.";
                    return NotFound(response: deliveryBoyDto.Response, backUrl: "/DeliveryBoys");
                }
                if (deliveryBoyDto.Id is null) return Error(response: deliveryBoyDto.Response, backUrl: "/DeliveryBoys");
            }
            catch (Exception)
            {
                deliveryBoyDto.Response.SetError($"An Error Occurred, while loading Delivery Boy data.");
                return Error(response: deliveryBoyDto.Response, backUrl: "/DeliveryBoys");
            }
            return View(model: deliveryBoyDto);
        }


        [JsonResponseAction, HttpPost(template: nameof(Edit))]
        public async Task<IActionResult> Edit(DeliDeliveryBoyDto deliveryBoyDto)
        {
            var response = new Response();
            try
            {
                if (ModelState.IsValid && deliveryBoyDto.Id > 0)
                {
                    response = await _deliveryBoyService.Edit(token: TOKEN, model: deliveryBoyDto);
                    if (response.ResponseCode == StatusCodesEnums.Not_Found.ToInt())
                        response.SetError("Delivery Boy Not Found.");
                }
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while updating Delivery Boy data.", StatusCodesEnums.Error_Occured);
            }
            return Json(response);
        }

        [JsonResponseAction, HttpGet(template: "Delete/{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            var response = new Response();
            try
            {
                return Json(data: await _deliveryBoyService.Delete(token: TOKEN, id: id));
            }
            catch (Exception)
            {
                response.SetError("An Error Occurred, while deleting Delivery Boy.");
                return Json(data: response);
            }
        }
    }
}