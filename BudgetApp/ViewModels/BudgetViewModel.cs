using Android.Content;
using Android.Preferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BudgetApp
{
    public class BudgetViewModel
    {
        private Context _context;
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

        public class MonthlyExpence : BudgetItem
        {
            public MonthlyExpence(decimal netIncome)
                : base("", netIncome)
            { }
        }

        public class BudgetItem
        {
            public string Name;
            public decimal Allocated;
            public decimal Current;

            public decimal Remaining
            {
                get { return Allocated - Current; }
            }

            List<Expenditure> expenditures;

            public BudgetItem(string name, decimal allocated)
            {
                this.Allocated = allocated;
                Current = allocated;
                this.Name = name;
            }

            public void AddExpenditure(Expenditure expenditure)
            {
                expenditures.Add(expenditure);
                Current -= expenditure.Amount;
            }

            public void RemoveExpenditure(Expenditure expenditure)
            {
                expenditures.Remove(expenditure);
                Current += expenditure.Amount;
            }
        }

        public class Expenditure
        {
            public string Description { get; set; }
            public DateTime DateRecorded { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
