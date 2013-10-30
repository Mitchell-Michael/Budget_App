namespace BudgetApp
{
    public class NetIncome : IValidation
    {
        public decimal Amount { get; set; }

        public virtual bool Validate()
        {
            return Amount > 0;
        }
    }
}