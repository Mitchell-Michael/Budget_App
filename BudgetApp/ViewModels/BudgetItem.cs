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
    public class BudgetItem : MonthlyBill
    {
        public decimal Allocated { get; set; }

        public decimal Remaining
        {
            get { return Allocated - Amount; }
        }

        List<Expenditure> expenditures;

        public void AddExpenditure(Expenditure expenditure)
        {
            if (expenditure.Validate())
            {
                expenditures.Add(expenditure);
                Amount -= expenditure.Amount;
            }
        }

        public void RemoveExpenditure(Expenditure expenditure)
        {
            if (expenditure.Validate())
            {
                expenditures.Remove(expenditure);
                Amount += expenditure.Amount;
            }
        }

        public void Reset()
        {
            Amount = Allocated;
        }

        public bool Validate()
        {
            return base.Validate() && Allocated > 0;
        }
    }
}