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
    public class SetupListExpenditureAdapter : ArrayAdapter<MonthlyBill>
    {
        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();
        List<MonthlyBill> _list;
        int _id;

        public SetupListExpenditureAdapter(Context context, int resourceId, List<MonthlyBill> list)
            : base(context, resourceId, list)
        {
            _list = list;
            _id = resourceId;
        }

        public override View GetView(int position, View view, ViewGroup parent)
        {
            TextView name = null, amount = null;
            if (view == null)
            {
                view = LayoutInflater.FromContext(Context).Inflate(_id, null, false);
            }

            name = view.FindViewById<TextView>(Resource.Id.MontlyExpenseName);
            amount = view.FindViewById<TextView>(Resource.Id.MontlyExpenseAmount);

            name.Text = _list[position].Name;
            name.Tag = position;
            amount.Text = _list[position].Amount.ToString("C");
            amount.Tag = position;

            return view;
        }

        public void AddItem(MonthlyBill item)
        {
            _list.Add(item);
            NotifyDataSetChanged();
        }

        public override int Count
        {
            get
            {
                return _list.Count;
            }
        }
    }
}