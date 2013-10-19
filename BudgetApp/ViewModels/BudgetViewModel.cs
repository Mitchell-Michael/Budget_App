using Android.Content;
using Android.Preferences;
using Newtonsoft.Json;
using System;
using System.Linq;
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

        public bool IsValid(BudgetItem item)
        {
            return item != null && item.Name != string.Empty && item.Allocated > 0;
        }

        public List<BudgetItem> Values
        {
            get
            {
                return BudgetItems.Select(t => t.Value) == null ? new List<BudgetItem>() : BudgetItems.Select(t => t.Value).ToList<BudgetItem>();
            }
        }

        public List<string> Keys
        {
            get
            {
                return BudgetItems.Select(t => t.Key) == null ? new List<string>() : BudgetItems.Select(t => t.Key).ToList<string>();
            }
        }

        public BudgetViewModel()
        {
            _lock = new object();
        }

        private void PushBudget()
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

        private void PullBudget()
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
