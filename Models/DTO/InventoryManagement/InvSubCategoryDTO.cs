using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvSubCategoryDto : BaseModel
    {
        public InvSubCategoryDto()
        {

            SubCategories = new List<InvSubCategoryDto>();
        }

        [DisplayName("Category Code")]
        public string CategoryCode { get; set; }

        public string SubCategoryName { get; set; }

        [DisplayName("Display Image")]
        public string ImageUrl { get; set; }

        [DisplayName("Display On POS")]
        public bool DisplayOnPos { get; set; }
        public string DisplayOnPosText => DisplayOnPos == false ? "No" : "Yes";

        [DisplayName("Parent Category")]
        public int? CategoryId { get; set; }

        //public int? CompanyId { get; set; }

        public InvCategoryDto Category { get; set; }

        //dto property
        public List<InvSubCategoryDto> SubCategories { get; set; }
    }
}
