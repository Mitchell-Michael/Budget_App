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
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();
            ServiceContainer.Register<BudgetViewModel>();
            if (ServiceContainer.Resolve<BudgetViewModel>().BudgetItems.Count == 0)
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