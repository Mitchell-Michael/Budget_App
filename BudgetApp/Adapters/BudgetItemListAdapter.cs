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
    public class BudgetItemListAdapter : ArrayAdapter<BudgetItem>
    {
        List<BudgetItem> _list;
        int _id;

        public BudgetItemListAdapter(Context context, int resourceId, List<BudgetItem> list)
            : base(context, resourceId, list)
        {
            _list = list;
            _id = Resource.Layout.BudgetItem;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(Context).Inflate(_id, null, false);
            }

            var item = _list[position];
            convertView.FindViewById<TextView>(Resource.Id.Category).Text = item.Name;
            convertView.FindViewById<TextView>(Resource.Id.BudgetAllocated).Text = item.Allocated.ToString("C");
            convertView.FindViewById<TextView>(Resource.Id.BudgetRemaining).Text = item.Remaining.ToString("C");

            return convertView;
        }

        public void AddItem(BudgetItem item)
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