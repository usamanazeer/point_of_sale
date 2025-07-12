using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Notifications;
using Models.DTO.UserManagement;
using POS_API.Data;
using StatusType = Models.Enums.StatusTypes;

namespace POS_API.Repositories.UserManagement.RoleRepos
{
    public class RoleRepository : RepositoryBase, IRoleRepository, IRepository
    {
        public RoleRepository(PosDB_Context dbContext,
                              IMapper mapper) : base(dbContext: dbContext,
                                                     mapper: mapper)
        {
        }


        public async Task<RoleDto> Create(RoleDto model)
        {
            var data = _mapper.Map<Role>(source: model);

            data.Status = StatusType.Active.ToInt();
            await _dbContext.Role.AddAsync(entity: data);
            await _dbContext.SaveChangesAsync();
            return model;
        }


        public async Task<RoleDto> Get(SearchModel model)
        {
            Role data = null;
            var query = _dbContext.Role;
            if (model.id.HasValue) data = await query.FindAsync(model.id.Value);

            var retModel = _mapper.Map<RoleDto>(source: data);
            return retModel;
        }


        public async Task<IEnumerable<RoleDto>> GetAll(RoleDto model)
        {
            var query = _dbContext.Role.Include(navigationPropertyPath: r => r.RoleRights)
                                  .Include(navigationPropertyPath: x => x.NotiRoleNotification)
                                  .Where(predicate: c =>
                                             c.CompanyId == model.CompanyId && c.Status != StatusType.Secret.ToInt());
            //if not getting by id, then apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(predicate: r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted) query = query.Where(predicate: c => c.Status != StatusType.Delete.ToInt());

                if (model.Status.HasValue) query = query.Where(predicate: r => r.Status == model.Status);
            }

            var data = _mapper.Map<List<RoleDto>>(source: await query.ToListAsync());
            return data;
        }


        public async Task<RoleDto> InActive(int id)
        {
            var role = await _dbContext.Role.FindAsync(id);
            role.Status = Convert.ToInt16(value: StatusType.InActive);
            await _dbContext.SaveChangesAsync();
            var model = _mapper.Map<RoleDto>(source: role);
            return model;
        }


        public async Task<RoleDto> Active(int id)
        {
            var role = await _dbContext.Role.FindAsync(id);
            role.Status = StatusType.Active.ToInt();
            await _dbContext.SaveChangesAsync();
            var model = _mapper.Map<RoleDto>(source: role);
            return model;
        }


        public async Task<bool> Delete(RoleDto model)
        {
            var role = await _dbContext.Role.FirstOrDefaultAsync(predicate: x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Editable && x.Status != StatusType.Delete.ToInt());
            if (role == null) return false;
            role.Status = Convert.ToInt16(value: StatusType.Delete);
            role.ModifiedBy = model.ModifiedBy;
            role.ModifiedOn = model.ModifiedOn;
            role.Status = Convert.ToInt16(value: StatusType.Delete);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<RoleDto> Edit(RoleDto model)
        {
            var data = await _dbContext
                             .Role.FirstOrDefaultAsync(predicate: x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Editable);
            
            if (data == null) return null;

            data.Name = model.Name;
            data.Description = model.Description;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = model.ModifiedOn;
            if (model.Status.HasValue) data.Status = model.Status.Value;

            var newRights = _mapper.Map<List<RoleRights>>(source: model.RoleRights);
            //delete old rights
            var oldRights = await _dbContext
                                  .RoleRights.Where(predicate: x =>
                                                        x.RoleId == model.Id && x.CompanyId == model.CompanyId)
                                  .ToListAsync();
            if (oldRights.Count > 0) _dbContext.RemoveRange(entities: oldRights);
            //save new rights
            foreach (var item in newRights)
            {
                item.CompanyId = model.CompanyId;
                if (model.Id.HasValue) item.RoleId = model.Id.Value;
                item.ModifiedBy = model.ModifiedBy;
                item.ModifiedOn = model.ModifiedOn;
                item.Status = StatusType.Active.ToInt();
                _dbContext.RoleRights.Add(entity: item);
            }

            var newNotifications = _mapper.Map<List<NotiRoleNotification>>(source: model.NotiRoleNotification);
            //delete old notifications
            var oldNotifications = await _dbContext
                                         .NotiRoleNotification
                                         .Where(predicate: x =>
                                                    x.RoleId == model.Id && x.CompanyId == model.CompanyId)
                                         .ToListAsync();
            if (oldRights.Count > 0) _dbContext.RemoveRange(entities: oldNotifications);
            //save new notifications
            foreach (var item in newNotifications)
            {
                item.CompanyId = model.CompanyId;
                if (model.Id.HasValue) item.RoleId = model.Id.Value;
                item.ModifiedBy = model.ModifiedBy;
                item.ModifiedOn = model.ModifiedOn;
                item.Status = StatusType.Active.ToInt();
                _dbContext.NotiRoleNotification.Add(entity: item);
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RoleDto>(source: data);
        }


        public async Task<bool> IsExists(RoleDto model)
        {
            return await _dbContext.Role.AsNoTracking()
                                  .AnyAsync(predicate: x =>
                                                           x.Name.ToLower() ==
                                                           (model.Name != null ? model.Name.ToLower().Trim() : "") &&
                                                           x.CompanyId == model.CompanyId &&
                                                           x.Status != StatusType.Delete.ToInt() &&
                                                           x.Id != model.Id);
            
        }


        public async Task<IList<NotiRoleNotificationDto>> GetRoleNotificationTypes(RoleDto roleDto)
        {
            var roleNotifications = await _dbContext.NotiRoleNotification
                                                    .Where(predicate: x =>
                                                               x.RoleId == roleDto.Id &&
                                                               x.CompanyId == roleDto.CompanyId).AsNoTracking().ToListAsync();
            return _mapper.Map<IList<NotiRoleNotificationDto>>(source: roleNotifications);
        }
    }
}