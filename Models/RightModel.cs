using System.Collections.Generic;
namespace Models
{
    public class RightModel
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public string CssClasses { get; set; }
        public string LiCssclasses { get; set; }
        public int? ParentId { get; set; }
        public int? SequenceNo { get; set; }
        public int? DepthLevel { get; set; }
        public virtual IList<RightModel> InverseParent { get; set; }
        public List<RightModel> Rights { get; set; }
    }
}
