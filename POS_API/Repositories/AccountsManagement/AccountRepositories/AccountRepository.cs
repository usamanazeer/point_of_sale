using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Accounts;
using Models.DTO.ViewModels.SelectList.AccountsManagement;
using Models.Enums;
using POS_API.Data;

namespace POS_API.Repositories.AccountsManagement.AccountRepositories
{
    public class AccountRepository : RepositoryBase, IAccountRepository, IRepository
    {
        public AccountRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext: dbContext, mapper: mapper){}

        public async Task<IList<AccAccountTypeDto>> GetChartOfAccount(int companyId)
        {
            var chartOfAccount = await _dbContext.AccAccountType
                                                 .Include(navigationPropertyPath: at => at.AccAccount)
                                                 .Where(predicate: at => at.AccAccount.Any(x => x.CompanyId == companyId))
                                                 .ToListAsync();
            var coa = _mapper.Map<List<AccAccountTypeDto>>(source: chartOfAccount);
            foreach (var accType in coa)
                accType.AccAccount = FilterChartOfAccount(accAccount: accType.AccAccount, level: 1);
            return coa;
        }

        public async Task<AccAccountDto> Create(AccAccountDto accAccountDto, bool? isEditable = null)
        {
            var data = _mapper.Map<AccAccount>(source: accAccountDto);
            data.Code = await GetNextAccountCode(accountTypeId: data.AccountTypeId, companyId: data.CompanyId);
            //data.AccNo ??= "000" + data.Code;
            data.Status = 1;
            data.IsEditable = isEditable == null || (bool) isEditable;
            
            if (data.IsParent) data.AllowForManualTransaction = false;

            if (data.ParentId.HasValue)
            {
                var parentAcc = await _dbContext.AccAccount.Where(predicate: x => x.Id == data.ParentId).SingleOrDefaultAsync();
                data.AccLevel = parentAcc.AccLevel + 1;
            }
            else
                data.AccLevel = 1;

            await _dbContext.AccAccount.AddAsync(entity: data);
            await _dbContext.SaveChangesAsync();
            return Map<AccAccountDto>(obj: data);
        }

        public async Task<AccAccountDto> Edit(AccAccountDto accAccountDto, bool forceEdit = false)
        {
            var data = await _dbContext.AccAccount.SingleOrDefaultAsync(predicate: x => x.Id == accAccountDto.Id);

            if (data == null) return null;
            if (!data.IsEditable && forceEdit == false) return accAccountDto;

            if (data.ParentId.HasValue)
            {
                var parentAcc = await _dbContext.AccAccount.Where(predicate: x => x.Id == data.ParentId).SingleOrDefaultAsync();
                data.AccLevel = parentAcc.AccLevel + 1;
            }
            else
                data.AccLevel = 1;

            data.Title = accAccountDto.Title;
            data.AccNo = accAccountDto.AccNo;
            //data.AccountTypeId = accAccountDto.AccountTypeId;
            //data.ParentId = accAccountDto.ParentId;
            if (accAccountDto.Status.HasValue) data.Status = accAccountDto.Status.Value;
            data.ModifiedBy = accAccountDto.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<AccAccountDto>(source: await GetDetails(accAccountDto: accAccountDto));
        }

        public async Task<AccAccountDto> GetDetails(AccAccountDto accAccountDto)
        {

            //don't use AsNoTracking
            var data = (await _dbContext.AccAccount.Include(navigationPropertyPath: x => x.AccountType)
                .Include(navigationPropertyPath: x => x.Parent).Select(selector: account =>
                new
                {
                    account,
                    createdBy = UserWithRoleSelect.FirstOrDefault(x=> x.Id ==  account.CreatedBy),
                    modifiedBy = UserWithRoleSelect.FirstOrDefault(x => x.Id == account.ModifiedBy),
                }).FirstOrDefaultAsync(record => record.account.Id == accAccountDto.Id && record.account.CompanyId == accAccountDto.CompanyId));
            if (data is null) return null;
            var dataModel = Map<AccAccountDto>(obj: data.account);
            dataModel.CreatedByUser = data.createdBy;
            dataModel.ModifiedByUser =  data.modifiedBy;
            return dataModel;
        }

        public async Task<AccAccountDto> GetDetails(int accountId, int companyId) => await GetDetails(accAccountDto: new AccAccountDto { Id = accountId, CompanyId = companyId });

        public async Task<bool> IsExist(AccAccountDto accAccountDto)
        {
            return await _dbContext.AccAccount.AsNoTracking()
                                   .AnyAsync(
                                             predicate: x =>
                                                 x.ParentId == accAccountDto.ParentId &&
                                                 (x.Title == accAccountDto.Title ||
                                                  x.AccNo == accAccountDto.AccNo) &&
                                                 x.CompanyId == accAccountDto.CompanyId &&
                                                 x.Id != accAccountDto.Id
                                            );
        }

        public async Task<IList<AccAccountTypeDto>> GetAccountTypes() => _mapper.Map<IList<AccAccountTypeDto>>(source: await _dbContext.AccAccountType.AsNoTracking().ToListAsync());

        [Obsolete]
        public async Task<IList<AccAccountDto>> GetAllAccounts(int companyId) =>
            //don't use AsNoTracking
            _mapper.Map<IList<AccAccountDto>>(source: await _dbContext.AccAccount.ToListAsync());

        public async Task<IList<Account_SLM>> GetAccountsSelectList(int companyId, bool skipSystemMade = false, bool skipIfParent = false, bool selectIfParent = false,
                                                                    bool selectForManualTransactions = false, bool selectBankAccountsOnly = false)
        {
            //don't use AsNoTracking
            var query = _dbContext.AccAccount.Include(x=>x.InverseParent).Where(predicate: x => x.CompanyId == companyId && x.Status == StatusTypes.Active.ToInt());
            query = skipSystemMade ? query.Where(predicate: x => x.SystemMade == false) : query;
            query = skipIfParent ? query.Where(predicate: x => x.IsParent != true) : query;
            query = selectIfParent ? query.Where(predicate: x => x.IsParent) : query;
            query = selectForManualTransactions ? query.Where(predicate: x => x.AllowForManualTransaction) : query;
            query = selectBankAccountsOnly ? query.Where(predicate: x => x.ParentId == Account.Bank.ToInt()) : query;
            query = query.OrderBy(x => x.AccountTypeId).ThenBy(x => x.Id);
            var accounts = await query
                .Select(x=> new Account_SLM { 
                Value = x.Id.ToString(), Text = x.Title, Code = x.Code, AccNo = x.AccNo, AccountTypeId = x.AccountTypeId, IsEditable = x.IsEditable, ParentId = x.ParentId
            }).ToListAsync();
            return accounts;
        }

        public async Task<AccAccountDto[]> GetAllChildAccounts(int accountId, int companyId)
        {
            //don't use AsNoTracking
            var accounts = await _dbContext.AccAccount.Where(x => x.CompanyId == companyId).ToListAsync();
            var account = accounts.FirstOrDefault(x => x.Id == accountId);
            var accountsList = new List<AccAccount>();
            GetAccountChildren(accountsList, account);

            return _mapper.Map<List<AccAccountDto>>(accountsList).ToArray();
        }

        private void GetAccountChildren(List<AccAccount> accountsList, AccAccount account)
        {
            accountsList.Add(account);
            foreach (var acc in account.InverseParent)
                GetAccountChildren(accountsList, acc);
        }

        private async Task<string> GetNextAccountCode(int accountTypeId, int companyId)
        {
            if (accountTypeId == 0 || companyId == 0)
                throw new Exception(message: "AccountTypeId or CompanyId is null or equals to 0.");
            var lastAccount = await _dbContext.AccAccount.AsNoTracking().OrderBy(keySelector: x => x.Code)
                                    .LastOrDefaultAsync(predicate: x => x.AccountTypeId == accountTypeId && x.CompanyId == companyId);
            if (lastAccount == null) return $"{accountTypeId}-001";
            var lastAccountCode = Convert.ToInt32(value: lastAccount.Code.Split(separator: "-")[1]);
            return $"{accountTypeId}-{Convert.ToString(value: lastAccountCode + 1).PadLeft(totalWidth: 3, paddingChar: '0')}";
        }

        private IList<AccAccountDto> FilterChartOfAccount(IList<AccAccountDto> accAccount, int level)
        {
            //don't use AsNoTracking
            accAccount = accAccount.Where(predicate: x => x.AccLevel == level).ToList();
            foreach (AccAccountDto acc in accAccount)
                acc.InverseParent = FilterChartOfAccount(accAccount: acc.InverseParent, level: level + 1);
            return accAccount;
        }
    }
}