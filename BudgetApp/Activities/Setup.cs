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
    public class Setup : Activity, BudgetViewModel.IEventListener
    {
        private ListView _setupList;
        private EditText _netIncome;
        private TextView _remaining;
        private Button _add;
        private string _lastText = string.Empty;

        private SetupListExpenditureAdapter _adapter;

        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Setup);

            var addLayout = FindViewById<LinearLayout>(Resource.Id.Setup_AddLayout);
            var addName = FindViewById<EditText>(Resource.Id.Setup_AddName);
            var addAmount = FindViewById<EditText>(Resource.Id.Setup_AddAmount);

            _setupList = FindViewById<ListView>(Resource.Id.Setup_List);
            _setupList.Adapter = 
                _adapter = new SetupListExpenditureAdapter(this, Resource.Layout.SetupItem, _budgetViewModel.MonthlyBills);

            _add = FindViewById<Button>(Resource.Id.Setup_NewItem);
            _add.Click += delegate
            {
                MonthlyBill bill;
                if ((bill = new MonthlyBill() { Name = addName.Text, Amount = decimal.Parse(addAmount.Text) }).Validate())
                {
                    (_setupList.Adapter as SetupListExpenditureAdapter).AddItem(bill);
                    var bills = _budgetViewModel.MonthlyBills;
                    bills.Add(bill);
                    _budgetViewModel.MonthlyBills = bills;

                    addName.Text = string.Empty;
                    addAmount.Text = string.Empty;
                }
            };

            _remaining = FindViewById<TextView>(Resource.Id.Setup_Remaining);

            _netIncome = FindViewById<EditText>(Resource.Id.Setup_NetIncomeAmount);
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
                        }
                    }
                };
        }

        protected override void OnResume()
        {
            base.OnResume();
            _budgetViewModel.PropertyChanged += OnPropertyChanged;
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
                        _netIncome.Text = _budgetViewModel.NetIncome.GetValueOrDefault(0m).ToString("C");
                    }
                    break;
                case BudgetViewModel.Property.MonthlyBill:
                    {
                        (_setupList.Adapter as SetupListExpenditureAdapter).NotifyDataSetChanged();
                        decimal c = _budgetViewModel.NetIncome.GetValueOrDefault(0);
                        foreach (var bill in _budgetViewModel.MonthlyBills)
                        {
                            c -= bill.Amount;
                        }
                        _remaining.Text = c.ToString("C");
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
        }
    }
}
