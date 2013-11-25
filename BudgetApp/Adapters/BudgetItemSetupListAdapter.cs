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
    public class BudgetItemSetupListAdapter : BaseAdapter<BudgetItem>
    {
        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();
        List<BudgetItem> _list;

        Context _context;
        TextView _header;

        public BudgetItemSetupListAdapter(Context context, ref TextView header)
        {
            _context = context;
            _header = header;
            _list = _budgetViewModel.BudgetItems;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(_context).Inflate(Resource.Layout.SetupItem, null, false);
            }

            var item = _list[position];
            convertView.FindViewById<TextView>(Resource.Id.MontlyExpenseName).Text = item.Name;
            convertView.FindViewById<TextView>(Resource.Id.MontlyExpenseAmount).Text = item.Allocated.ToString("C");

            return convertView;
        }

        public override void NotifyDataSetChanged()
        {
            _list = _budgetViewModel.BudgetItems;
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

        public override BudgetItem this[int position]
        {
            get { return _list[position]; }
        }
    }
}
