using System;

namespace BudgetApp
{
    public interface IValidation
    {
        public bool IsValid { get { return Validate(); } }
        private abstract bool Validate();
    }
}