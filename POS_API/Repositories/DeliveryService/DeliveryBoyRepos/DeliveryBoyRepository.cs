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

namespace POS_API.Repositories.DeliveryService.DeliveryBoyRepos
{
    public class DeliveryBoyRepository : RepositoryBase, IDeliveryBoyRepository, IRepository
    {
        private readonly IAccountRepository _accountRepository;


        public DeliveryBoyRepository(PosDB_Context dbContext,
                                     IMapper mapper,
                                     IAccountRepository accountRepository) : base(dbContext: dbContext,
                                                                                  mapper: mapper) =>
            _accountRepository = accountRepository;


        public async Task<DeliDeliveryBoyDto> Create(DeliDeliveryBoyDto deliDeliveryBoyDto)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //3
                await using var createDeliveryBoyTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var isExist = await IsExist(deliDeliveryBoyDto: deliDeliveryBoyDto);

                    deliDeliveryBoyDto.AccountNo =
                        deliDeliveryBoyDto.AccountNo is null || deliDeliveryBoyDto.AccountNo.Trim() == ""
                            ? $"{deliDeliveryBoyDto.ContactNo}"
                            : deliDeliveryBoyDto.AccountNo;

                    var isAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                    {
                        Title = $"{deliDeliveryBoyDto.Name} - {deliDeliveryBoyDto.Cnic}",
                        ParentId = Account.DeliveryBoys.ToInt(),
                        AccNo = deliDeliveryBoyDto.AccountNo,
                        CompanyId = deliDeliveryBoyDto.CompanyId
                    });

                    if (isExist)
                    {
                        deliDeliveryBoyDto.Response = Response.Error("Delivery Boy Already Exists.");
                        return deliDeliveryBoyDto;
                    }

                    if (isAccountExist)
                    {
                        deliDeliveryBoyDto.Response = Response.Error("Account No. Already Exists.");
                        return deliDeliveryBoyDto;
                    }
                    var data = _mapper.Map<DeliDeliveryBoy>(source: deliDeliveryBoyDto);
                    data.Status = StatusTypes.Active.ToInt();
                    await _dbContext.DeliDeliveryBoy.AddAsync(entity: data);
                    await _dbContext.SaveChangesAsync();

                    //create receivable account for delivery.
                    var deliveryBoyAccount = await _accountRepository.Create(accAccountDto: new AccAccountDto
                    {
                        Title = $"{deliDeliveryBoyDto.Name} - {deliDeliveryBoyDto.Cnic}",
                        AccountTypeId = AccountType.Asset.ToInt(),
                        ParentId = Account.DeliveryBoys.ToInt(),
                        AccNo = deliDeliveryBoyDto.AccountNo,
                        SystemMade = true,
                        IsParent = false,
                        AllowForManualTransaction = true,
                        CompanyId = data.CompanyId,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = data.CreatedOn
                    }, isEditable: false);
                    deliDeliveryBoyDto.Id = data.Id;
                    if (deliveryBoyAccount.Id.HasValue) data.AccountId = deliveryBoyAccount.Id.Value;

                    await _dbContext.SaveChangesAsync();
                    await createDeliveryBoyTransaction.CommitAsync();
                    deliDeliveryBoyDto.Response = Response.Message("Delivery Boy Created Successfully.", StatusCodes.Created);
                    return deliDeliveryBoyDto;
                    //return Map<DeliDeliveryBoyDto>(obj: data);
                }
                catch (Exception)
                {
                    await createDeliveryBoyTransaction.RollbackAsync();
                    throw;
                }
            });
            
        }

        public async Task<bool> Delete(DeliDeliveryBoyDto model)
        {
            var modifier = await _dbContext.DeliDeliveryBoy.SingleOrDefaultAsync(predicate: x =>
                                                                                     x.Id == model.Id &&
                                                                                     x.CompanyId == model.CompanyId &&
                                                                                     x.Status != StatusTypes
                                                                                                 .Delete.ToInt());
            if (modifier == null) return false;
            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<DeliDeliveryBoyDto> Edit(DeliDeliveryBoyDto deliDeliveryBoyDto)
        {
            var data =
                await _dbContext.DeliDeliveryBoy.SingleOrDefaultAsync(predicate: x => x.Id == deliDeliveryBoyDto.Id);
            if (data == null) return null;
            
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //4
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var isExist = await IsExist(deliDeliveryBoyDto: deliDeliveryBoyDto);
                    deliDeliveryBoyDto.AccountNo =
                        deliDeliveryBoyDto.AccountNo is null || deliDeliveryBoyDto.AccountNo.Trim() == ""
                            ? $"{deliDeliveryBoyDto.ContactNo}"
                            : deliDeliveryBoyDto.AccountNo;
                    var isAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                    {
                        Title = $"{deliDeliveryBoyDto.Name} - {deliDeliveryBoyDto.Cnic}",
                        ParentId = Account.DeliveryBoys.ToInt(),
                        AccNo = deliDeliveryBoyDto.AccountNo,
                        CompanyId = deliDeliveryBoyDto.CompanyId,
                        Id = data.AccountId
                    });

                    if (isExist || isAccountExist)
                    {
                        deliDeliveryBoyDto.Response = Response.Error(isExist ? "Delivery Boy Already Exists." : "Account No. Already Exists.");
                        return deliDeliveryBoyDto;
                    }

                    data.Name = deliDeliveryBoyDto.Name;
                    data.ContactNo = deliDeliveryBoyDto.ContactNo;
                    data.Cnic = deliDeliveryBoyDto.Cnic;
                    data.BikeNo = deliDeliveryBoyDto.BikeNo;
                    data.Email = deliDeliveryBoyDto.Email;
                    if (deliDeliveryBoyDto.Status.HasValue) data.Status = deliDeliveryBoyDto.Status.Value;
                    data.ModifiedBy = deliDeliveryBoyDto.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();

                    var deliveryBoyAccount = await _accountRepository.GetDetails(accountId: data.AccountId,
                                                                                 companyId: data.CompanyId);

                    if (deliveryBoyAccount.AccNo != deliDeliveryBoyDto.AccountNo || deliveryBoyAccount.Title !=
                        $"{deliDeliveryBoyDto.Name} - {deliDeliveryBoyDto.Cnic}")
                    {
                        deliveryBoyAccount.AccNo = deliDeliveryBoyDto.AccountNo;
                        deliveryBoyAccount.Title = $"{deliDeliveryBoyDto.Name} - {deliDeliveryBoyDto.Cnic}";
                        deliveryBoyAccount.ModifiedBy = data.ModifiedBy;
                        await _accountRepository.Edit(accAccountDto: deliveryBoyAccount, forceEdit: true);
                    }

                    await transaction.CommitAsync();
                    var returnModel = _mapper.Map<DeliDeliveryBoyDto>(source: data);
                    returnModel.Response = Response.Message("Delivery Boy Updated Successfully.", StatusCodes.Updated);
                    return returnModel;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }


        public async Task<List<DeliDeliveryBoyDto>> GetAll(DeliDeliveryBoyDto model)
        {
            var query = _dbContext.DeliDeliveryBoy.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId);
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

            var data =  await query
                .Select(x=> new DeliDeliveryBoyDto{
                    Id = x.Id, Name = x.Name, ContactNo = x.ContactNo,Cnic = x.Cnic, Email = x.Email, Status = x.Status
                }).ToListAsync();
            return data;
        }


        public async Task<DeliDeliveryBoyDto> GetDetails(DeliDeliveryBoyDto model)
        {
            var data = await _dbContext.Set<DeliDeliveryBoy>().AsNoTracking()
                .Select(selector: deliveryBoy =>
                new
                {
                    deliveryBoy = new DeliDeliveryBoyDto
                    {
                        Id = deliveryBoy.Id,
                        Name = deliveryBoy.Name,
                        ContactNo = deliveryBoy.ContactNo,
                        Cnic = deliveryBoy.Cnic,
                        BikeNo = deliveryBoy.BikeNo,
                        Email = deliveryBoy.Email,
                        Status = deliveryBoy.Status,
                        CreatedBy = deliveryBoy.CreatedBy,
                        CreatedOn = deliveryBoy.CreatedOn,
                        ModifiedBy = deliveryBoy.ModifiedBy,
                        ModifiedOn = deliveryBoy.ModifiedOn,
                        CompanyId = deliveryBoy.CompanyId
                    },
                    cu = UserWithRoleSelect.FirstOrDefault(cu => deliveryBoy.CreatedBy == cu.Id),
                    bu = UserWithRoleSelect.FirstOrDefault(bu => deliveryBoy.ModifiedBy == bu.Id)
                }).FirstOrDefaultAsync(predicate: x => x.deliveryBoy.Id == model.Id && x.deliveryBoy.CompanyId == model.CompanyId);
            
            if (data is null) return null;
            //data.cu.Company.Branch = await _dbContext.Branch.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId).Select(x => new BranchDto { Id = x.Id, Name = x.Name }).ToListAsync();
            
            var dataModel = data.deliveryBoy;
            dataModel.CreatedByUser = data.cu;
            dataModel.ModifiedByUser = data.bu;
            return dataModel;
        }

        public async Task<IList<DeliveryBoy_SLM>> GetSelectList(DeliDeliveryBoyDto model)
        {
            var diningTable =
                await _dbContext.DeliDeliveryBoy.AsNoTracking().Where(predicate: x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .Select(x=>new DeliveryBoy_SLM { 
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).ToListAsync();
            return diningTable;
        }

        public async Task<bool> IsExist(DeliDeliveryBoyDto deliDeliveryBoyDto)
        {
            return await _dbContext.DeliDeliveryBoy.AsNoTracking()
                .AnyAsync(
                          predicate: x =>
                              x.Status != StatusTypes.Delete.ToInt() &&
                              x.Name == deliDeliveryBoyDto.Name &&
                              x.Cnic == deliDeliveryBoyDto.Cnic &&
                              x.CompanyId == deliDeliveryBoyDto.CompanyId &&
                              x.Id != deliDeliveryBoyDto.Id
                         );
        }
    }
}