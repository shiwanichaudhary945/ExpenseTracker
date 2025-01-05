using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class AppData
    {
        public List<Usermodel> Users { get; set; } = new();
        public List<DebtModel> Debts { get; set; } = new();
        public List<TransactionModel> Transactions { get; set; } = new();


    }
}
