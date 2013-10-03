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
    [Activity(Label = "Setup")]
    public class Setup : Activity
    {

        private ListView _setupList;
        private EditText _netIncome;
        private TextView _remaining;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Setup);

            _setupList = FindViewById<ListView>(Resource.Id.Setup_List);

            _remaining = FindViewById<TextView>(Resource.Id.Setup_Remaining);

            _netIncome = FindViewById<EditText>(Resource.Id.Setup_NetIncomeAmount);
        }
    }
}