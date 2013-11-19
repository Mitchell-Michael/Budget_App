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
    public class BudgetItemSetupListAdapter : ArrayAdapter<BudgetItem>
    {
        List<BudgetItem> _list = ServiceContainer.Resolve<BudgetViewModel>().BudgetItems;
        int _id;
        TextView _header;

        public BudgetItemSetupListAdapter(Context context, int resourceId, List<BudgetItem> list, ref TextView header)
            : base(context, resourceId, list)
        {
            _list = list;
            _id = Resource.Layout.SetupItem;
            _header = header;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(Context).Inflate(_id, null, false);
            }

            var item = _list[position];
            convertView.FindViewById<TextView>(Resource.Id.MontlyExpenseName).Text = item.Name;
            convertView.FindViewById<TextView>(Resource.Id.MontlyExpenseAmount).Text = item.Allocated.ToString("C");

            return convertView;
        }

        public void AddItem(BudgetItem item)
        {
            _list.Add(item);
            NotifyDataSetChanged();
        }

        public void RemoveItem(int index)
        {
            _list.RemoveAt(index);
            NotifyDataSetChanged();
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
    }
}
