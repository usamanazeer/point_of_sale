using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using POS_API.Repositories.InventoryManagement.GoodsReturnNoteRepos;
using POS_API.Services.UserManagement.CompanyServices;
using System.Linq;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.GoodsReturnNoteServices
{
    public class GrrnService : IGrrnService, IService
    {
        private readonly IGrrnRepository _grrnRepository;
        private readonly ICompanyService _companyService;
        public GrrnService(IGrrnRepository grrnRepository, ICompanyService companyService)
        {
            _grrnRepository = grrnRepository;
            _companyService = companyService;
        }
        public async Task<Response> Create(InvGrrnMasterDto model)
        {
            if (model.InvGrrnDetails != null)
            {
                //removing extra entries from recipe items
                model.InvGrrnDetails = model.InvGrrnDetails.Where(x => x.Status.HasValue && (x.Status == StatusTypes.Active.ToInt() && x.ItemId > 0 && x.ReturnQuantity > 0))
                     .GroupBy(x => new { x.BatchNo, x.ItemId, x.Rate }).Select(y => new InvGrrnDetailsDto
                     {
                         BatchNo = y.First().BatchNo,
                         ItemId = y.First().ItemId,
                         Rate = y.First().Rate,
                         Status = y.First().Status,
                         ReturnQuantity = y.Sum(x => x.ReturnQuantity),
                         //OrderedQuantity = x.Sum(x=>x.OrderedQuantity)
                     }).ToList();
            }
            var res = await _grrnRepository.Create(model);
            return Response.Message("GRRN Created Successfully.", StatusCodes.Created, res);
        }


        public async Task<Response> Delete(InvGrrnMasterDto model)
        {
            return await _grrnRepository.Delete(model) ? Response.Message("GRRN Deleted Successfully.", model: true)
                : Response.Message("GRRN Not Found.", StatusCodes.Not_Found, false);
        }


        public async Task<Response> Edit(InvGrrnMasterDto model)
        {
            if (model.InvGrrnDetails != null)
            {
                //removing extra entries from recipe items
                model.InvGrrnDetails = model.InvGrrnDetails.Where(x => x.Status.HasValue && (x.Status == StatusTypes.Active.ToInt() && x.ItemId > 0 && x.ReturnQuantity > 0))
                     .GroupBy(x => new { x.BatchNo, x.ItemId, x.Rate }).Select(y => new InvGrrnDetailsDto()
                     {
                         BatchNo = y.First().BatchNo, ItemId = y.First().ItemId, Rate = y.First().Rate,
                         Status = y.First().Status, ReturnQuantity = y.Sum(x => x.ReturnQuantity)
                     }).ToList();
            }
            var res = await _grrnRepository.Edit(model);
            return res != null ?
                Response.Message("GRRN Updated Successfully.", StatusCodes.Updated, model: res) :
                Response.Message("GRRN Not Found.", StatusCodes.Not_Found, model: model);
        }

        public async Task<Response> GetAll(InvGrrnMasterDto model)
        {
            var res = await _grrnRepository.GetAll(model: model);
            return res.Any() ? Response.Message(null, model: res) : Response.Message("GRRN Not Found.", StatusCodes.Not_Found);
        }

        public async Task<Response> GetDetails(InvGrrnMasterDto model)
        {
            var res = await _grrnRepository.GetDetails(model);
            if (res == null)
                return Response.Message("GRRNs Not Found", StatusCodes.Not_Found);

            res.CreatedByUser.Company.Logo = _companyService.GetLogoPath(res.CreatedByUser.Company.Logo);
            return Response.Message(null, model: res);
        }

        public async Task<Response> GetSelectList(InvGrrnMasterDto model)
        {
            var itemsList = await _grrnRepository.GetSelectList(model);
            return itemsList != null ? Response.Message(null, model: itemsList) : Response.Message("GRRNs Not Found.", StatusCodes.Not_Found);
        }
    }
}