using Models.DTO.InventoryManagement;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.GeneralSettings
{
    public class TaxDto : ExtendedBaseModel
    {
        public TaxDto()
        {
            Taxes = new List<TaxDto>();
        }


        public string Name { get; set; }
        public decimal Amount { get; set; }

        [DisplayName(displayName: "Amount")]
        public string AmountText => IsInPercent == false ? Amount.ToString() : Amount + "%";

        [DisplayName(displayName: "In Percent")]
        public bool IsInPercent { get; set; }

        [DisplayName("Account No")]
        public string AccountNo { get; set; }
        public int AccountId { get; set; }

        public string IsInPercentText => IsInPercent == false ? "No" : "Yes";
        //public int? CompanyId { get; set; }

        [DisplayName(displayName: "Enable For Pos")]
        public bool EnableForPos { get; set; }

        public string EnableForPosText => EnableForPos == false ? "No" : "Yes";
        public virtual ICollection<InvItemDto> InvItem { get; set; }

        public virtual ICollection<InvPhysicalInventoryItemDto> InvPhysicalInventoryItem { get; set; }

        //dto prop
        public List<TaxDto> Taxes { get; set; }
    }
}