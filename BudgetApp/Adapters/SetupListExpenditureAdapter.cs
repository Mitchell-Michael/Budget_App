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
    public class SetupListExpenditureAdapter : BaseAdapter<MonthlyBill>
    {
        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();
        Context _context;
        List<MonthlyBill> _list;
        TextView _header;
        int _id;

        public SetupListExpenditureAdapter(Context context, ref TextView header)
        {
            _header = header;
            _context = context;
            _list = _budgetViewModel.MonthlyBills;
        }

        public override View GetView(int position, View view, ViewGroup parent)
        {
            TextView name, amount;
            if (view == null)
            {
                view = LayoutInflater.FromContext(_context).Inflate(Resource.Layout.SetupItem, null, false);
            }

            name = view.FindViewById<TextView>(Resource.Id.MontlyExpenseName);
            amount = view.FindViewById<TextView>(Resource.Id.MontlyExpenseAmount);

            name.Text = _list[position].Name;
            name.Tag = position;
            amount.Text = _list[position].Amount.ToString("C");
            amount.Tag = position;

            return view;
        }

        public void RemoveItem(int index)
        {
            _list.RemoveAt(index);
            NotifyDataSetChanged();
        }

        public override void NotifyDataSetChanged()
        {
            _list = _budgetViewModel.MonthlyBills;
            base.NotifyDataSetChanged();
        }

        public override int Count
        {
            get
            {
                if (_list.Count == 0)
                {
                    _header.Visibility = ViewStates.Gone;
                }
                else
                {
                    _header.Visibility = ViewStates.Visible;
                }
                return _list.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override MonthlyBill this[int position]
        {
            get { return _list[position]; }
        }
    }
}