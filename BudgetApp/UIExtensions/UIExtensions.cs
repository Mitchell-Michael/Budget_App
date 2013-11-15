using Android.Content;
using Android.Views.InputMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetApp
{
    public static class UIExtensions
    {
        public static void ShowKeyboard(Context context)
        {
            var imm = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            imm.ToggleSoftInput(ShowFlags.Implicit, HideSoftInputFlags.ImplicitOnly);
        }
        public static void HideKeyboard(Context context, Android.OS.IBinder windowToken)
        {
            var imm = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            imm.ShowSoftInputFromInputMethod(windowToken, ShowFlags.Implicit);
        }
    }
}
