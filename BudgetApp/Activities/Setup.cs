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
        private Button _add;

        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Setup);

            _setupList = FindViewById<ListView>(Resource.Id.Setup_List);
            _setupList.Adapter = new SetupListExpenditureAdapter(this, Resource.Layout.Setup, _budgetViewModel.Values);

            _add = FindViewById<Button>(Resource.Id.Setup_AddButton);
            _add.Click += delegate
            {
                ((SetupListExpenditureAdapter)_setupList.Adapter).LastItem();
            };

            _remaining = FindViewById<TextView>(Resource.Id.Setup_Remaining);

            _netIncome = FindViewById<EditText>(Resource.Id.Setup_NetIncomeAmount);
        }
    }
}