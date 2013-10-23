namespace BudgetApp
{
    public class MonthlyBill : NetIncome
    {
        public string Name { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(Name) && base.Validate();
        }
    }
}