using AutoMapper;
using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using POS_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Enums;
using Microsoft.EntityFrameworkCore;
using Models;

namespace POS_API.Repositories.RestaurantManagement.WaitersRepos
{
    public class WaitersRepository :RepositoryBase, IWaitersRepository, IRepository
    {
        public WaitersRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext, mapper)
        {}

        public async Task<RestWaiterDto> Create(RestWaiterDto model)
        {
            var data = _mapper.Map<RestWaiter>(model);
            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.RestWaiter.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return Map<RestWaiterDto>(data);
        }

        public async Task<bool> Delete(RestWaiterDto model)
        {
            var modifier = await _dbContext.RestWaiter.Where(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt()).FirstOrDefaultAsync();
            if (modifier is null) return false;

            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = Convert.ToInt16(StatusTypes.Delete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<RestWaiterDto> Edit(RestWaiterDto model)
        {
            var data = await _dbContext.RestWaiter.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            if (data is null) return null;

            data.Name = model.Name;
            data.BranchId = model.BranchId;
            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RestWaiterDto>(await GetDetails(model));
        }

        public async Task<List<RestWaiterDto>> GetAll(RestWaiterDto model)
        {
            var query = _dbContext.RestWaiter.AsNoTracking().Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted) query = query.Where(c => c.Status != StatusTypes.Delete.ToInt());
                if (model.Status.HasValue) query = query.Where(r => r.Status == model.Status);
            }
            return await query
                .Select(x=> new RestWaiterDto { Id = x.Id, Name = x.Name, Status= x.Status, CompanyId = x.CompanyId, CreatedOn = x.CreatedOn, ModifiedOn = x.ModifiedOn })
                .ToListAsync();
        }

        public async Task<RestWaiterDto> GetDetails(RestWaiterDto model)
        {
            return (await _dbContext.RestWaiter.AsNoTracking()
                .Select(waiter =>
                    new
                    {
                        waiter = new RestWaiterDto {
                            Id = waiter.Id, Name = waiter.Name, Status = waiter.Status, CompanyId = waiter.CompanyId, CreatedOn = waiter.CreatedOn, ModifiedOn = waiter.ModifiedOn,
                            CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => waiter.CreatedBy == cu.Id),
                            ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => waiter.ModifiedBy == bu.Id)
                        }
                    }).FirstOrDefaultAsync(x => x.waiter.Id == model.Id && x.waiter.CompanyId == model.CompanyId))?.waiter;
            
            //if (data is null) return null;
            //data.cu.Company.Branch = await _dbContext.Branch.Where(c => c.CompanyId == model.CompanyId).AsNoTracking().ToListAsync();
        }

        public async Task<IList<RestWaiter_SLM>> GetSelectList(RestWaiterDto model)
        {
            return await _dbContext.RestWaiter.AsNoTracking().Where(x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .Select(x=>new RestWaiter_SLM { Value = x.Id.ToString(), Text=x.Name, BranchId = x.BranchId}).ToListAsync();
        }

        public async Task<bool> IsExist(RestWaiterDto model)
        {
            return await _dbContext.RestWaiter.AsNoTracking().AnyAsync(x => 
                x.Status != StatusTypes.Delete.ToInt() &&
                x.Name == model.Name &&
                x.BranchId == model.BranchId &&
                x.CompanyId == model.CompanyId &&
                x.Id != model.Id);
        }
    }
}
