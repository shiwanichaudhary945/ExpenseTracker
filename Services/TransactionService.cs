using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ExpenseTracker.Models;


    public class TransactionService
    {
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
        private static readonly string FilePath = Path.Combine(FolderPath, "data.json"); // Updated file to store complete AppData

        // Load all data (users, debts, transactions) from the JSON file
        public AppData LoadAppData()
        {
            if (!File.Exists(FilePath))
                return new AppData();  // Return an empty AppData if the file doesn't exist

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<AppData>(json) ?? new AppData(); // Deserialize into AppData
        }

        // Save all data (users, debts, transactions) to the JSON file
        public void SaveAppData(AppData appData)
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            var json = JsonSerializer.Serialize(appData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);  // Save AppData to the file
        }

        // Save transaction data
        public void SaveTransactions(List<TransactionModel> transactions)
        {
            var appData = LoadAppData(); // Load existing data
            appData.Transactions = transactions;  // Update the Transactions list
            SaveAppData(appData);  // Save updated AppData to file
        }

        // Get all income transactions for a user
        public List<TransactionModel> GetAllIncomeTransactions(int userId)
        {
            // Load the transactions from the data
            var transactions = LoadAppData().Transactions;

            // Filter transactions where the UserId matches and the Type is Credit (Income)
            var incomeTransactions = transactions
                .Where(t => t.UserId == userId && t.Type == TransactionModel.TransactionType.Credit) // Use enum value for Credit
                .ToList();

            // Return the filtered list of income transactions
            return incomeTransactions;
        }

        // Calculate total income for a user
        public decimal CalculateTotalIncome(int userId)
        {
            var transactions = LoadAppData().Transactions;

            // Filter transactions where the UserId matches and the Type is Credit (income)
            var incomeTransactions = transactions
                .Where(t => t.UserId == userId && t.Type == TransactionModel.TransactionType.Credit)
                .ToList();

            return incomeTransactions.Sum(t => t.Amount);
        }

        // Calculate total expenses for a user
        public decimal CalculateTotalExpenses(int userId)
        {
            var transactions = LoadAppData().Transactions;

            // Filter transactions where the UserId matches and the Type is Debit (expense)
            var expenseTransactions = transactions
                .Where(t => t.UserId == userId && t.Type == TransactionModel.TransactionType.Debit)
                .ToList();

            return expenseTransactions.Sum(t => t.Amount);
        }

        // Save transactions for a specific user
        public void SaveUserTransactions(int userId, List<TransactionModel> transactions)
        {
            var appData = LoadAppData();
            var userTransactions = appData.Transactions.Where(t => t.UserId != userId).ToList(); // Exclude user's existing transactions
            userTransactions.AddRange(transactions); // Add updated transactions for the user
            appData.Transactions = userTransactions;
            SaveAppData(appData);
        }

        // Get transactions for a specific user
        public List<TransactionModel> GetUserTransactions(int userId)
        {
            var appData = LoadAppData();
            return appData.Transactions.Where(t => t.UserId == userId).ToList();
        }

        public bool CheckSufficientBalance(int userId, decimal transactionAmount)
        {
            decimal totalIncome = CalculateTotalIncome(userId); // Sum of all credit (income) transactions
            decimal totalExpenses = CalculateTotalExpenses(userId); // Sum of all debit (expense) transactions

            decimal balance = totalIncome - totalExpenses;

            return balance >= transactionAmount; // Check if balance is enough for the transaction
        }

        public List<TransactionModel> FilterTransactions(int userId, string? type = null, List<string>? tags = null, DateTime? exactDate = null)
        {
            var transactions = LoadAppData().Transactions
                .Where(t => t.UserId == userId);

            // Filter by type (Credit/Debit)
            if (!string.IsNullOrEmpty(type))
            {
                transactions = transactions.Where(t => t.Type.ToString().Equals(type, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by tags
            if (tags != null && tags.Any())
            {
                transactions = transactions.Where(t => t.Tags != null && t.Tags.Intersect(tags).Any());
            }

            // Filter by exact date
            if (exactDate.HasValue)
            {
                transactions = transactions.Where(t => t.Date.Date == exactDate.Value.Date); // Comparing only the date part
            }

            return transactions.ToList();
        }

        public TransactionModel GetHighestIncome(int userId)
        {
            return LoadAppData().Transactions
                .Where(t => t.UserId == userId && t.Type == TransactionModel.TransactionType.Credit)
                .OrderByDescending(t => t.Amount)
                .FirstOrDefault();
        }

        // Get lowest income for a user
        public TransactionModel GetLowestIncome(int userId)
        {
            return LoadAppData().Transactions
                .Where(t => t.UserId == userId && t.Type == TransactionModel.TransactionType.Credit)
                .OrderBy(t => t.Amount)
                .FirstOrDefault();
        }

        // Get highest expense for a user
        public TransactionModel GetHighestExpense(int userId)
        {
            return LoadAppData().Transactions
                .Where(t => t.UserId == userId && t.Type == TransactionModel.TransactionType.Debit)
                .OrderByDescending(t => t.Amount)
                .FirstOrDefault();
        }

        // Get lowest expense for a user
        public TransactionModel GetLowestExpense(int userId)
        {
            return LoadAppData().Transactions
                .Where(t => t.UserId == userId && t.Type == TransactionModel.TransactionType.Debit)
                .OrderBy(t => t.Amount)
                .FirstOrDefault();
        }

        public List<TransactionModel> SortTransactionsByTransactionDate(List<TransactionModel> transactions, bool ascending = true)
        {
            if (ascending)
            {
                return transactions.OrderBy(t => t.Date).ToList();
            }
            else
            {
                return transactions.OrderByDescending(t => t.Date).ToList();
            }
        }

        public List<TransactionModel> FindTransactionsByTitle(int userId, string searchTitle)
        {
            return LoadAppData().Transactions
                .Where(t => t.UserId == userId &&
                            !string.IsNullOrEmpty(t.Title) &&
                            t.Title.Contains(searchTitle, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }






    }


