using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Accounts;
using Models.DTO.InventoryManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Models.Enums;
using POS_API.Data;
using POS_API.Repositories.AccountsManagement.AccountRepositories;

namespace POS_API.Repositories.InventoryManagement.VendorRepos
{
    public class VendorRepository : RepositoryBase, IVendorRepository, IRepository
    {
        private readonly IAccountRepository _accountRepository;


        public VendorRepository(PosDB_Context dbContext,IMapper mapper,IAccountRepository accountRepository) 
            : base(dbContext: dbContext, mapper: mapper) =>
            _accountRepository = accountRepository;


        public async Task<bool> ChangeStatus(InvVendorDto model)
        {
            var data = await _dbContext.InvVendor.FindAsync(model.Id);
            if (data is null) return false;

            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<InvVendorDto> Create(InvVendorDto model)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //16
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var isExist = await IsExist(vender: model);

                    model.AccountNo = model.AccountNo == null || model.AccountNo.Trim() == ""
                        ? $"{model.Id}-{model.Mobile}"
                        : model.AccountNo;

                    var isAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                    {
                        Title = $"{model.ContactName} - {model.CompanyName} (Acc. Payable)",
                        ParentId = Account.Vendors.ToInt(),
                        AccNo = model.AccountNo,
                        CompanyId = model.CompanyId
                    });

                    if (isExist || isAccountExist)
                    {
                        model.Response = Response.Message(isExist ? "Vendor Already Exists." : "Account No. Already Exists.");
                        return model;
                    }

                    var data = _mapper.Map<InvVendor>(source: model);
                    var next = await _dbContext.InvVendor.CountAsync(predicate: x => x.CompanyId == data.CompanyId) + 1;
                    data.VendorCode = $"VEN-{next.ToString().PadLeft(totalWidth: 4, paddingChar: '0')}";
                    data.Status = StatusTypes.Active.ToInt();
                    await _dbContext.InvVendor.AddAsync(entity: data);
                    await _dbContext.SaveChangesAsync();

                    //create payable account for this vendor.
                    var vendorAccount = await _accountRepository.Create(accAccountDto: new AccAccountDto
                    {
                        Title = $"{model.ContactName} - {model.CompanyName} (Acc. Payable)",
                        AccountTypeId = AccountType.Liability.ToInt(),
                        ParentId = Account.Vendors.ToInt(),
                        AccNo = model.AccountNo,
                        SystemMade = true,
                        IsParent = false,
                        AllowForManualTransaction = true,
                        CompanyId = data.CompanyId,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = data.CreatedOn
                    }, isEditable: false);
                    model.Id = data.Id;
                    data.AccountId = vendorAccount.Id;

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Response = Response.Message("Vendor Created Successfully.", StatusCodes.Created);
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }


        public async Task<bool> Delete(InvVendorDto model)
        {
            var vendor = await _dbContext.InvVendor.FirstOrDefaultAsync(predicate: x =>
                                                                            x.Id == model.Id &&
                                                                            x.CompanyId == model.CompanyId &&
                                                                            x.Status != StatusTypes.Delete.ToInt());
            if (vendor is null) return false;
            vendor.ModifiedBy = model.ModifiedBy;
            vendor.ModifiedOn = model.ModifiedOn;
            vendor.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<InvVendorDto> Edit(InvVendorDto model)
        {
            var data = await _dbContext.InvVendor.FindAsync(model.Id);

            if (data == null) return null;
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //17
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var isExist = await IsExist(vender: model);
                    model.AccountNo = model.AccountNo == null || model.AccountNo.Trim() == "" ? $"{model.Id}-{model.Mobile}" : model.AccountNo;
                    var isAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                    {
                        Title = $"{model.ContactName} - {model.CompanyName}",
                        ParentId = Account.Vendors.ToInt(),
                        AccNo = model.AccountNo,
                        CompanyId = model.CompanyId,
                        Id = data.AccountId
                    });
                    if (isExist || isAccountExist)
                    {
                        model.Response = Response.Error(isExist ? "Vendor Already Exists." : "Account No. Already Exists.");
                        return model;
                    }
                    data.ContactName = model.ContactName;
                    data.CompanyName = model.CompanyName;
                    data.City = model.City;
                    data.Country = model.Country;
                    data.Phone = model.Phone;
                    data.Mobile = model.Mobile;
                    data.PrimaryEmail = model.PrimaryEmail;
                    data.OtherEmail = model.OtherEmail;
                    data.Address = model.Address;
                    data.CompanyId = model.CompanyId;
                    if (model.Status.HasValue) data.Status = model.Status.Value;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;


                    if (data.AccountId.HasValue)
                    {
                        var vendorAccountEdit = await _accountRepository.GetDetails(accountId: data.AccountId.Value, companyId: data.CompanyId);

                        if (vendorAccountEdit is not null)
                        {
                            if (vendorAccountEdit.AccNo != model.AccountNo || vendorAccountEdit.Title != $"{model.ContactName} - {model.CompanyName}")
                            {
                                vendorAccountEdit.AccNo = model.AccountNo;
                                vendorAccountEdit.Title = $"{model.ContactName} - {model.CompanyName}";
                                vendorAccountEdit.ModifiedBy = data.ModifiedBy;
                                await _accountRepository.Edit(accAccountDto: _mapper.Map<AccAccountDto>(source: vendorAccountEdit),forceEdit: true);
                            }
                        }
                        else
                        {
                            //create payable account for this vendor.
                            var vendorAccount = await _accountRepository.Create(accAccountDto: new AccAccountDto
                            {
                                Title = $"{model.ContactName} - {model.CompanyName} (Acc. Payable)",
                                AccountTypeId = AccountType.Liability.ToInt(),
                                ParentId = Account.Vendors.ToInt(),
                                AccNo = model.AccountNo,
                                SystemMade = true,
                                IsParent = false,
                                AllowForManualTransaction= true,
                                CompanyId = data.CompanyId,
                                CreatedBy = data.CreatedBy,
                                CreatedOn = data.CreatedOn
                            }, isEditable: false);
                            model.Id = data.Id;
                            data.AccountId = vendorAccount.Id;
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var returnModel = _mapper.Map<InvVendorDto>(source: data);
                    returnModel.Response = Response.Message("Vendor Updated Successfully.", StatusCodes.Updated);
                    return returnModel;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
            
        }


        public async Task<List<InvVendorDto>> GetAll(InvVendorDto model)
        {
            var query = _dbContext.InvVendor.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId);
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

            var data = await query
                .Select(vendor => new InvVendorDto { 
                    Id = vendor.Id, VendorCode = vendor.VendorCode, CompanyName = vendor.CompanyName, ContactName = vendor.ContactName, Status = vendor.Status,
                    CreatedOn = vendor.CreatedOn, ModifiedOn = vendor.ModifiedOn
                }).ToListAsync();
            return data;
        }


        public async Task<IList<InvVendor_SLM>> GetSelectList(InvVendorDto model)
        {
            return await _dbContext.InvVendor.AsNoTracking()
                .Where(predicate: x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .Select(x=>new InvVendor_SLM { 
                    Value = x.Id.ToString(), Text = x.ContactName, CompanyName = x.CompanyName
                }).ToListAsync();
        }


        public async Task<InvVendorDto> GetDetails(InvVendorDto model)
        {
            var vendor = (await _dbContext.InvVendor.AsNoTracking()
                .Select(selector: invVendor =>
                new
                {
                    Vendor = new InvVendorDto
                    {
                        Id = invVendor.Id,VendorCode = invVendor.VendorCode,CompanyName = invVendor.CompanyName,ContactName = invVendor.ContactName, City = invVendor.City, 
                        Country = invVendor.Country, Address = invVendor.Address, PrimaryEmail = invVendor.PrimaryEmail, OtherEmail= invVendor.OtherEmail,
                        Phone = invVendor.Phone, Mobile = invVendor.Mobile, AccountId = invVendor.AccountId, Status = invVendor.Status,
                        CreatedOn = invVendor.CreatedOn, ModifiedOn = invVendor.ModifiedOn, CompanyId = invVendor.CompanyId,
                                    
                        CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => invVendor.CreatedBy == cu.Id),
                        ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => invVendor.ModifiedBy == bu.Id)
                    }
                }).FirstOrDefaultAsync(predicate: x => x.Vendor.Id == model.Id && x.Vendor.CompanyId == model.CompanyId))?.Vendor;

            
            if (vendor is null) return null;
            //vendor.CreatedByUser.Branch = await _dbContext.Branch.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId).ToListAsync();
            //get vendor account no
            if (vendor.AccountId.HasValue)
            {
                var vendorAccount = await _accountRepository.GetDetails(accountId: vendor.AccountId.Value, companyId: vendor.CompanyId);
                vendor.AccountNo = vendorAccount?.AccNo;
            }
            
            return vendor;
        }


        public async Task<bool> IsExist(InvVendorDto vender)
        {
            return await _dbContext.InvVendor.AsNoTracking().AnyAsync(predicate: x =>
                x.Status != StatusTypes.Delete.ToInt() && x.CompanyName == vender.CompanyName && x.CompanyId == vender.CompanyId && x.Id != vender.Id);
        }
    }
}