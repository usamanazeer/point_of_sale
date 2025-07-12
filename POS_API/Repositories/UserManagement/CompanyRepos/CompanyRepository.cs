using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.DTO.UserManagement;
using POS_API.Data;
using POS_API.Utilities;
using StatusType = Models.Enums.StatusTypes;

namespace POS_API.Repositories.UserManagement.CompanyRepos
{
    public class CompanyRepository : RepositoryBase, ICompanyRepository, IRepository
    {
        private readonly IMemoryCache _memoryCache;
        private string GetCompanyCacheKey(int companyId) => "company" + companyId;

        public CompanyRepository(PosDB_Context dbContext,
                                 IMapper mapper,
                                 IMemoryCache memoryCache) : base(dbContext,
                                                                  mapper)
        {
            _memoryCache = memoryCache;
        }
        public async Task<int> AddCompany(CompanyDto companyDto)
        {
            using (_dbContext)
            {
                var model = _mapper.Map<Company>(companyDto);
                await _dbContext.Company.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return model.Id;
            }
        }

        public async Task<CompanyDto> Edit(CompanyDto model)
        {
            var data = await (from company in _dbContext.Company 
                              join branch in _dbContext.Branch on company.Id equals branch.CompanyId into cb
                              from branch in cb.DefaultIfEmpty()
                              select new { company, branch }).FirstOrDefaultAsync(x=> x.company.Id == model.Id && x.branch.IsMainBranch == true);

            //.Where(c=>c.Id == model.Id).FirstOrDefaultAsync();
            if (data == null) return null;

            data.company.Name = model.Name;
            data.company.Email = model.Email;
            data.company.BusinessTypeId = model.BusinessTypeId;
            if (model.Status.HasValue) data.company.Status = model.Status.Value;
            data.company.ModifiedBy = model.ModifiedBy;
            data.company.ModifiedOn = model.ModifiedOn;
            

            if (data.branch != null)
            {
                data.branch.Phone = model.Branch[0].Phone;
                data.branch.Mobile = model.Branch[0].Mobile;
                data.branch.ShowPhoneOnBill = model.Branch[0].ShowPhoneOnBill;
                data.branch.ShowMobileOnBill = model.Branch[0].ShowMobileOnBill;
                data.branch.Address = model.Branch[0].Address;
                data.branch.City = model.Branch[0].City;
                data.branch.ModifiedBy = model.ModifiedBy;
                data.branch.ModifiedOn = model.ModifiedOn;
            }
            await _dbContext.SaveChangesAsync();
            if (_memoryCache.IsAvailableInCache(GetCompanyCacheKey(data.company.Id)))
            {
                //var companyData = await _dbContext.Company.Include(b => b.Branch).FirstOrDefaultAsync(c => c.Id == company.Id);
                _memoryCache.UpdateToCache(GetCompanyCacheKey(data.company.Id), data.company, 10, 5, CacheItemPriority.High);
            }
            return _mapper.Map<CompanyDto>(data.company);
        }

        public async Task<List<CompanyDto>> Get(SearchModel model)
        {
            var query = _dbContext.Company.AsNoTracking();
            //by id
            if (model.id.HasValue) 
                query = query.Where(x => x.Id == model.id.Value);
            
            //by status
            if (model.status != null)
            {
                var statusId = _dbContext.StatusType.FirstOrDefault(x => x.StatusType1 == model.status)?.Id;
                query = query.Where(x => x.Status == statusId);
            }
            return await query
                .Select( x=> new CompanyDto { Id = x.Id, Name = x.Name,LogoPhysicalPath = x.Logo })
                .ToListAsync();
             
        }

        public async Task<bool> ChangeStatus(CompanyDto model)
        {
            var data = await _dbContext.Company.FindAsync(model.Id);
            if (data == null) return false;

            if (model.Status.HasValue) data.Status = model.Status.Value;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CompanyDto> GetByUserId(int userId)
        {
            var data = await _dbContext.User.Where(u => u.Id == userId && u.Status == StatusType.Active.ToInt())
                .Select(u => u.Company).Where(c=>c.Status == StatusType.Active.ToInt()).AsNoTracking().FirstOrDefaultAsync();
            return _mapper.Map<CompanyDto>(data);
        }

        public async Task<CompanyDto> GetCompanyById(int id)
        {
            var query = _dbContext.Company.AsNoTracking().Include(b => b.Branch);
            
            //if (fromCache)
            //{
            //    if (!_memoryCache.IsAvailableInCache(GetCompanyCacheKey(id)))
            //    {
            //        var company = await query.FirstOrDefaultAsync(c => c.Id == id);
            //        _memoryCache.SetToCache(GetCompanyCacheKey(id) ,company,10,5,CacheItemPriority.High);
            //    }
            //    return _mapper.Map<CompanyDto>(_memoryCache.Get<Company>(GetCompanyCacheKey(id)));
            //}

            var data = await query.FirstOrDefaultAsync(c => c.Id == id);
            if (data!= null)
            {
                data.Branch = data.Branch.Where(b => b.IsMainBranch == true).ToList();
            }
            return _mapper.Map<CompanyDto>(data);
        }

        public async Task<bool> UpdateLogoPath(CompanyDto model)
        {
            var data = await _dbContext.Company.FindAsync(model.Id);
            if (data == null) return false;

            data.Logo = model.Logo;
            data.ModifiedBy = model.ModifiedBy;
            data.ModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SetupPrinters(CompanyDto companyDto)
        {
            var company = await _dbContext.Company.FindAsync(companyDto.Id);
            if (company == null) return false;

            company.OnDeskPrinter = companyDto.OnDeskPrinter;
            company.OffDeskPrinter = companyDto.OffDeskPrinter;
            await _dbContext.SaveChangesAsync();
            if (_memoryCache.IsAvailableInCache(GetCompanyCacheKey(company.Id)))
            {
                //var companyData = await _dbContext.Company.Include(b => b.Branch).FirstOrDefaultAsync(c => c.Id == company.Id);
                _memoryCache.UpdateToCache(GetCompanyCacheKey(company.Id), company, 10, 5, CacheItemPriority.High);
            }
            return true;
        }
    }
}
