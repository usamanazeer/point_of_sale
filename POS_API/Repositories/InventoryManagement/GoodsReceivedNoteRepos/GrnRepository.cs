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

namespace POS_API.Repositories.InventoryManagement.GoodsReceivedNoteRepos
{
    public class GrnRepository : RepositoryBase, IGrnRepository, IRepository
    {
        public GrnRepository(PosDB_Context dbContext, IMapper mapper) : base(dbContext, mapper)
        {}
        public async Task<InvGrnMasterDto> Create(InvGrnMasterDto model)
        {
            var data = _mapper.Map<InvGrnMaster>(model);
            data.GrnNo = await GetNextGrnNo(model);
            foreach (var item in data.InvGrnDetails)
            {
                item.CompanyId = data.CompanyId;
                item.CreatedBy = data.CreatedBy;
                item.CreatedOn = data.CreatedOn;
            }
            data.Status = StatusTypes.Active.ToInt();
            await _dbContext.InvGrnMaster.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return Map<InvGrnMasterDto>(data);
        }

        public async Task<bool> Delete(InvGrnMasterDto model)
        {
            var modifier = await _dbContext.InvGrnMaster.SingleOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt());
            if (modifier is null) return false;

            modifier.ModifiedBy = model.ModifiedBy;
            modifier.ModifiedOn = model.ModifiedOn;
            modifier.Status = StatusTypes.Delete.ToInt();
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<InvGrnMasterDto> Edit(InvGrnMasterDto model)
        {
            return await TransactionStrategy.ExecuteAsync(async () =>
            {
                //12
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var grnDetailsOld = await _dbContext.InvGrnDetails.Where(x => x.GrnId == model.Id).ToListAsync();
                    _dbContext.InvGrnDetails.RemoveRange(grnDetailsOld);
                    await _dbContext.SaveChangesAsync();

                    var data = await _dbContext.InvGrnMaster.Include(x => x.InvGrnDetails).FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (data is null) return null;

                    if (model.GrnDate.HasValue) data.GrnDate = model.GrnDate.Value;
                    data.VendorId = model.VendorId;
                    data.Description = model.Description;
                    if (model.Status.HasValue) data.Status = model.Status.Value;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;
                    //add new
                    foreach (var item in model.InvGrnDetails)
                    {
                        item.CompanyId = data.CompanyId;
                        item.CreatedBy = data.ModifiedBy;
                        item.CreatedOn = data.ModifiedOn;
                        data.InvGrnDetails.Add(Map<InvGrnDetails>(item));
                    }

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return _mapper.Map<InvGrnMasterDto>(await GetDetails(model));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // ReSharper disable once PossibleIntendedRethrow
                    throw ex;
                }
            });
        }

        public async Task<List<InvGrnMasterDto>> GetAll(InvGrnMasterDto model)
        {
            var query = _dbContext.InvGrnMaster.AsNoTracking().Include(x => x.Vendor).Where(c => c.CompanyId == model.CompanyId);
            //if getting by id, don't apply filters.
            if (model.Id.HasValue)
            {
                query = query.Where(r => r.Id == model.Id);
            }
            else
            {
                if (!model.DisplayDeleted) query = query.Where(c => c.Status != StatusTypes.Delete.ToInt());

                if (model.Status.HasValue) query = query.Where(r => r.Status == model.Status);
            }
            var data = await query
                .Select(x=> new InvGrnMasterDto { 
                    Id= x.Id, GrnNo = x.GrnNo, GrnDate = x.GrnDate, InvoiceNo =x.InvoiceNo, Status = x.Status, VendorId = x.VendorId,
                    Vendor = new InvVendorDto { Id = x.Vendor.Id, CompanyName = x.Vendor.CompanyName, ContactName = x.Vendor.ContactName }
                })
                .ToListAsync();
            return (data);
        }

        public async Task<InvGrnMasterDto> GetDetails(InvGrnMasterDto model)
        {
            var data = await _dbContext.InvGrnMaster.AsNoTracking().Include(x => x.InvGrnDetails).ThenInclude(x=>x.Po).Include(x => x.InvGrnDetails).ThenInclude(x => x.Item).Include(x => x.Vendor).Where(x => x.Id == model.Id && x.CompanyId == model.CompanyId && x.Status != StatusTypes.Delete.ToInt())
                .Select(grn =>
                    new
                    {
                        grn = new InvGrnMasterDto
                        {
                            Id = grn.Id, GrnNo = grn.GrnNo, GrnDate = grn.GrnDate, InvoiceNo = grn.InvoiceNo, Description = grn.Description, Status = grn.Status, 
                            VendorId = grn.VendorId, CreatedBy = grn.CreatedBy, CreatedOn = grn.CreatedOn, ModifiedBy = grn.ModifiedBy, ModifiedOn = grn.ModifiedOn,
                            CompanyId = grn.CompanyId,
                            Vendor = new InvVendorDto { Id = grn.Vendor.Id, CompanyName = grn.Vendor.CompanyName, ContactName = grn.Vendor.ContactName },
                            InvGrnDetails = grn.InvGrnDetails.Select(x=> new InvGrnDetailsDto { 
                                Id = x.Id, ItemId = x.ItemId, Item = new InvItemDto { Id = x.Item.Id, Name = x.Item.FullName },
                                BatchNo = x.BatchNo, ReceivedQuantity = x.ReceivedQuantity, Rate = x.Rate, GrnId = x.GrnId, PoId = x.PoId, Status = x.Status
                            }).ToList()
                        },
                        CreatedByUser = UserWithRoleAndCompanySelect.FirstOrDefault(cu => grn.CreatedBy == cu.Id),
                        ModifiedByUser = UserWithRoleSelect.FirstOrDefault(bu => grn.ModifiedBy == bu.Id)
                    }).FirstOrDefaultAsync(x=>x.grn.Id == model.Id && x.grn.CompanyId == model.CompanyId);
            if (data is null) return null;

            data.CreatedByUser.Company.Branch = await _dbContext.Branch.AsNoTracking().Where(c => c.CompanyId == model.CompanyId && c.IsMainBranch == true)
                    .Select(x => new BranchDto { Id = x.Id, Name = x.Name, Address = x.Address, IsMainBranch = x.IsMainBranch }).ToListAsync();
            data.grn.InvGrnDetails = data.grn.InvGrnDetails.Where(x => x.Status == StatusTypes.Active.ToInt()).ToList();
            var dataModel = data.grn;
            dataModel.CreatedByUser = data.CreatedByUser;
            dataModel.ModifiedByUser = data.ModifiedByUser;
            return dataModel;
        }

        public async Task<IList<InvGrnMaster_SLM>> GetSelectList(InvGrnMasterDto model)
        {
            var grnMasterList = await _dbContext.InvGrnMaster.AsNoTracking().Where(x => x.CompanyId == model.CompanyId && x.Status == StatusTypes.Active.ToInt()).
                Select(x=>new InvGrnMaster_SLM {
                    Value = x.Id.ToString(), Text = x.GrnNo, GrnNo = x.GrnNo, GrnDate = x.GrnDate, InvoiceNo = x.InvoiceNo, VendorId = x.VendorId
                })
                .ToListAsync();
            return grnMasterList;
        }

        private async Task<string> GetNextGrnNo(InvGrnMasterDto model)
        {
            var next = (await _dbContext.InvGrnMaster.AsNoTracking().CountAsync(x => x.CompanyId == model.CompanyId && ((DateTime)x.CreatedOn).Date == DateTime.Now.Date) + 1);
            return "GRN-" + next.ToString().PadLeft(4, '0');
        }
    }
}
