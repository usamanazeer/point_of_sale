using System.Collections.Generic;

namespace Models.DTO.SalesManagement
{
    public class SalesOrderTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IList<SalesOrderMasterDto> SalesOrderMaster { get; set; }
    }
}
