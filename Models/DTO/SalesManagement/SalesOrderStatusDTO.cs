using System;
using System.Collections.Generic;

namespace Models.DTO.SalesManagement
{
    public sealed class SalesOrderStatusDto
    {
        public SalesOrderStatusDto()
        {
            SalesOrderMaster = new List<SalesOrderMasterDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public IList<SalesOrderMasterDto> SalesOrderMaster { get; set; }
    }
}
