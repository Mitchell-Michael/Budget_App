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
    [Activity(Label = "Budget App", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden, Icon = "@drawable/icon")]
    public class Overview : Activity, BudgetViewModel.IEventListener
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
                StartActivity(typeof(Setup));
            };

            _list = FindViewById<ListView>(Resource.Id.BudgetList);
            _list.Adapter = new BudgetItemListAdapter(this, 0, _budgetViewModel.BudgetItems );
        }

        protected override void OnResume()
        {
            base.OnResume();
            _budgetViewModel.PropertyChanged += OnPropertyChanged;

            _bills.Text = _budgetViewModel.MonthlyBills.Sum(t => t.Amount).ToString("C");
            _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");
        }

        protected override void OnPause()
        {
            base.OnPause();
            _budgetViewModel.PropertyChanged -= OnPropertyChanged;
        }

        public void OnPropertyChanged(object sender, EventArgs e)
        {
            var property = (BudgetViewModel.Property)sender;
            switch (property)
            {
                case BudgetViewModel.Property.NetIncome:
                    {
                        _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");
                    }
                    break;
                case BudgetViewModel.Property.MonthlyBill:
                    {
                        _bills.Text = _budgetViewModel.MonthlyBills.Sum(t => t.Amount).ToString("C");
                        _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");
                    }
                    break;
                case BudgetViewModel.Property.BudgetItem:
                    {
                        ((BudgetItemListAdapter)_list.Adapter).NotifyDataSetInvalidated();
                        _bills.Text = _budgetViewModel.MonthlyBills.Sum(t => t.Amount).ToString("C");
                        _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");
                    }
                    break;
            }
        }
    }
}
