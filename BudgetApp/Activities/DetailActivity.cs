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
    [Activity(Label = "Transactions", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden, Icon = "@drawable/ic_launcher")]
    public class DetailActivity : Activity
    {
        private readonly BudgetViewModel _budgetViewModel = ServiceContainer.Resolve<BudgetViewModel>();

        LinearLayout _headers;
        TextView _remaining, _detailName, _allocated;
        EditText _addName, _addAmount, _editDetailName, _editAllocated;
        ListView _list;
        Button _add, _delete;
        BudgetItem _item;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BudgetDetail);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            _item = _budgetViewModel.BudgetItems.ElementAt(Intent.GetIntExtra("position", 0));

            _headers = FindViewById<LinearLayout>(Resource.Id.Detail_Headers);

            _detailName = FindViewById<TextView>(Resource.Id.Detail_Name);
            _editDetailName = FindViewById<EditText>(Resource.Id.Detail_EditName);
            _editDetailName.EditorAction += delegate
            {
                _item.Name = _editDetailName.Text;
                OnItemChanged();
                _detailName.Text = _editDetailName.Text;
                _detailName.Visibility = ViewStates.Gone;
                _editDetailName.Visibility = ViewStates.Visible;
            };
            _detailName.LongClick += delegate
            {
                _editDetailName.Text = _detailName.Text;
                _detailName.Visibility = ViewStates.Gone;
                _editDetailName.Visibility = ViewStates.Visible;
                _editDetailName.RequestFocus();
            };
            _editDetailName.EditorAction += delegate
            {
                if (!string.IsNullOrEmpty(_detailName.Text))
                {
                    _item.Name = _detailName.Text;
                    OnItemChanged();
                    _detailName.Text = _editDetailName.Text;
                    _detailName.Visibility = ViewStates.Visible;
                    _editDetailName.Visibility = ViewStates.Gone;
                }
            };
            _editDetailName.FocusChange += (sender, e) =>
            {
                if (!e.HasFocus)
                {
                    if (!string.IsNullOrEmpty(_detailName.Text))
                    {
                        _item.Name = _detailName.Text;
                        OnItemChanged();
                        _detailName.Text = _editDetailName.Text;
                        _detailName.Visibility = ViewStates.Visible;
                        _editDetailName.Visibility = ViewStates.Gone;
                    }
                }
            };

            _allocated = FindViewById<TextView>(Resource.Id.Detail_Allocated);
            _editAllocated = FindViewById<EditText>(Resource.Id.Detail_EditAllocated);
            _editAllocated.EditorAction += delegate
            {
                decimal d;
                if (decimal.TryParse(_editAllocated.Text, out d))
                {
                    _item.Allocated = d;
                    OnItemChanged();
                    _allocated.Text = d.ToString("C");
                    _editAllocated.Text = d.ToString("C");
                    _allocated.Visibility = ViewStates.Visible;
                    _editAllocated.Visibility = ViewStates.Gone;
                }
            };
            _allocated.LongClick += delegate
            {
                _editAllocated.Text = _allocated.Text;
                _allocated.Visibility = ViewStates.Gone;
                _editAllocated.Visibility = ViewStates.Visible;
                _editAllocated.RequestFocus();
            };
            _editAllocated.FocusChange += (sender, e) =>
                {
                    if (!e.HasFocus)
                    {
                        decimal d;
                        if (decimal.TryParse(_editAllocated.Text, out d))
                        {
                            _item.Allocated = d;
                            OnItemChanged();
                            _allocated.Text = d.ToString("C");
                            _editAllocated.Text = d.ToString("C");
                            _allocated.Visibility = ViewStates.Visible;
                            _editAllocated.Visibility = ViewStates.Gone;
                        }
                    }
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
                        DateRecorded = DateTime.Now.Day,
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
            _budgetViewModel.BudgetItems = _budgetViewModel.BudgetItems;
            (_list.Adapter as BaseAdapter).NotifyDataSetChanged();
            _list.SmoothScrollToPosition(_list.Adapter.Count);
            _remaining.Text = _item.Remaining.ToString("C");
            _remaining.SetTextColor(_item.GetColor());
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}