using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTO.InventoryManagement;
using Models.DTO.UserManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Models.Enums;
using POS_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace POS_API.Repositories.InventoryManagement.PurchaseOrderRepos
{
    // ReSharper disable once InconsistentNaming
    public class PORepository : RepositoryBase, IPORepository, IRepository
    {
        public PORepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext, mapper){}

        public async Task<InvPoMasterDto> Create(InvPoMasterDto model)
        {
            
            var data = _mapper.Map<InvPoMaster>(model);
            data.Pono = await GetNextPoNo(model);
            foreach (var item in data.InvPodetails)
            {
                item.CompanyId = data.CompanyId;
                item.CreatedBy = data.CreatedBy;
                item.CreatedOn = data.CreatedOn;
            }
            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.InvPoMaster.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return Map<InvPoMasterDto>(data);
        }

        public async Task<bool> Delete(InvPoMasterDto model)
        {
            var modifier = await _dbContext.InvPoMaster.Where(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt()).FirstOrDefaultAsync();
            if (modifier is null) return false;

            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<InvPoMasterDto> Edit(InvPoMasterDto model)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //15
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var poDetailsOld = await _dbContext.InvPodetails.Where(x => x.PoId == model.Id).ToListAsync();
                    _dbContext.InvPodetails.RemoveRange(poDetailsOld);
                    await _dbContext.SaveChangesAsync();

                    var data = await _dbContext.InvPoMaster.Include(x => x.InvPodetails).Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    if (data == null) return null;

                    if (model.PoDate.HasValue) data.Podate = (DateTime)model.PoDate;
                    if (model.DeliveryDate.HasValue) data.DeliveryDate = (DateTime)model.DeliveryDate;
                    data.VendorId = model.VendorId ?? 0;
                    data.Description = model.Description;
                    if (model.Status.HasValue) data.Status = model.Status.Value;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;
                    //add new
                    foreach (var item in model.InvPoDetails)
                    {
                        data.InvPodetails.Add(Map<InvPodetails>(item));
                    }

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return _mapper.Map<InvPoMasterDto>(await GetDetails(model));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // ReSharper disable once PossibleIntendedRethrow
                    throw ex;
                }
            });
            
        }

        public async Task<List<InvPoMasterDto>> GetAll(InvPoMasterDto model)
        {
            var query = _dbContext.InvPoMaster.AsNoTracking().Include(x => x.Vendor).Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted)
                {
                    query = query.Where(c => c.Status != StatusTypes.Delete.ToInt());
                }

                if (model.Status.HasValue)
                {
                    query = query.Where(r => r.Status == model.Status);
                }
            }
            var data = await query.
                Select(x=> new InvPoMasterDto { 
                    Id = x.Id, PoDate = x.Podate, PoNo = x.Pono, DeliveryDate = x.DeliveryDate, Description = x.Description, VendorId = x.VendorId, Status = x.Status,
                    CreatedOn = x.CreatedOn, ModifiedOn = x.ModifiedOn,
                    Vendor = x.Vendor != null ?new InvVendorDto { 
                        Id = x.Vendor.Id, ContactName = x.Vendor.ContactName, CompanyName = x.Vendor.CompanyName
                    }:null
                }).ToListAsync();
            return (data);
        }

        public async Task<InvPoMasterDto> GetDetails(InvPoMasterDto model)
        {
            
            var data = (await _dbContext.Set<InvPoMaster>().AsNoTracking().Include(x => x.InvPodetails).ThenInclude(x => x.Item).Include(x=>x.Vendor).Where(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt())
                .Select(po =>
                    new
                    {   po = new InvPoMasterDto { 
                            Id = po.Id, PoNo = po.Pono, PoDate = po.Podate, DeliveryDate = po.DeliveryDate, VendorId = po.VendorId, Description = po.Description, Status = po.Status,
                            CreatedOn = po.CreatedOn, ModifiedOn = po.ModifiedOn,
                            Vendor = new InvVendorDto { Id = po.Vendor.Id, ContactName = po.Vendor.ContactName, CompanyName = po.Vendor.CompanyName }
                        },
                        podetails = po.InvPodetails.Select(dt=> new InvPoDetailsDto { 
                            Id = dt.Id, PoId = dt.PoId, ItemId = dt.ItemId, RequestedQuantity = dt.RequestedQuantity, Rate = dt.Rate, Status = dt.Status,
                            Item = new InvItemDto { 
                                Id = dt.Item.Id, Name = dt.Item.Name, FullName = dt.Item.FullName, PurchaseRate = dt.Item.PurchaseRate, SalesRate = dt.Item.SalesRate, FinalSalesRate = dt.Item.FinalSalesRate
                            },
                        }).ToList(),
                        
                        cu = _dbContext.Set<User>().Include(r=>r.Role).Include(c => c.Company)
                        .Select(x => new UserDto
                        {
                            Id = x.Id,FirstName = x.FirstName,LastName = x.LastName,
                            Role = new RoleDto{Id = x.Role.Id,Name = x.Role.Name},
                            Company = new CompanyDto { Id = x.Company.Id,Name = x.Company.Name, Logo = x.Company.Logo}
                        }).FirstOrDefault(cu => po.CreatedBy == cu.Id),
                        
                        bu = _dbContext.Set<User>().Include(r => r.Role)
                        .Select(x => new UserDto
                        {
                            Id = x.Id,FirstName = x.FirstName,LastName = x.LastName,
                            Role = new RoleDto{Id = x.Role.Id,Name = x.Role.Name}
                        }).FirstOrDefault(bu => po.ModifiedBy == bu.Id),
                        
                        branch = _dbContext.Branch.AsNoTracking().Where(c => c.CompanyId == model.CompanyId)
                        .Select(x => new BranchDto{Id = x.Id,Name = x.Name,Address = x.Address, IsMainBranch = x.IsMainBranch}).ToList()}).ToListAsync()).FirstOrDefault();
           
            if (data is null) return null;
            data.cu.Company.Branch = data.branch;
            data.po.InvPoDetails = data.podetails.Where(x => x.Status == StatusTypes.Active.ToInt()).ToList();
            data.po.CreatedByUser = data.cu;
            data.po.ModifiedByUser = data.bu;

           
            return data.po;
        }
        //public Task<bool> IsExist(InvPoMasterDTO model)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<IList<InvPoMaster_SLM>> GetSelectList(InvPoMasterDto model)
        {
            var query = _dbContext.InvPoMaster.AsNoTracking().Where(x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt());
            if (model.VendorId.HasValue) query = query.Where(x => x.VendorId == model.VendorId);
            return await query
                         .Select(x=> new InvPoMaster_SLM { 
                                                             Value = x.Id.ToString(), Text =x.Pono, PoNo = x.Pono, DeliveryDate= x.DeliveryDate, Description = x.Description, PoDate = x.Podate, VendorId = x.VendorId 
                                                         })
                         .ToListAsync();
        }

        private async Task<string> GetNextPoNo(InvPoMasterDto model)
        {
            var next = ((await _dbContext.InvPoMaster.AsNoTracking().Where(x => x.CompanyId == model.CompanyId && ((DateTime)x.CreatedOn).Date == DateTime.Now.Date).CountAsync()) + 1);
            return "PO-" + next.ToString().PadLeft(4, '0');
        }
    }
}
