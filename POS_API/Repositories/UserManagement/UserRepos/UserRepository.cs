using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.UserManagement;
using POS_API.Data;
using StatusType = Models.Enums.StatusTypes;

namespace POS_API.Repositories.UserManagement.UserRepos
{
    public class UserRepository: RepositoryBase, IUserRepository,IRepository
    {
        public UserRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<UserDto> Create(UserDto user)
        {
            user.Company = null;
            user.Role = null;
            user.MainUser = user.MainUser;
            var data = _mapper.Map<User>(user);
            await _dbContext.User.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<UserDto>(data);
        }

        public async Task<UserDto> Edit(UserDto user)
        {
            user.Company = null;
            user.Role = null;
            var data = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == user.Id && x.CompanyId == user.CompanyId);
            if (data is null) return null;

            data.FirstName = user.FirstName;
            data.LastName = user.LastName;
            data.UserName = user.UserName;
            data.Gender = user.Gender;
            data.Phone = user.Phone;
            data.PrimaryEmail = user.PrimaryEmail;
            data.OtherEmail = user.OtherEmail;

            if (data.MainUser != true)
            {
                if (user.Status.HasValue) data.Status = user.Status.Value;
                data.RoleId = user.RoleId;
            }
            data.ModifiedBy = user.ModifiedBy;
            data.ModifiedOn = user.ModifiedOn;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<UserDto>(data);
        }

        public async Task<List<UserDto>> GetAll(UserDto model)
        {
            var query = _dbContext.User.Include(c => c.Company).Include(r => r.Role).Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (!model.Id.HasValue)
            {
                if (!model.DisplayDeleted)
                {
                    query = query.Where(c => c.Status != StatusType.Delete.ToInt());
                }

                if (model.Status.HasValue)
                {
                    query = query.Where(r => r.Status == model.Status);
                }
            }
            else
            {
                query = query.Where(r => r.Id == model.Id);
            }

            var data = _mapper.Map<List<UserDto>>(await query.AsNoTracking().ToListAsync());
            return (data);
        }

        public async Task<UserDto> GetById(UserDto model)
        {
            var data = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(u => u.CompanyId == model.CompanyId && u.Id == model.Id.Value);
            return _mapper.Map<UserDto>(data);
        }

        public async Task<bool> IsExist(UserDto user)
        {
            return await _dbContext.User.AsNoTracking()
                .AnyAsync(x => x.Status != StatusType.Delete.ToInt() &&
                                               ((x.UserName == user.UserName) || x.PrimaryEmail == user.PrimaryEmail) &&
                                               x.Id != user.Id);
        }

        public async Task<UserDto> Login(AuthenticationDto user)
        {
            var data = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(x => x.Status == StatusType.Active.ToInt() && x.Role.Status == StatusType.Active.ToInt() && x.UserName == user.UserName && x.Password == user.Password);
            var model = _mapper.Map< UserDto >(data);
            return model;
        }

        public async Task<UserDto> Register(UserDto userDto)
        {
            userDto.Status = StatusType.Active.ToInt();
            var userEntity = _mapper.Map<User>(userDto);
            await _dbContext.User.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
            return userDto;
        }
        public async Task<bool> Delete(UserDto userDto)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userDto.Id && x.CompanyId == userDto.CompanyId && x.MainUser == false && x.Status != StatusType.Delete.ToInt());
            if (user == null) return false;

            user.ModifiedBy = userDto.ModifiedBy;
            user.ModifiedOn = userDto.ModifiedOn;
            user.Status = StatusType.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
