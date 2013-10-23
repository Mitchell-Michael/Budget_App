namespace BudgetApp
{
    public class NetIncome : IValidation
    {
        public decimal Amount { get; set; }

        public bool Validate()
        {
            return Amount > 0;
        }
    }
}