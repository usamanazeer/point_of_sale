using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTO.RestaurantManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using Models.Enums;
using POS_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace POS_API.Repositories.RestaurantManagement.RestaurantFloorsRepos
{
    public class RestaurantFloorsRepository : RepositoryBase, IRestaurantFloorsRepository, IRepository
    {
        public RestaurantFloorsRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<RestRestaurantFloorsDto> Create(RestRestaurantFloorsDto model)
        {
            var data = _mapper.Map<RestRestaurantFloors>(model);
            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.RestRestaurantFloors.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return Map<RestRestaurantFloorsDto>(data);
        }

        public async Task<bool> Delete(RestRestaurantFloorsDto model)
        {
            var modifier = await _dbContext.RestRestaurantFloors.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            if (modifier is null) return false;

            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<RestRestaurantFloorsDto> Edit(RestRestaurantFloorsDto model)
        {
            var data = await _dbContext.RestRestaurantFloors.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            
            if (data is null) return null;
            
            data.Name = model.Name;
            data.BranchId = model.BranchId;
            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RestRestaurantFloorsDto>(await GetDetails(model));
        }

        public async Task<List<RestRestaurantFloorsDto>> GetAll(RestRestaurantFloorsDto model)
        {
            var query = _dbContext.RestRestaurantFloors.AsNoTracking()
                //.Include(x => x.RestDiningTable)
                .Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {query = query.Where(r => r.Id == model.Id);}
            else
            {
                if (!model.DisplayDeleted) query = query.Where(c => c.Status != StatusTypes.Delete.ToInt());
                if (model.Status.HasValue) query = query.Where(r => r.Status == model.Status);
            }
            return  await query.Select(floor=> new RestRestaurantFloorsDto {
                Id = floor.Id, Name = floor.Name, Status = floor.Status, CompanyId = floor.CompanyId, CreatedOn = floor.CreatedOn, ModifiedOn = floor.ModifiedOn
            }).ToListAsync();
        }

        public async Task<RestRestaurantFloorsDto> GetDetails(RestRestaurantFloorsDto model)
        {
            return (await _dbContext.RestRestaurantFloors.AsNoTracking()
                //.Include(x => x.RestDiningTable)
                .Select(floor =>
                new
                {
                    floor = new RestRestaurantFloorsDto { 
                        Id = floor.Id, Name = floor.Name, Status = floor.Status, CompanyId = floor.CompanyId, CreatedOn = floor.CreatedOn, ModifiedOn = floor.ModifiedOn,
                        CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => floor.CreatedBy == cu.Id),
                        ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => floor.ModifiedBy == bu.Id)
                    }
                }).FirstOrDefaultAsync(x => x.floor.Id == model.Id && x.floor.CompanyId == model.CompanyId))?.floor;

            //if (data is null)return null;
            //data.CreatedByUser.Company.Branch = await _dbContext.Branch.Select(x=> new BranchDto { }).Where(c => c.CompanyId == model.CompanyId).AsNoTracking().ToListAsync();
            //remove deleted
            //data.floor.RestDiningTable = data.floor.RestDiningTable.Where(x => x.Status == StatusTypes.Active.ToInt()).ToList();
            //return data;
        }

        public async Task<IList<RestRestaurantFloors_SLM>> GetSelectList(RestRestaurantFloorsDto model)
        {
            return await _dbContext.RestRestaurantFloors.AsNoTracking().Where(x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .Select(x=>new RestRestaurantFloors_SLM { 
                    Value = x.Id.ToString(), Text = x.Name, BranchId = x.BranchId
                }).ToListAsync();
        }
        public async Task<bool> IsExist(RestRestaurantFloorsDto model)
        {
            return await _dbContext.RestRestaurantFloors.AsNoTracking().AnyAsync(x =>  x.Status != StatusTypes.Delete.ToInt() &&
                x.Name == model.Name && x.BranchId == model.BranchId && x.CompanyId == model.CompanyId && x.Id != model.Id);
        }
    }
}
