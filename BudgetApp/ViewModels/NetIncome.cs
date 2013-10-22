namespace BudgetApp
{
    public class NetIncome : IValidation
    {
        public decimal Amount { get; set; }

        public override bool Validate()
        {
            return Amount > 0;
        }
    }
}