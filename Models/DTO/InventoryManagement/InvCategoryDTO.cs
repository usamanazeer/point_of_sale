using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.InventoryManagement
{
    public sealed class InvCategoryDto : BaseModel
    {
        public InvCategoryDto()
        {
            InvSubCategory = new List<InvSubCategoryDto>();
            MainCategories = new List<InvCategoryDto>();
        }

        [DisplayName("Category Code")]
        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        [DisplayName("Display Image")]
        public string ImageUrl { get; set; }

        [DisplayName("Display On POS")]
        public bool DisplayOnPos { get; set; }
        public string DisplayOnPosText => DisplayOnPos ? "Yes" : "No";

        public IList<InvSubCategoryDto> InvSubCategory { get; set; }

        //dto property
        public IList<InvCategoryDto> MainCategories { get; set; }
    }
}
