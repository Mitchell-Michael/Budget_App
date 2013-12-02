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
using System.Threading.Tasks;

namespace BudgetApp.Activities
{
    [Activity(Label = "Micro Budget", Icon = "@drawable/ic_launcher", MainLauncher=true, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden | SoftInput.AdjustPan, Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class SplashActivity : Activity
    {

        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Splash);
        }

        protected async override void OnResume()
        {
            base.OnResume();
            await Task.Delay(500);
            if (_budgetViewModel.BudgetItems == null || _budgetViewModel.BudgetItems.Count == 0)
            {
                StartActivity(typeof(SetupBillsActivity));
            }
            else
            {
                StartActivity(typeof(OverviewActivity));
            }
        }
    }
}