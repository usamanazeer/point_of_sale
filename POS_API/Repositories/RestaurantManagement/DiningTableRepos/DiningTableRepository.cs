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

namespace POS_API.Repositories.RestaurantManagement.DiningTableRepos
{
    public class DiningTableRepository : RepositoryBase, IDiningTableRepository, IRepository
    {
        public DiningTableRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext, mapper) { }
        public async Task<RestDiningTableDto> Create(RestDiningTableDto model)
        {
            var data = _mapper.Map<RestDiningTable>(model);
            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.RestDiningTable.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return Map<RestDiningTableDto>(data);
        }

        public async Task<bool> Delete(RestDiningTableDto model)
        {
            var modifier = await _dbContext.RestDiningTable.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            if (modifier is null) return false;

            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<RestDiningTableDto> Edit(RestDiningTableDto model)
        {
            var data = await _dbContext.RestDiningTable.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());

            if (data is null) return null;

            data.TableNo = model.TableNo ?? 0;
            data.Capacity = model.Capacity;
            data.FloorId = model.FloorId;
            data.BranchId = model.BranchId;
            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RestDiningTableDto>(await GetDetails(model));
        }

        public async Task<List<RestDiningTableDto>> GetAll(RestDiningTableDto model)
        {
            var query = _dbContext.RestDiningTable.AsNoTracking().Include(x => x.Floor).Where(c => c.CompanyId == model.CompanyId);
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
            return await query
                .Select(x=> new RestDiningTableDto { 
                    Id = x.Id, TableNo = x.TableNo, Capacity = x.Capacity, IsOccupied = x.IsOccupied, Status = x.Status, CreatedOn = x.CreatedOn, ModifiedOn = x.ModifiedOn, CompanyId = x.CompanyId,
                    FloorId = x.FloorId, Floor = new RestRestaurantFloorsDto { Id = x.Floor.Id, Name = x.Floor.Name }
                })
                .ToListAsync();
        }

        public async Task<RestDiningTableDto> GetDetails(RestDiningTableDto model)
        {
            var data = (await _dbContext.RestDiningTable.AsNoTracking().Include(x => x.Floor)
                .Select(table =>
                    new
                    {
                        table = new RestDiningTableDto {
                            Id = table.Id,TableNo = table.TableNo,Capacity = table.Capacity, IsOccupied = table.IsOccupied, Status = table.Status, CreatedOn = table.CreatedOn,
                            ModifiedOn = table.ModifiedOn, FloorId = table.FloorId, CompanyId = table.CompanyId,
                            Floor = new RestRestaurantFloorsDto { Id = table.Floor.Id, Name = table.Floor.Name },

                            CreatedByUser = UserWithRoleSelect.FirstOrDefault(cu => table.CreatedBy == cu.Id),
                            ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => table.ModifiedBy == bu.Id)
                        }
                    }).FirstOrDefaultAsync(x => x.table.Id == model.Id && x.table.CompanyId == model.CompanyId))?.table;

            //if (data is null)return null;
            //data.CreatedByUser.Company.Branch = await _dbContext.Branch.Select(x=> new BranchDto { }).Where(c => c.CompanyId == model.CompanyId).AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<IList<RestDiningTable_SLM>> GetSelectList(RestDiningTableDto model)
        {
            var diningTables = await _dbContext.RestDiningTable.AsNoTracking().Include(x=>x.Floor).Where(x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .Select(x=>new RestDiningTable_SLM { 
                    Value = x.Id.ToString(), Text = x.TableNo.ToString(), BranchId = x.BranchId, Capacity= x.Capacity, FloorId = x.FloorId, FloorName = x.Floor.Name, IsOccupied = x.IsOccupied
                })
                .ToListAsync();
            return diningTables;
        }

        public async Task<bool> IsExist(RestDiningTableDto model)
        {
            return await _dbContext.RestDiningTable.AsNoTracking()
                   .AnyAsync(x => x.Status != StatusTypes.Delete.ToInt() &&
                                  x.FloorId == model.FloorId &&
                                  x.TableNo == model.TableNo &&
                                  x.BranchId == model.BranchId &&
                                  x.CompanyId == model.CompanyId &&
                                  x.Id != model.Id);
        }

        public async Task<bool> ReleaseOrOccupy(RestDiningTableDto model)
        {
            var modifier = await _dbContext.RestDiningTable.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            if (modifier is null) return false;

            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.IsOccupied = model.IsOccupied;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}