using AutoMapper;
using Models.DTO.UserManagement;
using Models.DTO.ViewModels.SelectList.UserManagement;
using Models.Enums;
using POS_API.Data;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace POS_API.Repositories.UserManagement.BranchRepos
{
    public class BranchRepository : RepositoryBase, IBranchRepository, IRepository
    {
        public BranchRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<BranchDto> Create(BranchDto model)
        {
            var data = _mapper.Map<Branch>(model);
            data.IsMainBranch ??= false;
            data.Status = Convert.ToInt16(StatusTypes.Active);
            await _dbContext.Branch.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return model;
        }
        public async Task<BranchDto> Edit(BranchDto model)
        {
            var data = await _dbContext.Branch.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            if (data == null) return null;

            data.Name = model.Name;
            data.Phone = model.Phone;
            data.Address = model.Address;
            data.City = model.City;
            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<BranchDto>(await GetDetails(model));
        }
        public async Task<bool> Delete(BranchDto model)
        {
            var modifier = await _dbContext.Branch.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsMainBranch == false && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            if (modifier == null) return false;

            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = Convert.ToInt16(StatusTypes.Delete);
            await _dbContext.SaveChangesAsync();
            return true;

        }
        public async Task<IList<BranchDto>> GetAll(BranchDto model)
        {
            var query = _dbContext.Branch.AsNoTracking().Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted)
                    query = query.Where(c => c.Status != StatusTypes.Delete.ToInt());

                if (model.Status.HasValue)
                    query = query.Where(r => r.Status == model.Status);
            }
            return await query.OrderBy(x => x.IsMainBranch)
                .Select(x=> new BranchDto { Id = x.Id, Name = x.Name, IsMainBranch = x.IsMainBranch, Status = x.Status, CreatedOn = x.CreatedOn, ModifiedOn = x.ModifiedOn}).ToListAsync();
        }
        public async Task<BranchDto> GetDetails(BranchDto model)
        {
            var data = await _dbContext.Branch.AsNoTracking().Include(x => x.Company).Select(Branch => 
                new {
                    Branch,
                    CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => Branch.CreatedBy == cu.Id),
                    ModifiedByUser = UserWithRoleSelect.FirstOrDefault(mu => Branch.ModifiedBy == mu.Id)
            }).FirstOrDefaultAsync(x => x.Branch.Id == model.Id && x.Branch.CompanyId == model.CompanyId);
            
            if (data == null) return null;
            var retData = Map<BranchDto>(data.Branch);
            retData.CreatedByUser = data.CreatedByUser;
            retData.ModifiedByUser = data.ModifiedByUser;
            return retData;
        }
        public async Task<bool> IsExists(BranchDto model) 
        {
            return await _dbContext.Branch.AsNoTracking()
               .AnyAsync(x => x.Status != Convert.ToInt16(StatusTypes.Delete) &&
                                   x.Name == model.Name &&
                                   x.CompanyId == model.CompanyId &&
                                   x.Id != model.Id);
        }
        public async Task<IList<Branch_SLM>> GetSelectList(BranchDto model)
        {
            return await _dbContext.Branch.AsNoTracking().Include(x=>x.Company).Where(x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .Select(x=>new Branch_SLM { 
                    Value = x.Id.ToString(), Text = x.Name, Address = x.Address, City= x.City, IsMainBranch = x.IsMainBranch, Phone = x.Phone
                })
                .ToListAsync();
        }
    }
}
