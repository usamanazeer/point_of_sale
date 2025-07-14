using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Accounts;
using Models.DTO.GeneralSettings;
using Models.DTO.Reporting.Sales;
using Models.Enums;
using POS_API.Data;
using POS_API.Repositories.AccountsManagement.AccountRepositories;

namespace POS_API.Repositories.GeneralSettings.TaxRepos
{
    public class TaxRepository : RepositoryBase, ITaxRepository, IRepository
    {
        private readonly IAccountRepository _accountRepository;


        public TaxRepository(PosDB_Context dbContext,
                             IMapper mapper,
                             IAccountRepository accountRepository) : base(dbContext: dbContext,
                                                                          mapper: mapper) =>
            _accountRepository = accountRepository;


        public async Task<TaxDto> Create(TaxDto model)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //10
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    if (model.EnableForPos)
                    {
                        var enabledTax = await _dbContext.Tax.SingleOrDefaultAsync(predicate: x =>
                                                                                       x.EnableForPos == true &&
                                                                                       x.CompanyId == model.CompanyId &&
                                                                                       x.Status !=
                                                                                       StatusTypes.Delete.ToInt());
                        if (enabledTax != null) enabledTax.EnableForPos = false;
                    }

                    var data = _mapper.Map<Tax>(source: model);
                    var isExist = await IsExist(model: model);
                    var isAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                    {
                        Title =
                                                                                                 $"{model.Name} (Acc. Payable)",
                        ParentId = Account
                                                                                                        .TaxCollected
                                                                                                        .ToInt(),
                        AccNo = model.AccountNo,
                        CompanyId = model.CompanyId
                    });

                    if (isExist || isAccountExist)
                    {
                        model.Response = Response.Error(isExist ? $"{model.Name} Already Exists." : "Account No. Already Exists.");
                        return model;
                    }

                    data.Status = StatusTypes.Active.ToInt();
                    await _dbContext.Tax.AddAsync(entity: data);
                    await _dbContext.SaveChangesAsync();
                    //create payable account for this tax collection.
                    var taxPayableAccount = await _accountRepository.Create(accAccountDto: new AccAccountDto
                    {
                        Title = $"{model.Name} (Acc. Payable)",
                        AccountTypeId = AccountType.Liability.ToInt(),
                        ParentId = Account.TaxCollected.ToInt(),
                        AccNo = model.AccountNo,
                        SystemMade = true,
                        IsParent = false,
                        AllowForManualTransaction = true,
                        CompanyId = data.CompanyId,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = data.CreatedOn
                    },isEditable: false);

                    if (taxPayableAccount.Id.HasValue) data.AccountId = taxPayableAccount.Id.Value;
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Response = Response.Message("Tax Created Successfully.", StatusCodes.Created);
                    model.Id = data.Id;
                    return model;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
            
        }


        public async Task<bool> Delete(TaxDto model)
        {
            var vendor = await _dbContext.Tax.FirstOrDefaultAsync(predicate: x =>
                  x.Id == model.Id && x.CompanyId == model.CompanyId &&
                  x.Status != StatusTypes.Delete.ToInt());
            if (vendor != null)
            {
                vendor.ModifiedBy = model.ModifiedBy;
                vendor.ModifiedOn = model.ModifiedOn;
                vendor.Status = StatusTypes.Delete.ToInt();
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<TaxDto> Edit(TaxDto model)
        {
            var data = await _dbContext.Tax.FindAsync(model.Id);

            if (data is null) return null;

            if (model.EnableForPos)
            {
                var enabledTax = await _dbContext.Tax.SingleOrDefaultAsync(predicate: x =>
                                                                               x.EnableForPos == true &&
                                                                               x.CompanyId == model.CompanyId &&
                                                                               x.Status != StatusTypes.Delete.ToInt());
                if (enabledTax != null) enabledTax.EnableForPos = false;
            }
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //11
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var isExist = await IsExist(model: model);
                    var isAccountExist = await _accountRepository.IsExist(accAccountDto: new AccAccountDto
                    {
                        Title = $"{model.Name} (Acc. Payable)",
                        ParentId = Account.TaxCollected.ToInt(),
                        AccNo = model.AccountNo,
                        CompanyId = model.CompanyId,
                        Id = data.AccountId
                    });

                    if (isExist || isAccountExist)
                    {
                        model.Response = Response.Error(isExist ? $"{model.Name} Already Exists." : "Account No. Already Exists.");
                        return model;
                    }

                    data.Name = model.Name;
                    data.Amount = model.Amount;
                    data.IsInPercent = model.IsInPercent;
                    data.EnableForPos = model.EnableForPos;
                    data.CompanyId = model.CompanyId;
                    if (model.Status.HasValue) data.Status = model.Status;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;



                    var taxAccountEdit = await _accountRepository.GetDetails(accountId: data.AccountId,
                                                                         companyId: data.CompanyId);
                    if (taxAccountEdit != null)
                    {
                        if (taxAccountEdit.AccNo != model.AccountNo ||
                            taxAccountEdit.Title != $"{model.Name} (Acc. Payable)")
                        {
                            taxAccountEdit.AccNo = model.AccountNo;
                            taxAccountEdit.Title = $"{model.Name} (Acc. Payable)";
                            taxAccountEdit.ModifiedBy = data.ModifiedBy;
                            await _accountRepository.Edit(accAccountDto: _mapper.Map<AccAccountDto>(source: taxAccountEdit),
                                                          forceEdit: true);
                        }
                    }
                    else
                    {
                        //create payable account for this tax collection.
                        var taxAccountCreate = await _accountRepository.Create(accAccountDto: new AccAccountDto
                        {
                            Title = $"{model.Name} (Acc. Payable)",
                            AccountTypeId =AccountType.Liability.ToInt(),
                            ParentId = Account.TaxCollected.ToInt(),
                            AccNo = model.AccountNo,
                            SystemMade = true,
                            IsParent = false,
                            AllowForManualTransaction = true,
                            CompanyId = data.CompanyId,
                            CreatedBy = data.CreatedBy,
                            CreatedOn = data.CreatedOn
                        }, isEditable: false);

                        if (taxAccountCreate.Id.HasValue) data.AccountId = taxAccountCreate.Id.Value;
                    }

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var returnModel = _mapper.Map<TaxDto>(source: data);
                    returnModel.Response = Response.Message("Tax Updated Successfully.", StatusCodes.Updated);
                    return returnModel;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<RptTaxCollectionDto> TaxCollectionReport( RptTaxCollectionDto rptTaxCollectionDto )
        {
            var taxCollectionReportData = await _dbContext
                                             .Rpt_Tax_GetTaxReport(rptTaxCollectionDto)
                                             .ConfigureAwait(continueOnCapturedContext: true);
            rptTaxCollectionDto.TaxCollectionData = (from DataRow dr in taxCollectionReportData.Rows
                 select new RptTaxCollectionRowDto
                 {
                     Id = Convert.ToInt32(dr["Id"]),
                     Name = Convert.ToString(dr["Name"]),
                     TaxAmount = Convert.ToDecimal(dr["TaxAmount"]),
                 }).ToList();
            return rptTaxCollectionDto;
        }
        public async Task<List<TaxDto>> GetAll(TaxDto model)
        {
            var query = _dbContext.Tax.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId);
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
                .Select(x=>new TaxDto { Id = x.Id, Name = x.Name, Amount = x.Amount, IsInPercent = x.IsInPercent, EnableForPos = x.EnableForPos??false, Status = x.Status })
                .ToListAsync();
            return data;
        }


        public async Task<TaxDto> GetDetails(TaxDto model)
        {
            var data = await _dbContext.Tax.AsNoTracking()
                .Select(selector: tax =>
                    new
                    {
                        tax = new TaxDto {
                            Id = tax.Id, Name = tax.Name, Amount = tax.Amount, IsInPercent = tax.IsInPercent, EnableForPos = tax.EnableForPos ?? false, Status = tax.Status,
                            AccountId = tax.AccountId, CreatedBy = tax.CreatedBy, CreatedOn = tax.CreatedOn, ModifiedBy = tax.ModifiedBy, ModifiedOn = tax.ModifiedOn,
                            CompanyId = tax.CompanyId
                        },
                        cu = UserWithRoleSelect.FirstOrDefault(cu => tax.CreatedBy == cu.Id),
                        bu = UserWithRoleSelect.FirstOrDefault(bu => tax.ModifiedBy == bu.Id)
                    }).FirstOrDefaultAsync(predicate: x => x.tax.Id == model.Id && x.tax.CompanyId == model.CompanyId);
            
            if (data is null) return null;
            //data.cu.Company.Branch = await _dbContext.Branch.AsNoTracking().Where(predicate: c => c.CompanyId == model.CompanyId).Select(x => new BranchDto { Id = x.Id, Name = x.Name }).ToListAsync();
            
            
            //get vendor account no
            var taxAccount = await _accountRepository.GetDetails(accountId: data.tax.AccountId, companyId: data.tax.CompanyId);
            var dataModel = data.tax;
            dataModel.AccountNo = taxAccount?.AccNo;
            dataModel.CreatedByUser = data.cu;
            dataModel.ModifiedByUser = data.bu;
            return dataModel;
        }


        public async Task<TaxDto> GetEnabledForPos(int companyId)
        {
            return await _dbContext.Tax.AsNoTracking()
                .Select(x=> new TaxDto{ 
                    Id = x.Id, Name = x.Name, Amount = x.Amount, IsInPercent = x.IsInPercent, AccountId = x.AccountId, Status = x.Status,
                    EnableForPos = x.EnableForPos??false, CreatedBy=x.CreatedBy, CreatedOn = x.CreatedOn,CompanyId = x.CompanyId
                }).SingleOrDefaultAsync(predicate: tax => tax.EnableForPos && tax.CompanyId == companyId && tax.Status == StatusTypes.Active.ToInt());
        }


        public async Task<bool> IsExist(TaxDto model)
        {
            return await _dbContext.Tax.AsNoTracking()
                .AnyAsync(predicate: x =>
                    x.Status != StatusTypes.Delete.ToInt() &&
                    x.Name == model.Name &&
                    x.CompanyId == model.CompanyId &&
                    x.Id != model.Id);
        }
    }
}