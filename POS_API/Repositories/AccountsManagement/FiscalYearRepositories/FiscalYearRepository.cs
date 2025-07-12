using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Accounts;
using Models.Enums;
using POS_API.Data;

namespace POS_API.Repositories.AccountsManagement.FiscalYearRepositories
{
    public class FiscalYearRepository : RepositoryBase, IFiscalYearRepository, IRepository
    {
        public FiscalYearRepository(PosDB_Context dbContext,
                                    IMapper mapper) : base(dbContext: dbContext,
                                                           mapper: mapper)
        {
        }


        public async Task<IList<AccFiscalYearDto>> GetAll(AccFiscalYearDto fiscalYearDto)
        {
            var query = _dbContext.AccFiscalYear.Where(predicate: c => c.CompanyId == fiscalYearDto.CompanyId);
            //if getting by id, don't apply filters.
            if (fiscalYearDto.Id.HasValue)
            {
                query = query.Where(predicate: r => r.Id == fiscalYearDto.Id);
            }
            else
            {
                if (!fiscalYearDto.DisplayDeleted)
                    query = query.Where(predicate: c => c.Status != StatusTypes.Delete.ToInt());

                if (fiscalYearDto.Status.HasValue)
                    query = query.Where(predicate: r => r.Status == fiscalYearDto.Status);
            }

            var data = _mapper.Map<List<AccFiscalYearDto>>(source: await query.AsNoTracking().ToListAsync());
            return data;
        }


        public async Task<AccFiscalYearDto> Create(AccFiscalYearDto fiscalYearDto)
        {
            var data = _mapper.Map<InvBrand>(source: fiscalYearDto);

            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.InvBrand.AddAsync(entity: data);
            await _dbContext.SaveChangesAsync();
            return fiscalYearDto;
        }
        public async Task<bool> IsExist(AccFiscalYearDto fiscalYearDto)
        {
            return await _dbContext.AccFiscalYear
                                   .AnyAsync(predicate: x => x.Status != StatusTypes.Delete.ToInt() &&
                                                             x.CompanyId == fiscalYearDto.CompanyId &&
                                                             x.Id != fiscalYearDto.Id);
        }
    }
}