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
    [Activity(Label = "Setup Budget", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden | SoftInput.AdjustPan)]
    public class SetupBudgetActivity : Activity
    {
        private ListView _setupList;
        private EditText _addAmount, _addName;
        private TextView _savings, _remaining, _header;
        private Button _add, _delete;
        private LinearLayout _rootLayout;
        private string _lastText = string.Empty;

        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SetupBudget);

            _rootLayout = FindViewById<LinearLayout>(Resource.Id.Budget_RootLayout);

            var addLayout = FindViewById<LinearLayout>(Resource.Id.Budget_AddLayout);
            _addName = FindViewById<EditText>(Resource.Id.Budget_AddName);
            _addAmount = FindViewById<EditText>(Resource.Id.Budget_AddAmount);
            _addName.EditorAction += (sender, e) =>
            {
                _addAmount.RequestFocus();
            };

            _setupList = FindViewById<ListView>(Resource.Id.Budget_List);
            _header = FindViewById<TextView>(Resource.Id.Budget_ListHeader);

            _setupList.Adapter = new BudgetItemSetupListAdapter(this, ref _header);

            _setupList.ItemLongClick += (sender, e) =>
            {
                var bill = _budgetViewModel.BudgetItems[e.Position];
                var bills = _budgetViewModel.BudgetItems;
                bills.RemoveAt(e.Position);
                _budgetViewModel.BudgetItems = bills;

                OnBudgetChanged();

                _addName.Text = bill.Name;
                _addAmount.Text = bill.Allocated.ToString();
                _addName.RequestFocus();
            };

            _add = FindViewById<Button>(Resource.Id.Budget_NewItem);
            _add.Click += delegate
            {
                decimal d;
                if (decimal.TryParse(_addAmount.Text, out d))
                {
                    if (!string.IsNullOrEmpty(_addName.Text))
                    {
                        BudgetItem bill = new BudgetItem() { Name = _addName.Text, Allocated = d };
                        var bills = _budgetViewModel.BudgetItems;
                        bills.Add(bill);
                        _budgetViewModel.BudgetItems = bills;

                        OnBudgetChanged();

                        _addName.Text = string.Empty;
                        _addAmount.Text = string.Empty;
                        _addName.RequestFocus();
                    }
                }
            };
            _addAmount.EditorAction += (sender, e) =>
            {
                UIExtensions.HideKeyboard(this, _addAmount.WindowToken);
            };

            _delete = FindViewById<Button>(Resource.Id.Budget_DeleteItem);
            _delete.Click += delegate
            {
                _addName.Text = string.Empty;
                _addAmount.Text = string.Empty;
            };

            _savings = FindViewById<TextView>(Resource.Id.Budget_Remaining);

            _remaining = FindViewById<TextView>(Resource.Id.Budget_NetIncomeAmount);

            FindViewById<Button>(Resource.Id.Budget_Done).Click += delegate
            {
                StartActivity(typeof(OverviewActivity));
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            _remaining.Text = (_budgetViewModel.NetIncome.Value - _budgetViewModel.MonthlyBills.Sum(t => t.Amount)).ToString("C");
            OnBudgetChanged();
        }

        private void OnBudgetChanged()
        {
            (_setupList.Adapter as BaseAdapter).NotifyDataSetChanged();
            RemainingInvalidated();
            _setupList.SmoothScrollToPosition(_setupList.Adapter.Count);
        }

        private void RemainingInvalidated()
        {
            decimal c = _budgetViewModel.NetIncome.GetValueOrDefault(0);

            c -= _budgetViewModel.MonthlyBills.Sum(t => t.Amount);
            c -= _budgetViewModel.BudgetItems.Sum(t => t.Allocated);
            _savings.Text = c.ToString("C");
        }
    }
}
