using System.Collections.Generic;
namespace Models.DTO.InventoryManagement
{
    public class InvBrandDto : ExtendedBaseModel
    {
        public InvBrandDto()
        {
            InvItem = new List<InvItemDto>();
            Brands = new List<InvBrandDto>();
        }


        public string Name { get; set; }
        //public int? CompanyId { get; set; }

        public List<InvItemDto> InvItem { get; set; }
        //dto property
        public List<InvBrandDto> Brands { get; set; }
    }
}
