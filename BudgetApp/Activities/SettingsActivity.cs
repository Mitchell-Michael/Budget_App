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
    [Activity(Label = "Budget App", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden, Icon = "@drawable/ic_launcher")]
    public class SettingsActivity : Activity
    {
        Button _bills, _budget;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Settings);

            ActionBar.Title = "Options";
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            _bills = FindViewById<Button>(Resource.Id.Settings_Bills);
            _bills.Click += delegate
            {
                StartActivity(typeof(SetupBillsActivity));
            };
            _budget = FindViewById<Button>(Resource.Id.Settings_Budget);
            _budget.Click += delegate
            {
                StartActivity(typeof(SetupBudgetActivity));
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}