using System;

namespace BudgetApp
{
    public class Expenditure : IValidation
    {
        public string Description { get; set; }
        public int DateRecorded { get; set; }
        public decimal Amount { get; set; }

        public bool Validate()
        {
            return string.IsNullOrEmpty(Description) && Amount > 0;
        }
    }
}