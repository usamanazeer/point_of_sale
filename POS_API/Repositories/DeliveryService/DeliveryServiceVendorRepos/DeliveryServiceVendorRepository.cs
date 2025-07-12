using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Accounts;
using Models.DTO.DeliveryService;
using Models.DTO.ViewModels.SelectList.DeliveryService;
using Models.Enums;
using POS_API.Data;
using POS_API.Repositories.AccountsManagement.AccountRepositories;

namespace POS_API.Repositories.DeliveryService.DeliveryServiceVendorRepos
{
    public class DeliveryServiceVendorRepository : RepositoryBase, IDeliveryServiceVendorRepository, IRepository
    {
        private readonly IAccountRepository _accountRepository;


        public DeliveryServiceVendorRepository(PosDB_Context dbContext,
                                               IMapper mapper,
                                               IAccountRepository accountRepository) : base(dbContext: dbContext,
                                                                                            mapper: mapper) =>
            _accountRepository = accountRepository;


        public async Task<DeliDeliveryServiceVendorDto> Create(DeliDeliveryServiceVendorDto deliveryServiceVendorDto)
        {
            return  await  TransactionStrategy.ExecuteAsync(operation: async () =>
            {
                //2
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    if (await IsExist(deliveryServiceVendorDto: deliveryServiceVendorDto))
                    {
                        deliveryServiceVendorDto.Response = Response.Error("Delivery Service Already Exists.");
                        return deliveryServiceVendorDto;
                    }
                    if (!deliveryServiceVendorDto.IsSelf)
                    {
                        var isRecAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                                                                                                {
                                                                                                    Title =
                                                                                                        $"{deliveryServiceVendorDto.Name} (Rec.)",
                                                                                                    ParentId = Account
                                                                                                               .DeliveryServicesReceivables
                                                                                                               .ToInt(),
                                                                                                    AccNo =
                                                                                                        $"{deliveryServiceVendorDto.AccountNo}-{AccountType.Asset.ToInt()}",
                                                                                                    CompanyId =
                                                                                                        deliveryServiceVendorDto
                                                                                                            .CompanyId
                                                                                                });

                        var isExpAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                                                                                                {
                                                                                                    Title =
                                                                                                        $"{deliveryServiceVendorDto.Name} (Exp.)",
                                                                                                    ParentId = Account
                                                                                                               .DeliveryServicesExpenses
                                                                                                               .ToInt(),
                                                                                                    AccNo =
                                                                                                        $"{deliveryServiceVendorDto.AccountNo}-{AccountType.Expenses.ToInt()}",
                                                                                                    CompanyId =
                                                                                                        deliveryServiceVendorDto
                                                                                                            .CompanyId
                                                                                                });

                        if (isRecAccountExist || isExpAccountExist)
                        {
                            deliveryServiceVendorDto.Response = Response.Error("Account No. Already Exists.");
                            return deliveryServiceVendorDto;
                        }
                    }

                    var data = _mapper.Map<DeliDeliveryServiceVendor>(source: deliveryServiceVendorDto);
                    data.Status = StatusTypes.Active.ToInt();
                    await _dbContext.DeliDeliveryServiceVendor.AddAsync(entity: data);
                    await _dbContext.SaveChangesAsync();

                    if (!deliveryServiceVendorDto.IsSelf)
                    {
                        // create payable account for this vendor.
                        var deliveryServiceRecAccount = await _accountRepository.Create(accAccountDto: new AccAccountDto
                                                                                                       {
                                                                                                           Title =
                                                                                                               $"{deliveryServiceVendorDto.Name} (Rec.)",
                                                                                                           AccountTypeId
                                                                                                               =
                                                                                                               AccountType
                                                                                                                   .Asset
                                                                                                                   .ToInt(),
                                                                                                           ParentId =
                                                                                                               Account
                                                                                                                   .DeliveryServicesReceivables
                                                                                                                   .ToInt(),
                                                                                                           AccNo =
                                                                                                               $"{deliveryServiceVendorDto.AccountNo}-{AccountType.Asset.ToInt()}",
                                                                                                           SystemMade =
                                                                                                               true,
                                                                                                           IsParent =
                                                                                                               false,
                                                                                                           AllowForManualTransaction
                                                                                                               = true,
                                                                                                           CompanyId =
                                                                                                               data
                                                                                                                   .CompanyId,
                                                                                                           CreatedBy =
                                                                                                               data
                                                                                                                   .CreatedBy,
                                                                                                           CreatedOn =
                                                                                                               data
                                                                                                                   .CreatedOn
                                                                                                       },
                                                                                        isEditable: false);

                        // create expense account for this vendor.
                        var deliveryServiceExpAccount = await _accountRepository.Create(accAccountDto: new AccAccountDto
                                                                                                       {
                                                                                                           Title =
                                                                                                               $"{deliveryServiceVendorDto.Name} (Exp.)",
                                                                                                           AccountTypeId
                                                                                                               =
                                                                                                               AccountType
                                                                                                                   .Expenses
                                                                                                                   .ToInt(),
                                                                                                           ParentId =
                                                                                                               Account
                                                                                                                   .DeliveryServicesExpenses
                                                                                                                   .ToInt(),
                                                                                                           AccNo =
                                                                                                               $"{deliveryServiceVendorDto.AccountNo}-{AccountType.Expenses.ToInt()}",
                                                                                                           SystemMade =
                                                                                                               true,
                                                                                                           IsParent =
                                                                                                               false,
                                                                                                           AllowForManualTransaction
                                                                                                               = false,
                                                                                                           CompanyId =
                                                                                                               data
                                                                                                                   .CompanyId,
                                                                                                           CreatedBy =
                                                                                                               data
                                                                                                                   .CreatedBy,
                                                                                                           CreatedOn =
                                                                                                               data
                                                                                                                   .CreatedOn
                                                                                                       },
                                                                                        isEditable: false);

                        deliveryServiceVendorDto.Id = data.Id;
                        data.RecAccountId = deliveryServiceRecAccount.Id;
                        data.ExpAccountId = deliveryServiceExpAccount.Id;
                        await _dbContext.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    deliveryServiceVendorDto.Response = Response.Message("Delivery Service Created Successfully.", StatusCodes.Created);
                    return deliveryServiceVendorDto;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }


        public async Task<bool> Delete(DeliDeliveryServiceVendorDto model)
        {
            var modifier = await _dbContext.DeliDeliveryServiceVendor
                                           .FirstOrDefaultAsync(predicate: x =>
                                                                            x.Id == model.Id && x.CompanyId == model.CompanyId &&
                                                                            x.Status != StatusTypes.Delete.ToInt());
            if (modifier is null) return false;
            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<DeliDeliveryServiceVendorDto> Edit(DeliDeliveryServiceVendorDto deliveryServiceVendorDto)
        {
            var data = await _dbContext.DeliDeliveryServiceVendor
                                       .FirstOrDefaultAsync(predicate: x => x.Id == deliveryServiceVendorDto.Id);
            if (data is null) return null;
            return await TransactionStrategy.ExecuteAsync(operation: async () =>
            {
                //1
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    if (await IsExist(deliveryServiceVendorDto: deliveryServiceVendorDto))
                    {
                        deliveryServiceVendorDto.Response = Response.Error("Delivery Service Already Exists.");
                        return deliveryServiceVendorDto;
                    }

                    if (!deliveryServiceVendorDto.IsSelf)
                    {
                        var isRecAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                                                                                                {
                                                                                                    Title =
                                                                                                        $"{deliveryServiceVendorDto.Name} (Rec.)",
                                                                                                    ParentId = Account
                                                                                                               .DeliveryServicesReceivables
                                                                                                               .ToInt(),
                                                                                                    AccNo =
                                                                                                        $"{deliveryServiceVendorDto.AccountNo}-{AccountType.Asset.ToInt()}",
                                                                                                    CompanyId =
                                                                                                        deliveryServiceVendorDto
                                                                                                            .CompanyId,
                                                                                                    Id = data
                                                                                                        .RecAccountId
                                                                                                });

                        var isExpAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                                                                                                {
                                                                                                    Title =
                                                                                                        $"{deliveryServiceVendorDto.Name} (Exp.)",
                                                                                                    ParentId = Account
                                                                                                               .DeliveryServicesExpenses
                                                                                                               .ToInt(),
                                                                                                    AccNo =
                                                                                                        $"{deliveryServiceVendorDto.AccountNo}-{AccountType.Expenses.ToInt()}",
                                                                                                    CompanyId =
                                                                                                        deliveryServiceVendorDto
                                                                                                            .CompanyId,
                                                                                                    Id = data
                                                                                                        .ExpAccountId
                                                                                                });

                        if (isRecAccountExist || isExpAccountExist)
                        {
                            deliveryServiceVendorDto.Response =Response.Error("Account No. Already Exists.");
                            return deliveryServiceVendorDto;
                        }
                    }

                    if (!deliveryServiceVendorDto.IsSelf && (deliveryServiceVendorDto.AccountNo != data.AccountNo ||
                                                             deliveryServiceVendorDto.Name != $"{data.Name}"))
                        if (data.RecAccountId.HasValue && data.ExpAccountId.HasValue)
                        {
                            var deliveryServiceRecAccount =
                                await _accountRepository.GetDetails(accountId: data.RecAccountId.Value,
                                                                    companyId: data.CompanyId);
                            var deliveryServiceExpAccount =
                                await _accountRepository.GetDetails(accountId:  data.ExpAccountId.Value,
                                                                    companyId: data.CompanyId);

                            deliveryServiceRecAccount.AccNo =
                                $"{deliveryServiceVendorDto.AccountNo}-{AccountType.Asset.ToInt()}";
                            deliveryServiceRecAccount.Title = $"{deliveryServiceVendorDto.Name} (Rec.)";
                            deliveryServiceRecAccount.ModifiedBy = data.ModifiedBy;
                            await _accountRepository.Edit(accAccountDto: deliveryServiceRecAccount,
                                                          forceEdit: true);

                            deliveryServiceExpAccount.AccNo =
                                $"{deliveryServiceVendorDto.AccountNo}-{AccountType.Expenses.ToInt()}";
                            deliveryServiceExpAccount.Title = $"{deliveryServiceVendorDto.Name} (Exp.)";
                            deliveryServiceExpAccount.ModifiedBy = data.ModifiedBy;
                            await _accountRepository.Edit(accAccountDto: deliveryServiceExpAccount,
                                                          forceEdit: true);
                        }

                    data.Name = deliveryServiceVendorDto.Name;
                    data.AccountNo = deliveryServiceVendorDto.AccountNo;
                    data.ServiceDiscount = deliveryServiceVendorDto.ServiceDiscount;
                    //if already one vendor for not exists for self delivery service.
                    if (deliveryServiceVendorDto.IsSelf)
                    {
                        if (!await IsSelfExist(companyId: deliveryServiceVendorDto.CompanyId))
                            data.IsSelf = deliveryServiceVendorDto.IsSelf;
                    }
                    else
                    {
                        data.IsSelf = deliveryServiceVendorDto.IsSelf;
                    }

                    data.IsServiceDiscountInPercent = deliveryServiceVendorDto.IsServiceDiscountInPercent;
                    if (deliveryServiceVendorDto.Status.HasValue) data.Status = deliveryServiceVendorDto.Status.Value;
                    data.ModifiedBy = deliveryServiceVendorDto.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;

                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    var returnModel = _mapper.Map<DeliDeliveryServiceVendorDto>(source: data);
                    returnModel.Response = Response.Message("Delivery Service Updated Successfully.", StatusCodes.Created);
                    return returnModel;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }


        public async Task<List<DeliDeliveryServiceVendorDto>> GetAll(DeliDeliveryServiceVendorDto model)
        {
            var query = _dbContext.DeliDeliveryServiceVendor.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(predicate: r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted) query = query.Where(predicate: c => c.Status != StatusTypes.Delete.ToInt());

                if (model.Status.HasValue) query = query.Where(predicate: r => r.Status == model.Status);
            }

            query = query.OrderBy(keySelector: x => x.IsSelf).ThenBy(keySelector: x => x.CreatedOn);
            var data = await query
                .Select(x=>new DeliDeliveryServiceVendorDto { 
                    Id = x.Id, 
                    Name = x.Name,
                    IsSelf = x.IsSelf,
                    ServiceDiscount = x.ServiceDiscount,
                    IsServiceDiscountInPercent = x.IsServiceDiscountInPercent,
                    //AccountNo = x.AccountNo, 
                    Status =x.Status
                }).ToListAsync();
            return data;
        }


        public async Task<DeliDeliveryServiceVendorDto> GetDetails(DeliDeliveryServiceVendorDto model)
        {
            var data = await _dbContext.Set<DeliDeliveryServiceVendor>().AsNoTracking()
                .Select(selector: deliveryService =>
                    new
                    {
                        deliveryService = new DeliDeliveryServiceVendorDto
                        {
                            Id = deliveryService.Id,
                            Name = deliveryService.Name,
                            IsSelf = deliveryService.IsSelf,
                            ServiceDiscount = deliveryService.ServiceDiscount,
                            IsServiceDiscountInPercent = deliveryService.IsServiceDiscountInPercent,
                            AccountNo = deliveryService.AccountNo,
                            Status = deliveryService.Status,
                            CreatedBy = deliveryService.CreatedBy,
                            CreatedOn = deliveryService.CreatedOn,
                            ModifiedBy = deliveryService.ModifiedBy,
                            ModifiedOn = deliveryService.ModifiedOn,
                            CompanyId = deliveryService.CompanyId
                        },
                        cu = UserWithRoleSelect.FirstOrDefault(cu => deliveryService.CreatedBy == cu.Id),
                        bu = UserWithRoleSelect.FirstOrDefault(bu => deliveryService.ModifiedBy == bu.Id)
                    }).FirstOrDefaultAsync(predicate: x => x.deliveryService.Id == model.Id && x.deliveryService.CompanyId == model.CompanyId);
            if (data is null) return null;
            //data.cu.Company.Branch =await _dbContext.Branch.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId).Select(x => new BranchDto { Id = x.Id, Name = x.Name }).ToListAsync();
            
            var dataModel = data.deliveryService;
            dataModel.CreatedByUser =  data.cu;
            dataModel.ModifiedByUser = data.bu;
            return dataModel;
        }


        public async Task<IList<DeliveryServiceVendor_SLM>> GetSelectList(DeliDeliveryServiceVendorDto model)
        {
            var deliveryServices = await _dbContext.DeliDeliveryServiceVendor.AsNoTracking()
                .Where(predicate: x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .OrderByDescending(keySelector: x => x.IsSelf)
                .ThenBy(keySelector: x => x.CreatedOn)
                .Select(x=> new DeliveryServiceVendor_SLM
                { Value =  x.Id.ToString(), Text = x.Name, IsSelf = x.IsSelf, ServiceCharges = x.ServiceDiscount, ChargesInPercent = x.IsServiceDiscountInPercent })
                .ToListAsync();
            return deliveryServices;
        }


        public async Task<bool> IsExist(DeliDeliveryServiceVendorDto deliveryServiceVendorDto)
        {
            return await _dbContext.DeliDeliveryServiceVendor.AsNoTracking()
                .AnyAsync(predicate: x => x.Status != StatusTypes.Delete.ToInt() &&
                    x.Name == deliveryServiceVendorDto.Name &&
                    x.CompanyId == deliveryServiceVendorDto.CompanyId &&
                    x.Id != deliveryServiceVendorDto.Id);
        }


        public async Task<bool> IsSelfExist(int companyId)
        {
            return await _dbContext.DeliDeliveryServiceVendor.AsNoTracking().AnyAsync(predicate: x => x.Status != StatusTypes.Delete.ToInt() && x.CompanyId == companyId && x.IsSelf);
        }
    }
}