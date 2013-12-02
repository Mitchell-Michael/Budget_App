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
    [Activity(Label = "Overview", LaunchMode=Android.Content.PM.LaunchMode.SingleInstance, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden, Icon = "@drawable/ic_launcher")]
    public class OverviewActivity : Activity
    {
        LinearLayout _headers;
        ListView _list;
        TextView _net, _bills, _remaining;
        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Overview);


            _headers = FindViewById<LinearLayout>(Resource.Id.Overview_Headers);

            _net = FindViewById<TextView>(Resource.Id.Overview_Income);
            _bills = FindViewById<TextView>(Resource.Id.BudgetBillTotal);

            _remaining = FindViewById<TextView>(Resource.Id.BudgetRemaining);

            FindViewById<LinearLayout>(Resource.Id.SetupLayout).Click += delegate
            {
                StartActivity(typeof(SetupBillsActivity));
            };

            _list = FindViewById<ListView>(Resource.Id.BudgetList);
            _list.Adapter = new BudgetItemListAdapter(this, ref _headers);

            _list.ItemClick += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(DetailActivity));
                intent.PutExtra("position", e.Position);
                StartActivity(intent);
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            _net.Text = _budgetViewModel.NetIncome.Value.ToString("C");
            _bills.Text = _budgetViewModel.MonthlyBills.Sum(t => t.Amount).ToString("C");
            _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");

            _list.Adapter = new BudgetItemListAdapter(this, ref _headers);
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.OverviewMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            if (item.ItemId == Resource.Id.Action_Settings)
            {
                StartActivity(typeof(SettingsActivity));
            }
            return base.OnMenuItemSelected(featureId, item);
        }
    }
}