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
    public class ExpenditureAdapter : BaseAdapter<Expenditure>
    {
        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        LinearLayout _headers;
        Context _context;
        List<Expenditure> _list;
        int _billNumber;
        int _id;

        public ExpenditureAdapter(Context context, int billNumber, ref LinearLayout headers)
        {
            _headers = headers;
            _context = context;
            _list = _budgetViewModel.BudgetItems.ElementAt(billNumber).Expenditures;
            _billNumber = billNumber;
        }

        public override View GetView(int position, View view, ViewGroup parent)
        {
            TextView date, name, amount;
            if (view == null)
            {
                view = LayoutInflater.FromContext(_context).Inflate(Resource.Layout.Expenditure, null, false);
            }

            var item = _list[position];

            date = view.FindViewById<TextView>(Resource.Id.Expenditure_Date);
            name = view.FindViewById<TextView>(Resource.Id.Expenditure_Name);
            amount = view.FindViewById<TextView>(Resource.Id.Expenditure_Amount);

            date.Text = item.DateRecorded.ToString("dd");
            name.Text = item.Description;
            amount.Text = item.Amount.ToString("C");

            return view;
        }

        public override int Count
        {
            get
            {
                if (_list.Count == 0)
                {
                    _headers.Visibility = ViewStates.Gone;
                }
                else
                {
                    _headers.Visibility = ViewStates.Visible;
                }
                return _list.Count;
            }
        }

        public override void NotifyDataSetChanged()
        {
            _list = _budgetViewModel.BudgetItems.ElementAt(_billNumber).Expenditures;
            base.NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Expenditure this[int position]
        {
            get { return _list[position]; }
        }
    }
}