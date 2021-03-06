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
    [System.Serializable]
    public class BudgetItem : MonthlyBill
    {

        public BudgetItem()
        {
            List = new ChildList();
        }

        public decimal Allocated { get; set; }

        public decimal Remaining
        {
            get { return Allocated - Amount; }
        }

        public List<Expenditure> Expenditures
        {
            get 
            {
                return List.Expenditure;
            }
            set
            {
                if (List.Expenditure == null)
                {
                    List.Expenditure = new List<Expenditure>();
                }
                List.Expenditure = value;
            }
        }

        private ChildList List { get; set; }

        [System.Serializable]
        private class ChildList
        {
            public ChildList() { }
            public List<Expenditure> Expenditure = new List<Expenditure>();
        }

        public void AddExpenditure(Expenditure expenditure)
        {
            var temp = Expenditures;
            temp.Add(expenditure);
            Expenditures = temp;
            Amount += expenditure.Amount;
        }

        public void RemoveExpenditure(Expenditure expenditure)
        {
            var temp = Expenditures;
            temp.Remove(expenditure);
            Expenditures = temp;
            Amount -= expenditure.Amount;
        }

        public Android.Graphics.Color GetColor()
        {
            Android.Graphics.Color color;
            if (Remaining >= 0)
            {
                decimal green = (int)(255m * Remaining / Allocated);
                if (green > 255) green = 255;
                else if (green < 0) green = 0;
                color = new Android.Graphics.Color(0, (int)green, 0);
            }
            else
            {
                decimal red = Remaining == 0 ? 0m : 255m * Allocated / Math.Abs(Remaining);
                if (red < 0) red = 0;
                else if (red > 255) red = 255;
                color = new Android.Graphics.Color((int)red, 0, 0);
            }
            return color;
        }
        
        public void Reset()
        {
            Amount = 0;
            Expenditures.Clear();
        }
    }
}