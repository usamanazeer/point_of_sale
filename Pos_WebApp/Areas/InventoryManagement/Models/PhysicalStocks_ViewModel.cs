using Models;
using Models.DTO.InventoryManagement;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
using System;
using System.Collections.Generic;

namespace Pos_WebApp.Areas.InventoryManagement.Models
{
    // ReSharper disable once InconsistentNaming
    public class PhysicalStocks_ViewModel:BaseModel
    {
        //view state
        //public int? Id { get; set; }
        public DateTime? BillDate { get; set; }
        public int? ItemId { get; set; }
        public int? BarCodeId { get; set; }
        public int? VendorId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool OnlyIfRemaining { get; set; }
        public bool OnlyNearToEnd { get; set; }
        public string Request { get; set; }
        //select lists
        public List<InvItemDto> Items { get; set; }
        public List<InvItemBarCodeDto> ItemBarCodes { get; internal set; }
        public List<InvVendorDto> Vendors { get; internal set; }
        public IList<InvPhysicalInventoryDto> PhysicalInventories { get; internal set; }

        //data
        public InvPhysicalInventoryViewDto PhysicalInventoryView { get; set; }
        
    }
}
