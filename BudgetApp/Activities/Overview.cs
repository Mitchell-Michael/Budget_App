using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace BudgetApp
{
    [Activity(Label = "Budget App", MainLauncher = true, Icon = "@drawable/icon")]
    public class Overview : Activity
    {
        ListView _list;
        private BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _list = FindViewById<ListView>(Resource.Id.BudgetList);
            _list.Adapter = new SetupListExpenditureAdapter(this, 0, new List<BudgetItem>(_budgetViewModel.BudgetItems.Values) );
        }
    }
}
