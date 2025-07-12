using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Models.DTO.Accounts;
using Models.DTO.InventoryManagement;
using POS_API.Data;
using POS_API.Repositories.InventoryManagement.PurchaseRepositories;

namespace POS_API.Repositories.AccountsManagement.BillsRepositories
{
    public class BillsRepository : RepositoryBase, IBillsRepository, IRepository
    {
        private readonly IPurchaseRepository _purchaseRepository;
        public BillsRepository(PosDB_Context dbContext,
                 IMapper mapper,
                 IPurchaseRepository purchaseRepository) : base(dbContext,
                                        mapper) =>
            _purchaseRepository = purchaseRepository;


        public async Task<IList<BillDto>> GetAll(BillDto billDto)
        {
            var purchaseModel = new InvPurchaseMasterDto
            {
                Id = billDto.Id,
                Status = billDto.Status,
                DisplayDeleted = billDto.DisplayDeleted,
                CompanyId = billDto.CompanyId,
                BranchId = billDto.BranchId,
                BillNo = billDto.BillNo,
                PurchaseDate = billDto.BillDate,
                VendorId = billDto.VendorId,
                BillDueDate = billDto.BillDueDate,
                FromDueDate = billDto.FromDueDate,
                ToDueDate = billDto.ToDueDate
            };
            var purchaseData = await _purchaseRepository.GetAll(purchaseModel, excludePaidBills :billDto.ExcludePaidBills);
            return _mapper.Map<IList<BillDto>>(purchaseData);
        }


        public async Task<BillDto> GetDetails(BillDto billDto)
        {
            var purchaseModel = new InvPurchaseMasterDto
            {
                Id = billDto.Id,
                Status = billDto.Status,
                CompanyId = billDto.CompanyId,
                BranchId = billDto.BranchId,
            };
            var purchaseData = await _purchaseRepository.GetDetails(purchaseModel, includePayments: true);
            return _mapper.Map<BillDto>(purchaseData);
        }


        public async Task<BillDto> PayBill(AccBillPaymentDto billPaymentDto)
        {
            await _dbContext.Acc_PayVendorBill(billPaymentDto);
            return await GetDetails(new BillDto{
                                               Id = billPaymentDto.BillId,
                                               Status = billPaymentDto.Status,
                                               CompanyId = billPaymentDto.CompanyId,
                                               BranchId = billPaymentDto.BranchId,
                                           });
        }
    }
}
