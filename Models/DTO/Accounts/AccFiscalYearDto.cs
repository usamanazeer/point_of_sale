using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.DTO.Accounts
{
    public class AccFiscalYearDto:ExtendedBaseModel
    {
        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }
        public List<AccFiscalYearDto> FiscalYears { get; set; }
    }
}
