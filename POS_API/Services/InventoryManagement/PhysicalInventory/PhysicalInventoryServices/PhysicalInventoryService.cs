using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Models.DTO.Notifications;
using StatusCodesEnums = Models.Enums.StatusCodes;
using Models.DTO.InventoryManagement;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using Microsoft.Extensions.Configuration;
using POS_API.Repositories.InventoryManagement.PhysicalInventoryRepos;

namespace POS_API.Services.InventoryManagement.PhysicalInventory.PhysicalInventoryServices
{
    public class PhysicalInventoryService : IPhysicalInventoryService, IService
    {
        private readonly IConfiguration _configuration;
        private readonly IPhysicalInventoryRepository _physicalInventoryRepository;
        //private readonly IStockNotificationManager _stockNotificationManager;
        public PhysicalInventoryService(IPhysicalInventoryRepository physicalInventoryRepository, IConfiguration configuration  /*,IStockNotificationManager stockNotificationManager*/)
        {
            _configuration = configuration;
            _physicalInventoryRepository = physicalInventoryRepository;
            //_stockNotificationManager = stockNotificationManager;
        }

        public async Task<Response> Add(InvPhysicalInventoryDto model)
        {
            var isExists = await IsExist(model);
            if (isExists)
                return Response.Error($"Inventory against Bill No: '{model.BillNo}' Already Exists.", model: model);
            
            var res = await _physicalInventoryRepository.AddPhysicalInventory(model);
            // ReSharper disable once UnusedVariable
            //var notificationsList = model.InvPhysicalInventoryItem.Select(x => x.ItemId != null ? new NotiNotificationDto( x.ItemId.Value, model.CompanyId) : null).ToList();
            //_ = _stockNotificationManager.RemoveLowInventoryNotifications(notificationsList).ConfigureAwait(false);

            res.InvPhysicalInventoryItem = null;
            return  Response.Message("Inventory Added Successfully.", StatusCodesEnums.Created, res);
        }

        public async Task<Response> GetAll(InvPhysicalInventoryDto model)
        {
            var res = await _physicalInventoryRepository.GetAll(model);
            return res.Any() ? Response.Message(null,model:model) : Response.Message("No Bill Found", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetBillDetails(PhysicalInventoryViewFilter filter)
        {
            var response = new Response();
            if (filter.Id is null) throw new Exception("Bill Id is Null.");
            var model = new InvPhysicalInventoryViewDto
            { BillId = filter.Id.Value };
            var res = await _physicalInventoryRepository.GetPhysicalInventory_View(filter);
            if (res != null)
            {
                if (res.Any())
                {
                    model = res[0].Clone();
                    model.PhysicalInventoryViews = res;
                }
                response.SetMessage(null, model: model);
            }
            else
            {
                response.SetError("Bill Not Found", StatusCodesEnums.Not_Found);
                response.SetMessage("Bill Not Found", StatusCodesEnums.Not_Found);
            }
            return response;
        }

        public async Task<Response> GetLowInventory(PhysicalInventoryViewFilter filters = null)
        {
            var response = new Response();
            var res = await _physicalInventoryRepository.GetLowInventory(filters);
            if (res.Any())
            {
                response.SetMessage(null, model: res);
            }
            else
            {
                response.ResponseName = "NotFound";
                response.SetMessage("No Low Inventory Found.", StatusCodesEnums.Not_Found);
            }
            return response;
        }

        public async Task<Response> GetPhysicalInventory_View(PhysicalInventoryViewFilter filters = null)
        {
            var host = _configuration.GetSection(Paths.AppSettings_ApiHost).Value;
            var response = new Response();
            var res = await _physicalInventoryRepository.GetPhysicalInventory_View(filters);
            foreach (var item in res)
                item.ItemImageUrl = item.ItemImageUrl != null ? host + item.ItemImageUrl : host + Paths.DEFAULT_IMAGE;
            if (res.Any())
            {
                response.SetMessage(null, model:res);
            }
            else
            {
                response.ResponseName = "NotFound";
                response.SetMessage("No Inventory Found.", StatusCodesEnums.Not_Found);
            }
            return response;
        }

        public async Task<bool> IsExist(InvPhysicalInventoryDto model) => await _physicalInventoryRepository.IsPhysicalInventoryExists(model);
    }
}