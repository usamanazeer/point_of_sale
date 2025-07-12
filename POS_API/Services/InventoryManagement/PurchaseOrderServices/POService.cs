using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using POS_API.Services.UserManagement.CompanyServices;
using POS_API.Repositories.InventoryManagement.PurchaseOrderRepos;
using static Models.Enums.StatusCodes;

namespace POS_API.Services.InventoryManagement.PurchaseOrderServices
{
    // ReSharper disable once InconsistentNaming
    public class POService : IPOService, IService
    {
        private readonly IPORepository _pORepository;
        private readonly ICompanyService _companyService;
        public POService(IPORepository pORepository, ICompanyService companyService)
        {
            _pORepository = pORepository;
            _companyService = companyService;
        }
        
        public async Task<Response> Create(InvPoMasterDto model)
        {
            //var isExists = await this.IsExist(model);
            //if (!isExists)
            //{

                if (model.InvPoDetails != null)
                {
                    //removing extra entries from recipe items
                    model.InvPoDetails = model.InvPoDetails.Where(x => x.Status == StatusTypes.Active.ToInt() && x.ItemId > 0 && x.RequestedQuantity > 0)
                         .GroupBy(x => new { x.ItemId, x.Rate }).Select(poDetail => new InvPoDetailsDto
                         {
                             ItemId = poDetail.First().ItemId,
                             Rate = poDetail.First().Rate,
                             Status = poDetail.First().Status,
                             RequestedQuantity = poDetail.Sum(x => x.RequestedQuantity),
                         }).ToList();
                }
                var res = await _pORepository.Create(model);
                return Response.Message("PO Created Successfully." , Created, res);
            //}
            //else
            //{
            //    response.Model = model;
            //    response.ErrorCode = ErrorOccured.ToInt();
            //    response.ErrorMessage = "PO Already Exists.";
            //    return response;
            //}
        }

        public async Task<Response> Delete(InvPoMasterDto model)
        {
            return await _pORepository.Delete(model) ? Response.Message("PO Deleted Successfully.", model: true)
                : Response.Message("PO Not Found.", Not_Found, false);
        }

        public async Task<Response> Edit(InvPoMasterDto model)
        {
            //var isExists = await this.IsExist(model);
            //if (!isExists)
            //{
                if (model.InvPoDetails != null)
                {
                    //removing extra entries from recipe items
                    model.InvPoDetails = model.InvPoDetails.Where(x => x.Status == StatusTypes.Active.ToInt() && x.ItemId > 0 && x.RequestedQuantity > 0)
                      .GroupBy(x => new { x.ItemId, x.Rate }).Select(poDetail =>
                      {
                          if (model.Id != null)
                              return new InvPoDetailsDto
                             { 
                                 PoId = model.Id.Value, ItemId = poDetail.First().ItemId, Rate = poDetail.First().Rate,
                                 CompanyId = model.CompanyId, CreatedBy = model.ModifiedBy, CreatedOn = DateTime.Now,
                                 Status = StatusTypes.Active.ToInt(), RequestedQuantity = poDetail.Sum(x => x.RequestedQuantity),
                             };
                          return null;
                      }).ToList();
            }
            var res = await _pORepository.Edit(model);
            return res != null ?
                Response.Message("PO Updated Successfully.",Updated, model: res) :
                Response.Message("PO Not Found.", Not_Found, model: model);
        }

        public async Task<Response> GetAll(InvPoMasterDto model)
        {
            var res = await _pORepository.GetAll(model: model);
            return res.Any() ? Response.Message(null, model: res) : Response.Message("PO Not Found.", Not_Found);
        }

        public async Task<Response> GetDetails(InvPoMasterDto model)
        {
            var res = await _pORepository.GetDetails(model: model);
            if (res == null)
                return Response.Message("PO Not Found.", Not_Found);
            res.CreatedByUser.Company.Logo = _companyService.GetLogoPath(res.CreatedByUser.Company.Logo);
            return Response.Message(null, model: res);
        }

        public async Task<Response> GetSelectList(InvPoMasterDto model)
        {
            var itemsList = await _pORepository.GetSelectList(model);
            return itemsList != null ? Response.Message(null, model:itemsList) : Response.Message("POs Not Found.",Not_Found);
        }

        //public async Task<bool> IsExist(InvPoMasterDTO model)
        //{
        //    return await _pORepository.IsPOExist(model);
        //}
    }
}
