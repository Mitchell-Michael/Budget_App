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
    public class Expenditure
    {
        public string Description { get; set; }
        public DateTime DateRecorded { get; set; }
        public decimal Amount { get; set; }
    }
}