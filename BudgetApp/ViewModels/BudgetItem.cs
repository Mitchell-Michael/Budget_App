using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BudgetApp
{
    public class BudgetItem
    {
        public string Name;
        public decimal Allocated;
        public decimal Current;

        public decimal Remaining
        {
            get { return Allocated - Current; }
        }

        List<Expenditure> expenditures;

        public BudgetItem(string name, decimal allocated)
        {
            this.Allocated = allocated;
            Current = allocated;
            this.Name = name;
        }

        public void AddExpenditure(Expenditure expenditure)
        {
            expenditures.Add(expenditure);
            Current -= expenditure.Amount;
        }

        public void RemoveExpenditure(Expenditure expenditure)
        {
            expenditures.Remove(expenditure);
            Current += expenditure.Amount;
        }
    }
}