using Android.Content;
using Android.Preferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BudgetApp
{
    public class BudgetViewModel
    {
        private object _lock;
        private Dictionary<string, BudgetItem> _budgetItems;

        public Dictionary<string, BudgetItem> BudgetItems
        {
            get
            {
                if (_budgetItems == null)
                {
                    PullBudget();
                }
                return _budgetItems;
            }
            set 
            {
                _budgetItems = value;
                PushBudget();
            }
        }

        public BudgetViewModel()
        {
            _lock = new object();
        }

        public void PushBudget()
        {
            lock (_lock)
            {
                using (var perfs = PreferenceManager.GetDefaultSharedPreferences(MainApplication.Context))
                {
                    using (var edit = perfs.Edit())
                    {
                        edit.PutString("Budget", JsonConvert.SerializeObject(_budgetItems));
                        edit.Commit();
                    }
                }
            }
        }

        public void PullBudget()
        {
            lock (_lock)
            {
                using (var perfs = PreferenceManager.GetDefaultSharedPreferences(MainApplication.Context))
                {
                    _budgetItems = JsonConvert.DeserializeObject<Dictionary<string, BudgetItem>>(perfs.GetString("Budget", ""));
                }
            }
        }
    }
}
