using System;

namespace BudgetApp
{
    public struct Expenditure : IValidation
    {
        public string Description { get; set; }
        public DateTime DateRecorded { get; set; }
        public decimal Amount { get; set; }

        public bool Validate()
        {
            return string.IsNullOrEmpty(Description) && Amount > 0;
        }
    }
}