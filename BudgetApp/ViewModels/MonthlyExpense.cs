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
    public class MonthlyExpence : BudgetItem
    {
        public MonthlyExpence(decimal netIncome)
            : base("", netIncome)
        { }
    }
}