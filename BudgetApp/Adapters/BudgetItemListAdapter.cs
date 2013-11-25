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
        BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        Context _context;
        List<BudgetItem> _list;
        int _id;

        public BudgetItemListAdapter(Context context)
        {
            _context = context;
            _id = Resource.Layout.BudgetItem;
            _list = _budgetViewModel.BudgetItems;
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
            //convertView.FindViewById<TextView>(Resource.Id.BudgetAllocated).Text = item.Allocated.ToString("C");
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
            //TODO: Maybe add header here
            get
            {
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