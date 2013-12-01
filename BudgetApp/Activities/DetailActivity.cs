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
    [Activity(Label = "Budget App", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden, Icon = "@drawable/icon")]
    public class DetailActivity : Activity
    {
        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        LinearLayout _headers;
        TextView _remaining;
        EditText _addName, _addAmount, _detailName, _allocated;
        ListView _list;
        Button _add, _delete;
        BudgetItem _item;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BudgetDetail);

            _item = _budgetViewModel.BudgetItems.ElementAt(Intent.GetIntExtra("position", 0));

            _headers = FindViewById<LinearLayout>(Resource.Id.Detail_Headers);

            _detailName = FindViewById<EditText>(Resource.Id.Detail_Name);
            _detailName.LongClick += delegate
            {
                _detailName.Enabled = true;
            };
            _detailName.EditorAction += delegate
            {
                if (!string.IsNullOrEmpty(_detailName.Text))
                {
                    _item.Name = _detailName.Text;
                }
                _detailName.Enabled = false;
            };

            _allocated = FindViewById<EditText>(Resource.Id.Detail_Allocated);
            _allocated.LongClick += delegate
            {
                _allocated.Enabled = true;
            };
            _allocated.EditorAction += delegate
            {
                decimal d;
                if (decimal.TryParse(_allocated.Text, out d))
                {
                    _item.Allocated = d;
                }
                _allocated.Enabled = false;
            };

            _remaining = FindViewById<TextView>(Resource.Id.Detail_Remaining);

            _addName = FindViewById<EditText>(Resource.Id.Detail_AddName);
            _addAmount = FindViewById<EditText>(Resource.Id.Detail_AddAmount);

            _list = FindViewById<ListView>(Resource.Id.Detail_List);
            _list.ItemLongClick += (sender, e) =>
                {
                    var expenditure = _item.Expenditures.ElementAt(e.Position);
                    _item.RemoveExpenditure(expenditure);
                    OnItemChanged();

                    _addName.Text = expenditure.Description;
                    _addAmount.Text = expenditure.Amount.ToString("0.00");
                };

            _add = FindViewById<Button>(Resource.Id.Detail_NewItem);
            _add.Click += delegate
            {
                decimal d;
                if(decimal.TryParse(_addAmount.Text, out d))
                {
                    var expenditure = new Expenditure()
                    {
                        DateRecorded = DateTime.Now,
                        Description = _addName.Text,
                        Amount = d,
                    };
                    if (!string.IsNullOrEmpty(expenditure.Description) && expenditure.Amount > 0)
                    {
                        _item.AddExpenditure(expenditure);
                        OnItemChanged();
                        _addName.Text = string.Empty;
                        _addAmount.Text = string.Empty;
                    }
                }
            };

            _delete = FindViewById<Button>(Resource.Id.Detail_DeleteItem);
            _delete.Click += delegate
            {
                _addName.Text = string.Empty;
                _addAmount.Text = string.Empty;
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            _remaining.Text = _item.Remaining.ToString("C");
            _remaining.SetTextColor(_item.GetColor());

            _allocated.Text = _item.Allocated.ToString("C");
            _detailName.Text = _item.Name;

            _list.Adapter = new ExpenditureAdapter(this, Intent.GetIntExtra("position", 0), ref _headers);
        }

        private void OnItemChanged()
        {
            _budgetViewModel.BudgetItems[Intent.GetIntExtra("position", 0)] = _item;
            (_list.Adapter as BaseAdapter).NotifyDataSetChanged();
            _remaining.Text = _item.Remaining.ToString("C");
            _remaining.SetTextColor(_item.GetColor());
        }
    }
}