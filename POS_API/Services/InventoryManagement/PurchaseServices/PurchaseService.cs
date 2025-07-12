using System.Linq;
using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;
using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.Notifications;
using POS_API.Repositories.InventoryManagement.PurchaseRepositories;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace POS_API.Services.InventoryManagement.PurchaseServices
{
    public class PurchaseService:IPurchaseService, IService
    {
        //private readonly IConfiguration _configuration;
        private readonly IPurchaseRepository _purchaseRepository;
        //private readonly IStockNotificationManager _stockNotificationManager;
        public PurchaseService(/*IConfiguration configuration,*/ IPurchaseRepository purchaseRepository  /*,IStockNotificationManager stockNotificationManager*/) =>
            //_configuration = configuration;
            _purchaseRepository = purchaseRepository;
        //_stockNotificationManager = stockNotificationManager;


        public async Task<Response> Create(InvPurchaseMasterDto purchaseMasterDto)
        {
            var isExists = await IsExist(purchaseMasterDto);
            if (isExists)
                return Response.Error($"Purchase against Bill No: '{purchaseMasterDto.BillNo}' Already Exists.", model: purchaseMasterDto);

            purchaseMasterDto.BillAmount = purchaseMasterDto.InvPurchaseDetail.Sum(x => x.Quantity * x.PurchaseRate);
            var res = await _purchaseRepository.Create(purchaseMasterDto);
            // ReSharper disable once UnusedVariable
            var notificationsList = purchaseMasterDto.InvPurchaseDetail.Select(x => new NotiNotificationDto(x.ItemId, purchaseMasterDto.CompanyId)).ToList();
            //_ = _stockNotificationManager.RemoveLowInventoryNotifications(notiNotifications).ConfigureAwait(false);

            res.InvPurchaseDetail = null;
            return Response.Message("Purchase Created Successfully.", StatusCodesEnums.Created, model:res);
        }

        public async Task<Response> GetAll(InvPurchaseMasterDto purchaseMasterDto)
        {
            var res = await _purchaseRepository.GetAll(purchaseMasterDto);
            return res.Any() ? Response.Message(null, model:res) : Response.Message("No Purchases Found.", StatusCodesEnums.Not_Found);
        }

        public async Task<Response> GetDetails(InvPurchaseMasterDto model)
        {
            var res = await _purchaseRepository.GetDetails(purchaseMasterDto: model);
            return res != null ? Response.Message(null, model: res) : Response.Message("Purchase Not Found", StatusCodesEnums.Not_Found);
        }

        private async Task<bool> IsExist(InvPurchaseMasterDto purchaseMasterDto) => await _purchaseRepository.IsExist(purchaseMasterDto);
    }
}
