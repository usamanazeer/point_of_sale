using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using Models.DTO.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.InventoryManagement.PhysicalInventoryServices
{
    public interface IPhysicalInventoryService
    {
        Task<InvPhysicalInventoryDto> Add(string token , InvPhysicalInventoryDto model);
        Task<InvPhysicalInventoryDto> Get(string token, InvPhysicalInventoryDto model = null);
        Task<InvPhysicalInventoryViewDto> GetBillDetails(string token, int id);
        Task<InvPhysicalInventoryViewDto> GetPhysicalInventoryView(string token, PhysicalInventoryViewFilter filter);
        Task<InvPhysicalInventoryViewDto> GetLowInventory(string token, PhysicalInventoryViewFilter filters = null);
    }
}
