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

        IList<MonthlyBill> _list;
        int _id;

        public SetupListExpenditureAdapter(Context context, int resourceId, IList<MonthlyBill> list)
            : base(context, resourceId, list)
        {
            _list = list;
            _id = resourceId;
        }

        public override View GetView(int position, View view, ViewGroup parent)
        {
            if (view == null)
            {
                view = LayoutInflater.FromContext(Context).Inflate(_id, null, false);
            }

            view.FindViewById<EditText>(Resource.Id.MontlyExpenseName).Text = _list[position].Name;
            view.FindViewById<EditText>(Resource.Id.MontlyExpenseAmount).Text = _list[position].Amount.ToString("C");

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