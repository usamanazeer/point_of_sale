using Models.DTO.SalesManagement;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvModifierDto : ExtendedBaseModel
    {
        public InvModifierDto()
        {
            InvModifierItems = new List<InvModifierItemDto>();
            InvItemModifiers = new List<InvItemModifierDto>();
            SalesOrderItemModifiers = new List<SalesOrderItemModifiersDto>();
            //dto prop init.
            Response = new Response();
        }
        public string Name { get; set; }

        [DisplayName("Modifier Charges")]
        public double ModifierCharges { get; set; }
        public IList<InvModifierItemDto> InvModifierItems { get; set; }
        public IList<InvItemModifierDto> InvItemModifiers { get; set; }
        public IList<SalesOrderItemModifiersDto> SalesOrderItemModifiers { get; set; }
        //DTO props
        public IList<InvModifierDto> Modifiers { get; set; }
    }
}