namespace Models.DTO.Accounts
{
    public class AccTrialBalanceDto
    {
        public int AccountId { get; set; }
        public string AccNo { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int AccountTypeId { get; set; }
        public double Balance { get; set; }
    }
}
