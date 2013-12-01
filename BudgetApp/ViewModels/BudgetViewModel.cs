using Android.Preferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class BudgetViewModel
    {

        private object _lock;

        public BudgetViewModel()
        {
            _lock = new object();
        }

        public decimal RemainingTotal
        {
            get
            {
                return _netIncome.GetValueOrDefault(0m) - MonthlyBills.Sum(t => t.Amount) - BudgetItems.Sum(t=>t.Remaining);
            }
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
                return _monthlyBills == null ? new List<MonthlyBill>() : _monthlyBills;
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
                return _budgetItems == null ? new List<BudgetItem>() : _budgetItems;
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

        private int LastOpened
        {
            get
            {
                try
                {
                lock (_lock)
                {
                    using (var perfs = PreferenceManager.GetDefaultSharedPreferences(MainApplication.Context))
                    {
                        return JsonConvert.DeserializeObject<int>(perfs.GetString("LastOpened", ""));
                    }
                }
                }
                catch(Exception e)
                {
                    return 0;
                }
            }
            set
            {
                lock (_lock)
                {
                    using (var perfs = PreferenceManager.GetDefaultSharedPreferences(MainApplication.Context))
                    {
                        using (var edit = perfs.Edit())
                        {
                            edit.PutString("LastOpened", JsonConvert.SerializeObject(value));
                            edit.Commit();
                        }
                    }
                }
            }
        }

        public bool IsNewMonth()
        {
            if (LastOpened == null)
            {
                LastOpened = 0;
                return false;
            }
            else if (LastOpened < DateTime.Now.Month)
            {
                LastOpened = DateTime.Now.Month;
                for (int i = 0; i < BudgetItems.Count; i++)
                {
                    _budgetItems[i].Reset();
                }
                return true;
            }
            else
            {
                LastOpened = DateTime.Now.Month;
                return false;
            }
        }
    }
}
