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

        public BudgetItem()
        {
            expenditures = new List<Expenditure>();
        }

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

        public Android.Graphics.Color Color
        {
            get { return Color(); }
        }

        private Android.Graphics.Color Color()
        {
            Android.Graphics.Color color;
            if (Remaining >= 0)
            {
                color = new Android.Graphics.Color(0, (int)(255m * Remaining / Allocated), 0);
            }
            else
            {
                decimal red = Remaining == 0 ? 0m : 255m * Allocated / Remaining;
                color = new Android.Graphics.Color((int)red, 0, 0);
            }
            return color;
        }
        

        public void Reset()
        {
            Amount = Allocated;
            expenditures.Clear();
        }
    }
}