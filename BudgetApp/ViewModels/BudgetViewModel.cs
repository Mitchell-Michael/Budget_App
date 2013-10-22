using Android.Preferences;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BudgetApp
{
    public class BudgetViewModel
    {
        private object _lock;

        public BudgetViewModel()
        {
            _lock = new object();
        }

        private List<MonthlyBill> _monthlyBills;
        public List<MonthlyBill> MonthlyBills
        {
            get
            {
                if (_monthlyBills == null)
                {
                    PullBills();
                }
                return _monthlyBills;
            }
            set
            {
                _monthlyBills = value;
                PushBills();
            }
        }

        private List<BudgetItem> _budgetItems;
        public List<BudgetItem> BudgetItems
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

        private decimal? _netIncome;
        public decimal? NetIncome
        {
            get
            {
                if (_netIncome == null)
                {
                    PullIncome();
                }
                return _netIncome;
            }
            set
            {
                _netIncome = value;
                PushIncome();
            }
        }

        private void PushIncome()
        {
            lock (_lock)
            {
                using (var perfs = PreferenceManager.GetDefaultSharedPreferences(MainApplication.Context))
                {
                    using (var edit = perfs.Edit())
                    {
                        edit.PutString("Income", JsonConvert.SerializeObject(_netIncome));
                        edit.Commit();
                    }
                }
            }
        }

        private void PullIncome()
        {
            lock (_lock)
            {
                using (var perfs = PreferenceManager.GetDefaultSharedPreferences(MainApplication.Context))
                {
                    _netIncome = JsonConvert.DeserializeObject<decimal?>(perfs.GetString("Income", ""));
                }
            }
        }

        private void PushBills()
        {
            lock (_lock)
            {
                using (var perfs = PreferenceManager.GetDefaultSharedPreferences(MainApplication.Context))
                {
                    using (var edit = perfs.Edit())
                    {
                        edit.PutString("Bills", JsonConvert.SerializeObject(_monthlyBills));
                        edit.Commit();
                    }
                }
            }
        }

        private void PullBills()
        {
            lock (_lock)
            {
                using (var perfs = PreferenceManager.GetDefaultSharedPreferences(MainApplication.Context))
                {
                    _monthlyBills = JsonConvert.DeserializeObject<List<MonthlyBill>>(perfs.GetString("Bills", ""));
                }
            }
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
                    _budgetItems = JsonConvert.DeserializeObject<List<BudgetItem>>(perfs.GetString("Budget", ""));
                }
            }
        }
    }
}
