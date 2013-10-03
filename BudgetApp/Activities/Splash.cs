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

namespace BudgetApp.Activities
{
    [Activity(Label = "Splash", MainLauncher=true)]
    public class Splash : Activity
    {

        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //SetContentView(Resource.Layout.Splash);
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (_budgetViewModel.BudgetItems == null || _budgetViewModel.BudgetItems.Count == 0)
            {
                StartActivity(typeof(Setup));
            }
            else
            {
                StartActivity(typeof(Overview));
            }
        }
    }
}