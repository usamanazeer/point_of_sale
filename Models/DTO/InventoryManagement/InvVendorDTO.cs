using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvVendorDto : ExtendedBaseModel
    {
        public InvVendorDto()
        {
            InvGrnMaster = new List<InvGrnMasterDto>();
            InvPoMaster = new List<InvPoMasterDto>();
            InvPhysicalInventoryItem = new List<InvPhysicalInventoryItemDto>();

            Vendors = new List<InvVendorDto>();
        }

        [DisplayName("Vendor Code")]
        public string VendorCode { get; set; }
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }


        [Range(0, Int64.MaxValue, ErrorMessage = "Phone number should not contain characters.")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Phone number should have minimum 7 digits.")]
        public string Phone { get; set; }


        [Range(0, Int64.MaxValue, ErrorMessage = "Mobile number should not contain characters.")]
        [StringLength(20, MinimumLength = 11, ErrorMessage = "Mobile number should have minimum 11 digits.")]
        public string Mobile { get; set; }

        [DisplayName("Account No")]
        public string AccountNo { get; set; }

        [DisplayName("Primary Email")]
        public string PrimaryEmail { get; set; }
        [DisplayName("Other Email")]
        public string OtherEmail { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public int? AccountId { get; set; }

        public IList<InvGrnMasterDto> InvGrnMaster { get; set; }
        public IList<InvPoMasterDto> InvPoMaster { get; set; }
        public IList<InvPhysicalInventoryItemDto> InvPhysicalInventoryItem { get; set; }

        //DTO prop
        public List<InvVendorDto> Vendors { get; set; }

    }
}
