using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{

    public enum DebtType
    {
        debtIn,
        debtOut
    }
    public class DebtModel
    {
        public int Id { get; set; } // Unique debt identifier
        public int UserId { get; set; } // Link debt to a user
        public decimal Amount { get; set; } // Total debt amount
        public decimal PaidAmount { get; set; } // Amount paid towards debt
        public string Source { get; set; } // Source of the debt (e.g., loan, mortgage)
        public bool IsCleared => Type == DebtType.debtOut && RemainigAmount == 0;
        public string Notes { get; set; } // Optional notes
        public DateTime DueDate { get; set; } // Due date for the debt payment
        public DateTime Date { get; set; } // Date the debt was created

        public DebtType Type { get; set; }
        public decimal RemainigAmount => Type == DebtType.debtOut ? Amount - PaidAmount : 0;
    }
}
