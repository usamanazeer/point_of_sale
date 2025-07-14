using System;
using System.Collections.Generic;

namespace Models.DTO.Reporting.Sales
{
    public class RptTaxCollectionDto
    {
        
        public int CompanyId { get; set; }
        public int? BranchId { get; set; }
        public Response Response { get; set; }
        public IList<RptTaxCollectionRowDto> TaxCollectionData { get; set; }
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
        public RptTaxCollectionDto()
        {
            TaxCollectionData = new List<RptTaxCollectionRowDto>();
        }
    }

    public class RptTaxCollectionRowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TaxAmount { get; set; }
    }
}
