using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class TransactionModel
    {
        // Enum defined inside the Transaction model
        public enum TransactionType
        {
            Credit,
            Debit,
        }

        public int Id { get; set; } // Unique identifier for the transaction
        public int UserId { get; set; } // Link transaction to a user
        public string Title { get; set; }
        public decimal Amount { get; set; } // Amount of the transaction
        public TransactionType Type { get; set; } // Enum: Credit, Debit
        public List<string> Tags { get; set; } = new List<string>(); // List of custom tags (e.g., "Rent", "Monthly")
        public List<string> Source { get; set; } = new List<string>(); // Source of transaction (e.g., "Bank", "Cash")
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }
}
