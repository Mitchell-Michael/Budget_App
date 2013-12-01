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
using Android.Graphics;

namespace BudgetApp
{
    public class BudgetItemListAdapter : BaseAdapter<BudgetItem>
    {
        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        LinearLayout _headers;
        Context _context;
        List<BudgetItem> _list;
        int _id;

        public BudgetItemListAdapter(Context context, ref LinearLayout headers)
        {
            _context = context;
            _id = Resource.Layout.BudgetItem;
            _list = _budgetViewModel.BudgetItems;
            _headers = headers;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(_context).Inflate(_id, null, false);
            }

            var item = _list[position];
            var category = convertView.FindViewById<TextView>(Resource.Id.BudgetCategory);
            category.Text = item.Name;
            var remaining = convertView.FindViewById<TextView>(Resource.Id.BudgetRemaining);
            remaining.Text = item.Remaining.ToString("C");
            remaining.SetTextColor(item.GetColor());

            return convertView;
        }

        public override void NotifyDataSetChanged()
        {
            _list = _budgetViewModel.BudgetItems;
        }

        public override int Count
        {
            get
            {
                if (_list.Count > 0)
                {
                    _headers.Visibility = ViewStates.Visible;
                }
                else
                {
                    _headers.Visibility = ViewStates.Gone;
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