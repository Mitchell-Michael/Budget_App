using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace BudgetApp
{
    [Activity(Label = "Budget App", LaunchMode=Android.Content.PM.LaunchMode.SingleInstance, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden, Icon = "@drawable/icon")]
    public class OverviewActivity : Activity
    {
        ListView _list;
        TextView _bills, _remaining;
        private BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Overview);

            _bills = FindViewById<TextView>(Resource.Id.BudgetBillTotal);

            _remaining = FindViewById<TextView>(Resource.Id.BudgetRemaining);

            FindViewById<LinearLayout>(Resource.Id.SetupLayout).Click += delegate
            {
                StartActivity(typeof(SetupBillsActivity));
            };

            _list = FindViewById<ListView>(Resource.Id.BudgetList);
            _list.Adapter = new BudgetItemListAdapter(this);
        }

        protected override void OnResume()
        {
            base.OnResume();

            _bills.Text = _budgetViewModel.MonthlyBills.Sum(t => t.Amount).ToString("C");
            _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");
        }

        public void OnNetIncomeChanged()
        {
            _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");
        }

        public void OnMonthlyBillChanged()
        {
            _bills.Text = _budgetViewModel.MonthlyBills.Sum(t => t.Amount).ToString("C");
            _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");
        }

        public void OnBudgetItemChanged()
        {
            ((BudgetItemListAdapter)_list.Adapter).NotifyDataSetInvalidated();
            _bills.Text = _budgetViewModel.MonthlyBills.Sum(t => t.Amount).ToString("C");
            _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");
        }
    }
}
