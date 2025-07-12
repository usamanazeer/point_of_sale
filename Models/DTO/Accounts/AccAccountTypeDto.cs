using System.Collections.Generic;

namespace Models.DTO.Accounts
{
    public sealed class AccAccountTypeDto
    {
        public AccAccountTypeDto()
        {
            AccAccount = new List<AccAccountDto>();

            ChartOfAccounts = new List<AccAccountTypeDto>();
            AccountTypes = new List<AccAccountTypeDto>();
            Response = new Response();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public IList<AccAccountDto> AccAccount { get; set; }

        //dto props

        public IList<AccAccountTypeDto> ChartOfAccounts { get; set; }
        public IList<AccAccountTypeDto> AccountTypes { get; set; }
        public Response Response { get; set; }
    }
}