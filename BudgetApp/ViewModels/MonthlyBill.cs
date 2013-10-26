namespace BudgetApp
{
    public class MonthlyBill : NetIncome
    {
        public string Name { get; set; }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(Name) && base.Validate();
        }
    }
}