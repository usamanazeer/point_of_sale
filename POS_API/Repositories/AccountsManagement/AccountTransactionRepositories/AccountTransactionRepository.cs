using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Accounts;
using Models.Enums;
using POS_API.Data;

namespace POS_API.Repositories.AccountsManagement.AccountTransactionRepositories
{
    public class AccountTransactionRepository : RepositoryBase, IAccountTransactionRepository, IRepository
    {
        public AccountTransactionRepository(PosDB_Context dbContext,
                                            IMapper mapper) : base(dbContext: dbContext,
                                                                   mapper: mapper)
        {}


        public async Task<AccTransactionMasterDto> Create(AccTransactionMasterDto accTransactionMaster)
        {
            await TransactionStrategy.Execute(async () =>
            {
                //6
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var data = _mapper.Map<AccTransactionMaster>(source: accTransactionMaster);
                    data.TransactionId = await GetNextTransactionId(companyId: accTransactionMaster.CompanyId);

                    data.Status = StatusTypes.Active.ToInt();
                    await _dbContext.AccTransactionMaster.AddAsync(entity: data);

                    await _dbContext.SaveChangesAsync();

                    accTransactionMaster.Id = data.Id;
                    accTransactionMaster.TransactionId = data.TransactionId;

                    if (accTransactionMaster.SystemMade)
                        await PostToLedger(data.Id, data.CreatedBy ?? 0);

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
            return accTransactionMaster;
        }


        public async Task<List<AccTransactionMasterDto>> GetAll(AccTransactionMasterDto transactionMasterDto)
        {
            var query = _dbContext.AccTransactionMaster.AsNoTracking();
            
            if (transactionMasterDto.IncludeDetails.HasValue)
                query = query.Include(navigationPropertyPath: x => x.AccTransactionDetail);

            query = query.Where(predicate: t => t.CompanyId == transactionMasterDto.CompanyId && t.Status == StatusTypes.Active.ToInt());

            //if getting by id, don't apply filters.
            if (transactionMasterDto.FromDate.HasValue)
                query = query.Where(predicate: x => x.TransactionDate >= transactionMasterDto.FromDate);
            if (transactionMasterDto.ToDate.HasValue)
                query = query.Where(predicate: x => x.TransactionDate <= transactionMasterDto.ToDate);
            if (transactionMasterDto.SelectUnverifiedOnly.HasValue && transactionMasterDto.SelectUnverifiedOnly.Value)
                query = query.Where(predicate: x => x.IsPosted == false);
            if (transactionMasterDto.SelectVerifiedOnly.HasValue && transactionMasterDto.SelectVerifiedOnly.Value)
                query = query.Where(predicate: x => x.IsPosted);

            query = query
                .OrderBy(keySelector: x => x.TransactionDate);
            var data = await query
                .Select(x=> new AccTransactionMasterDto { 
                    Id = x.Id,
                    TransactionId = x.TransactionId,
                    Description = x.Description,
                    TransactionDate = x.TransactionDate,
                    ReferenceNo = x.ReferenceNo,
                    SystemMade = x.SystemMade,
                    IsPosted= x.IsPosted,
                    AccTransactionDetail = x.AccTransactionDetail.Select(y => new AccTransactionDetailDto { 
                        Id = y.Id, AccountId = y.AccountId, Cr = y.Cr, Dr = y.Dr, Statement = y.Statement, TransactionMasterId = y.TransactionMasterId
                    }).ToList()
                }).ToListAsync();
            return data;
        }


        public async Task<bool> VerifyJournalEntry(AccTransactionMasterDto model)
        {
            var response = await TransactionStrategy.Execute(async () =>
              {
                  // execute your logic here
                  //5
                  await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                  try
                  {
                      var transactionMaster =  await _dbContext.AccTransactionMaster.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
                      if (transactionMaster == null) return false;

                       await PostToLedger(transactionMaster.Id, transactionMaster.ModifiedBy ?? 0);

                      //transactionMaster.IsPosted = true;
                      transactionMaster.ModifiedBy = model.ModifiedBy;
                      transactionMaster.ModifiedOn = model.ModifiedOn;
                       await _dbContext.SaveChangesAsync();
                       await transaction.CommitAsync();
                      return true;
                  }
                  catch (Exception)
                  {
                      await transaction.RollbackAsync();
                      throw;
                  }
              });
            return response;
        }
        

        private async Task<string> GetNextTransactionId(int companyId)
        {
            var next = await _dbContext.AccTransactionMaster.AsNoTracking()
                                       .CountAsync(predicate: x => x.CompanyId == companyId) + 1;
            return $"Trans-{next.ToString().PadLeft(totalWidth: 4, paddingChar: '0')}";
        }
        public async Task<int> PostToLedger(int transactionId, int postedById)
        {
            var parameters = new List<SqlParameter>
                                 {
                                     new(parameterName: "@TransactionId",
                                                      value: transactionId),
                                     new(parameterName: "@PostedBy",
                                                      value: postedById)
                                 };
                return await _dbContext.Database.ExecuteSqlRawAsync(sql:
                                                             @"dbo.[Acc_JournalToLedgerPosting] @TransactionId, @PostedBy",
                                                             parameters: parameters);
                
            
        }
    }
}