using System;
using Models;
using AutoMapper;
using System.Linq;
using POS_API.Data;
using System.Threading.Tasks;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StatusType = Models.Enums.StatusTypes;

namespace POS_API.Repositories.UserManagement.RightsRepos
{
    public class RightsRepository :RepositoryBase, IRightsRepository, IRepository
    {
        public RightsRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> ChangeStatus(RightsDto model)
        {
            var data = await _dbContext.Role.FindAsync(model.Id);
            if (data is null) return false;
            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        [Obsolete]
        public async Task<bool> ClaimRight(RoleRightsDto model)
        {
            //FOR SUPER ADMIN
            var superAdmin = (await (from role in _dbContext.Role
                where role.Id == model.RoleId &&
                role.CompanyId == model.CompanyId &&
                role.Editable == false
                select role).ToListAsync()).FirstOrDefault();
            if (superAdmin != null)
            {
                return true;
            }
            var right = (await (from roleRights in _dbContext.RoleRights
                join rights in _dbContext.Rights
                    on roleRights.RightId equals rights.Id
                join role in _dbContext.Role
                on roleRights.RoleId equals role.Id
                where
                    roleRights.CompanyId == model.CompanyId &&
                    roleRights.RoleId == model.RoleId &&
                    roleRights.Right.Area == model.Right.Area &&
                    roleRights.Right.Controller == model.Right.Controller &&
                    roleRights.Right.Action == model.Right.Action
                    && rights.Status == StatusType.Active.ToInt()
                select rights).ToListAsync()).FirstOrDefault();
            return right != null;
        }

        public async Task<RightsDto> ClaimRight1(RoleRightsDto model)
        {
            //FOR SUPER ADMIN
            var superAdmin = (await (from role in _dbContext.Role
                where role.Id == model.RoleId &&
                role.CompanyId == model.CompanyId &&
                role.Editable == false
                select role).ToListAsync()).FirstOrDefault();
            if (superAdmin != null)
            {
                return new RightsDto();
            }
            var right = (await (from roleRights in _dbContext.RoleRights
                join rights in _dbContext.Rights
                    on roleRights.RightId equals rights.Id
                join role in _dbContext.Role
                on roleRights.RoleId equals role.Id
                where
                roleRights.CompanyId == model.CompanyId && rights.Status == StatusType.Active.ToInt() && roleRights.RoleId == model.RoleId &&
                ((roleRights.Right.Area == model.Right.Area && roleRights.Right.Controller == model.Right.Controller && roleRights.Right.Action == model.Right.Action) 
                  || (roleRights.Right.Name ==model.Right.Name)) select rights).ToListAsync()).FirstOrDefault();
            
            return Map<RightsDto>(right);
        }
        public async Task<RightsDto> ClaimRightByName(RoleRightsDto model)
        {
            //FOR SUPER ADMIN
            var superAdmin = (await (from role in _dbContext.Role
                where role.Id == model.RoleId &&
                role.CompanyId == model.CompanyId &&
                role.Editable == false
                select role).ToListAsync()).FirstOrDefault();

            if (superAdmin != null)
                return new RightsDto();

            var right = (await (from roleRights in _dbContext.RoleRights
                join rights in _dbContext.Rights
                    on roleRights.RightId equals rights.Id
                join role in _dbContext.Role
                on roleRights.RoleId equals role.Id
                where
                    roleRights.CompanyId == model.CompanyId &&
                    roleRights.RoleId == model.RoleId &&
                    roleRights.Right.Area == model.Right.Area &&
                    roleRights.Right.Controller == model.Right.Controller &&
                    roleRights.Right.Action == model.Right.Action
                    && rights.Status == StatusType.Active.ToInt()
                select rights).ToListAsync()).FirstOrDefault();

            return Map<RightsDto>(right);
        }
        public async Task<RightsDto> Create(RightsDto model)
        {
            model.CreatedOn = DateTime.Now;
            var data = _mapper.Map<Rights>(model);
            await _dbContext.Rights.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<RightsDto> Edit(RightsDto model)
        {
            var data = await _dbContext.Rights.FindAsync(model.Id);
            if (data == null) return null;

            data.Name = model.Name;
            data.DisplayName = model.DisplayName;
            data.Url = model.Url;
            data.ParentId = model.ParentId;
            data.SequenceNo = model.SequenceNo;
            data.ModifiedBy = model.ModifiedBy;
            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RightsDto>(data);
        }

        public async Task<IEnumerable<CompanyModulesDto>> GetAll(CompanyModulesDto model)
        {
            var query = _dbContext.CompanyModules.Include(m => m.Module.Rights)
                .Where(c => c.CompanyId == model.CompanyId && c.Status != StatusType.Secret.ToInt() && c.Status != StatusType.Delete.ToInt());

            if (model.Status.HasValue)
                query = query.Where(r => r.Status == model.Status.Value);
            if (model.Id.HasValue)
                query = query.Where(r => r.Id == model.Id.Value);

            var result = await query.OrderBy(x => x.ModuleId).ToListAsync();
            var data = _mapper.Map<List<CompanyModulesDto>>(result);
            DepthLevelSelection(data);
            return data;
        }


        private void DepthLevelSelection(List<CompanyModulesDto> data)
        {
            foreach (var companyModule in data)
            {
                companyModule.Module.Rights = companyModule.Module.Rights.Where(x => x.DepthLevel == 1).ToList();
                foreach (var firstLevelRight in companyModule.Module.Rights)
                {
                    firstLevelRight.InverseParent = firstLevelRight.InverseParent.Where(x => x.DepthLevel == 2).ToList();
                    foreach (var secondLevelRight in firstLevelRight.InverseParent)
                    {
                        secondLevelRight.InverseParent = secondLevelRight.InverseParent.Where(x => x.DepthLevel == 3).ToList();
                        foreach (var thirdLevelRight in secondLevelRight.InverseParent)
                        {
                            thirdLevelRight.InverseParent = thirdLevelRight.InverseParent.Where(x => x.DepthLevel == 4).ToList();
                            foreach (var fourthLevelRight in thirdLevelRight.InverseParent)
                            {
                                fourthLevelRight.InverseParent = fourthLevelRight.InverseParent.Where(x => x.DepthLevel == 5).ToList();
                            }
                        }
                    }
                }
            }
        }


        public async Task<IEnumerable<RightModel>> GetByRole(RoleDto role) 
        {
            var userRole =  await _dbContext.Role.FindAsync(role.Id);
            //for admin
            if (userRole.Editable == false)
            {
                //select all
                var rights = await (from module in _dbContext.Module
                    join companyModule in _dbContext.CompanyModules
                    on module.Id equals companyModule.ModuleId
                    join right in _dbContext.Rights
                    on module.Id equals right.ModuleId
                    where companyModule.CompanyId == role.CompanyId && right.Status == StatusType.Active.ToInt()
                    orderby right.SequenceNo
                    select right).ToListAsync();
                return _mapper.Map<List<RightModel>>(rights);
            }
            else
            {
                var rights = await _dbContext.RoleRights.Join(_dbContext.Rights, rr => rr.RightId,
                r => r.Id, (rr, r) => new { rr, r }).Where(c => c.rr.CompanyId == role.CompanyId && c.rr.RoleId == role.Id)
                .Select(c => c.r)
                .OrderBy(c => c.SequenceNo).ToListAsync();
                return _mapper.Map<List<RightModel>>(rights);
            }
        }
    }
}