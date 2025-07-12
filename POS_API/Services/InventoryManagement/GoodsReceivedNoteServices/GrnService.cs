using Models;
using Models.DTO.InventoryManagement;
using Models.Enums;
using POS_API.Repositories.InventoryManagement.GoodsReceivedNoteRepos;
using POS_API.Services.UserManagement.CompanyServices;
using System.Linq;
using System.Threading.Tasks;

namespace POS_API.Services.InventoryManagement.GoodsReceivedNoteServices
{
    public class GrnService : IGrnService, IService
    {
        private readonly IGrnRepository _grnRepository;
        private readonly ICompanyService _companyService;
        public GrnService(IGrnRepository grnRepository, ICompanyService companyService)
        {
            _grnRepository = grnRepository;
            _companyService = companyService;
        }

        public async Task<Response> Create(InvGrnMasterDto model)
        {
            if (model.InvGrnDetails != null)
            {
                //removing extra entries from recipe items
                model.InvGrnDetails = model.InvGrnDetails.Where(x => x.Status.HasValue && (x.Status == StatusTypes.Active.ToInt() && x.ItemId > 0 && x.ReceivedQuantity > 0))
                     .GroupBy(x => new { x.PoId,x.BatchNo, x.ItemId, x.Rate }).Select(y => new InvGrnDetailsDto
                     {
                         PoId = y.First().PoId,
                         BatchNo = y.First().BatchNo,
                         ItemId = y.First().ItemId,
                         Rate = y.First().Rate,
                         Status = y.First().Status,
                         ReceivedQuantity = y.Sum(x => x.ReceivedQuantity),
                         //OrderedQuantity = x.Sum(x=>x.OrderedQuantity)
                     }).ToList();
            }
            var res = await _grnRepository.Create(model);
            return Response.Message("GRN Created Successfully.", StatusCodes.Created, res);
        }


        public async Task<Response> Delete(InvGrnMasterDto model)
        {
            return await _grnRepository.Delete(model) ? Response.Message("GRN Deleted Successfully.", model: true)
                : Response.Message("GRN Not Found.", StatusCodes.Not_Found, false);
        }

        public async Task<Response> Edit(InvGrnMasterDto model)
        {
            if (model.InvGrnDetails != null)
            {
                //removing extra entries from recipe items
                model.InvGrnDetails = model.InvGrnDetails.Where(x => x.Status.HasValue && (x.Status == StatusTypes.Active.ToInt() && x.ItemId > 0 && x.ReceivedQuantity > 0))
                     .GroupBy(x => new { x.PoId, x.BatchNo, x.ItemId, x.Rate }).Select(y => new InvGrnDetailsDto
                     {
                         PoId = y.First().PoId,
                         BatchNo = y.First().BatchNo,
                         ItemId = y.First().ItemId,
                         Rate = y.First().Rate,
                         Status = y.First().Status,
                         ReceivedQuantity = y.Sum(x => x.ReceivedQuantity),
                     }).ToList();
            }
            var res = await _grnRepository.Edit(model);
            return res != null ?
                Response.Message("GRN Updated Successfully.", StatusCodes.Updated, model: res) :
                Response.Message("GRN Not Found.", StatusCodes.Not_Found, model: model);
        }

        public async Task<Response> GetAll(InvGrnMasterDto model)
        {
            var res = await _grnRepository.GetAll(model: model);
            return res.Any() ? Response.Message(null, model: res) : Response.Message("GRN Not Found.", StatusCodes.Not_Found);
        }

        public async Task<Response> GetDetails(InvGrnMasterDto model)
        {
            var res = await _grnRepository.GetDetails(model);
            if (res == null)
                return Response.Message("GRN Not Found", StatusCodes.Not_Found);

            res.CreatedByUser.Company.Logo = _companyService.GetLogoPath(res.CreatedByUser.Company.Logo);
            return Response.Message(null, model: res);
        }

        public async Task<Response> GetSelectList(InvGrnMasterDto model)
        {
            var itemsList = await _grnRepository.GetSelectList(model);
            return itemsList != null ? Response.Message(null, model: itemsList) : Response.Message("GRNs Not Found.", StatusCodes.Not_Found);
        }
    }
}
