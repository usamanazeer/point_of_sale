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

namespace POS_API.Repositories.InventoryManagement.GoodsReturnNoteRepos
{
    public class GrrnRepository : RepositoryBase, IGrrnRepository, IRepository
    {
        public GrrnRepository(PosDB_Context dbContext, IMapper mapper):base(dbContext, mapper){}

        public async Task<InvGrrnMasterDto> Create(InvGrrnMasterDto model)
        {
            var data = _mapper.Map<InvGrrnMaster>(model);
            data.GrrnNo = await GetNextGrrnNo(model);
            foreach (var item in data.InvGrrnDetails)
            {
                item.CompanyId = data.CompanyId;
                item.CreatedBy = data.CreatedBy;
                item.CreatedOn = data.CreatedOn;
            }
            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.InvGrrnMaster.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return Map<InvGrrnMasterDto>(data);
        }

        public async Task<bool> Delete(InvGrrnMasterDto model)
        {
            var modifier = await _dbContext.InvGrrnMaster.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            if (modifier is null) return false;

            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = Convert.ToInt16(StatusTypes.Delete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<InvGrrnMasterDto> Edit(InvGrrnMasterDto model)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //13
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var grrnDetailsOld = await _dbContext.InvGrrnDetails.Where(x => x.GrrnId == model.Id).ToListAsync();
                    _dbContext.InvGrrnDetails.RemoveRange(grrnDetailsOld);
                    await _dbContext.SaveChangesAsync();

                    var data = await _dbContext.InvGrrnMaster.Include(x => x.InvGrrnDetails).Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    if (data is null) return null;

                    if (model.GrrnDate.HasValue) data.GrrnDate = model.GrrnDate.Value;
                    data.VendorId = model.VendorId;
                    data.Description = model.Description;
                    if (model.Status.HasValue) data.Status = model.Status.Value;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;
                    //add new
                    foreach (var item in model.InvGrrnDetails)
                    {
                        item.CompanyId = data.CompanyId;
                        item.CreatedBy = data.ModifiedBy;
                        item.CreatedOn = data.ModifiedOn;
                        data.InvGrrnDetails.Add(Map<InvGrrnDetails>(item));
                    }

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return _mapper.Map<InvGrrnMasterDto>(await GetDetails(model));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // ReSharper disable once PossibleIntendedRethrow
                    throw ex;
                }
            });
            
        }

        public async Task<List<InvGrrnMasterDto>> GetAll(InvGrrnMasterDto model)
        {
            var query = _dbContext.InvGrrnMaster.AsNoTracking().Include(x => x.Vendor).Where(c => c.CompanyId == model.CompanyId);
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
                .Select(x=>new InvGrrnMasterDto{
                    Id = x.Id, GrrnNo = x.GrrnNo, GrrnDate= x.GrrnDate, InvoiceNo = x.InvoiceNo,Status = x.Status,VendorId = x.VendorId,
                    Vendor = new InvVendorDto { Id = x.Vendor.Id, CompanyName = x.Vendor.CompanyName, ContactName = x.Vendor.ContactName }
                }).ToListAsync();
        }

        public async Task<InvGrrnMasterDto> GetDetails(InvGrrnMasterDto model)
        {
            var data = await _dbContext.InvGrrnMaster.AsNoTracking().Include(x => x.InvGrrnDetails).ThenInclude(x => x.Item).Include(x => x.Vendor).Where(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt())
                .Select(grrn =>
                    new
                    {
                        grrn = new InvGrrnMasterDto
                        {
                            Id = grrn.Id, GrrnNo = grrn.GrrnNo, GrrnDate = grrn.GrrnDate, InvoiceNo = grrn.InvoiceNo, Description = grrn.Description, Status = grrn.Status,
                            VendorId = grrn.VendorId, CreatedBy = grrn.CreatedBy, CreatedOn = grrn.CreatedOn, ModifiedBy = grrn.ModifiedBy, ModifiedOn = grrn.ModifiedOn,
                            CompanyId = grrn.CompanyId,
                            Vendor = new InvVendorDto { Id = grrn.Vendor.Id, CompanyName = grrn.Vendor.CompanyName, ContactName = grrn.Vendor.ContactName },
                            InvGrrnDetails = grrn.InvGrrnDetails.Select(x => new InvGrrnDetailsDto
                            {
                                Id = x.Id, ItemId = x.ItemId, Item = new InvItemDto { Id = x.Item.Id, Name = x.Item.FullName },
                                BatchNo = x.BatchNo, ReturnQuantity = x.ReturnQuantity, Rate = x.Rate, GrrnId = x.GrrnId, Status = x.Status
                            }).ToList()
                        },
                        CreatedByUser = UserWithRoleAndCompanySelect.FirstOrDefault(cu => grrn.CreatedBy == cu.Id),
                        ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => grrn.ModifiedBy == bu.Id)
                    }).FirstOrDefaultAsync(x => x.grrn.Id == model.Id && x.grrn.CompanyId == model.CompanyId);

            if (data is null) return null;
            data.CreatedByUser.Company.Branch = await _dbContext.Branch.AsNoTracking()
                .Where(c => c.CompanyId == model.CompanyId && c.IsMainBranch == true)
                .Select(x => new BranchDto { Id = x.Id, Name = x.Name, Address = x.Address, IsMainBranch = x.IsMainBranch })
                .ToListAsync();
            data.grrn.InvGrrnDetails = data.grrn.InvGrrnDetails.Where(x => x.Status == StatusTypes.Active.ToInt()).ToList();
            var dataModel = data.grrn;
            dataModel.CreatedByUser = data.CreatedByUser;
            dataModel.ModifiedByUser = data.ModifiedByUser;
            return dataModel;
        }

        public async Task<IList<InvGrrnMaster_SLM>> GetSelectList(InvGrrnMasterDto model)
        {
            return await _dbContext.InvGrrnMaster.AsNoTracking().Where(x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt())
                .Select(x=>new InvGrrnMaster_SLM { 
                    Value = x.Id.ToString(), Text = x.GrrnNo, GrrnNo= x.GrrnNo, GrrnDate= x.GrrnDate, VendorId = x.VendorId, InvoiceNo = x.InvoiceNo
                }).ToListAsync();
        }

        private async Task<string> GetNextGrrnNo(InvGrrnMasterDto model)
        {
            var next = (await _dbContext.InvGrrnMaster.AsNoTracking().CountAsync(x => x.CompanyId == model.CompanyId && x.CreatedOn.Value.Date == DateTime.Now.Date) + 1);
            return "GRRN-" + next.ToString().PadLeft(4, '0');
        }
    }
}
