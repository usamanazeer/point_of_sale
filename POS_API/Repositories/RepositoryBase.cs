using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Models.DTO.UserManagement;
using POS_API.Data;

namespace POS_API.Repositories
{
    public abstract class RepositoryBase
    {
        protected IMapper _mapper;
        protected PosDB_Context _dbContext;
        
        public RepositoryBase(PosDB_Context dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public T Map<T>(object obj) 
        {
            return _mapper.Map<T>(obj);
        }

        protected int SaveChanges() 
        {
            return _dbContext.SaveChanges();
        }
        protected Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
        protected IExecutionStrategy TransactionStrategy => _dbContext.Database.CreateExecutionStrategy();

        private static System.Linq.Expressions.Expression<Func<User, UserDto>> userWithRoleSelector = x => new UserDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Role = new RoleDto { Id = x.Role.Id, Name = x.Role.Name }
        };
        private static System.Linq.Expressions.Expression<Func<User, UserDto>> userWithRoleAndCompanySelector = x => new UserDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Role = new RoleDto { Id = x.Role.Id, Name = x.Role.Name },
            Company = new CompanyDto
            {
                Id = x.Company.Id,
                Name = x.Company.Name,
                Logo = x.Company.Logo,
            }
        };
        protected IQueryable<UserDto> UserWithRoleSelect => _dbContext.User.Include(r => r.Role).Select(userWithRoleSelector);
        protected IQueryable<UserDto> UserWithRoleAndCompanySelect => _dbContext.User.Include(r => r.Role).Include(x => x.Company).Select(userWithRoleAndCompanySelector);


    }
}
