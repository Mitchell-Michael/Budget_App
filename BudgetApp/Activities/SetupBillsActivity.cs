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
    [Activity(Label = "Setup Budget", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden | SoftInput.AdjustPan, Icon = "@drawable/ic_launcher")]
    public class SetupBillsActivity : Activity
    {
        private ListView _setupList;
        private EditText _netIncome, _addAmount, _addName;
        private TextView _remaining;
        private Button _add, _delete, _continue;
        private LinearLayout _rootLayout;
        private string _lastText = string.Empty;

        private SetupListExpenditureAdapter _adapter;

        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Setup);

            _continue = FindViewById<Button>(Resource.Id.Setup_Done);
            _continue.Click += delegate
            {
                StartActivity(typeof(SetupBudgetActivity));
            };

            if (!(_budgetViewModel.BudgetItems == null || _budgetViewModel.BudgetItems.Count == 0))
            {
                Title = "Edit Bills";
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                _continue.Visibility = ViewStates.Gone;

            }

            _rootLayout = FindViewById<LinearLayout>(Resource.Id.Setup_RootLayout);

            var addLayout = FindViewById<LinearLayout>(Resource.Id.Setup_AddLayout);
            _addName = FindViewById<EditText>(Resource.Id.Setup_AddName);
            _addAmount = FindViewById<EditText>(Resource.Id.Setup_AddAmount);
            _addName.EditorAction += (sender, e) =>
                {
                    _addAmount.RequestFocus();
                };

            _setupList = FindViewById<ListView>(Resource.Id.Setup_List);
            var header = FindViewById<TextView>(Resource.Id.Setup_ListHeader);
            
            _adapter = new SetupListExpenditureAdapter(this, ref header);
            _setupList.Adapter = _adapter;

            _setupList.ItemLongClick += (sender, e) =>
                {
                    var bill = _budgetViewModel.MonthlyBills[e.Position];
                    var bills = _budgetViewModel.MonthlyBills;
                    bills.RemoveAt(e.Position);
                    OnMonthlyBillsChanged();
                    _budgetViewModel.MonthlyBills = bills;
                    _addName.Text = bill.Name;
                    _addAmount.Text = bill.Amount.ToString();
                    _addName.RequestFocus();
                };

            _add = FindViewById<Button>(Resource.Id.Setup_NewItem);
            _add.Click += delegate
            {
              MonthlyBill bill;
                decimal d;
                if (decimal.TryParse(_addAmount.Text, out d))
                {
                    if ((bill = new MonthlyBill() { Name = _addName.Text, Amount = d }).Validate())
                    {
                        var bills = _budgetViewModel.MonthlyBills;
                        bills.Add(bill);
                        _budgetViewModel.MonthlyBills = bills;

                        OnMonthlyBillsChanged();

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

            _delete = FindViewById<Button>(Resource.Id.Setup_DeleteItem);
            _delete.Click += delegate
            {
                _addName.Text = string.Empty;
                _addAmount.Text = string.Empty;
            };

            _remaining = FindViewById<TextView>(Resource.Id.Setup_Remaining);

            _netIncome = FindViewById<EditText>(Resource.Id.Setup_NetIncomeAmount);
            _netIncome.EditorAction += (sender, e) =>
                {
                    _addName.RequestFocus();
                };
            _netIncome.FocusChange += (sender, e) =>
                {
                    if (e.HasFocus)
                    {
                        _netIncome.Text = string.Empty;
                    }
                    else
                    {
                        decimal d;
                        if (decimal.TryParse(_netIncome.Text, out d))
                        {
                            _budgetViewModel.NetIncome = d;
                            _netIncome.Text = d.ToString("C");
                            OnNetIncomeChanged();
                        }
                        else
                        {
                            _netIncome.Text = _budgetViewModel.NetIncome.HasValue ? _budgetViewModel.NetIncome.Value.ToString("C") : ((decimal)0m).ToString("C");
                        }
                    }
                };
        }

        protected override void OnResume()
        {
            base.OnResume();
            OnNetIncomeChanged();
            OnMonthlyBillsChanged();
        }

        private void OnNetIncomeChanged()
        {
            _netIncome.Text = _budgetViewModel.NetIncome.GetValueOrDefault(0m).ToString("C");
            if (_budgetViewModel.NetIncome.HasValue)
            {
                _add.Enabled = _delete.Enabled = _addAmount.Enabled = _continue.Enabled = _addName.Enabled = true;
                _remaining.Visibility = ViewStates.Visible;
            }
            else
            {
                _add.Enabled = _delete.Enabled = _addAmount.Enabled = _continue.Enabled = _addName.Enabled = false;
                _remaining.Text = _budgetViewModel.RemainingTotal.ToString("C");
                _remaining.Visibility = ViewStates.Invisible;
            }
            RemainingInvalidated();
        }

        private void OnMonthlyBillsChanged()
        {
            (_setupList.Adapter as BaseAdapter).NotifyDataSetChanged();
            RemainingInvalidated();
            _setupList.SmoothScrollToPosition(_setupList.Adapter.Count);
        }

        private void RemainingInvalidated()
        {
            decimal c = _budgetViewModel.NetIncome.GetValueOrDefault(0);

            c -= _budgetViewModel.MonthlyBills.Sum(t => t.Amount);
            _remaining.Text = c.ToString("C");
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
