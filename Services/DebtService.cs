using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using System.IO;



    public class DebtService
    {
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
        private static readonly string FilePath = Path.Combine(FolderPath, "data.json");

        // Load all data (debts) from the JSON file
        public AppData LoadAppData()
        {
            if (!File.Exists(FilePath))
                return new AppData();  // Return an empty AppData if the file doesn't exist

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<AppData>(json) ?? new AppData(); // Deserialize into AppData
        }

        // Save all data (debts) to the JSON file
        public void SaveAppData(AppData appData)
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            var json = JsonSerializer.Serialize(appData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);  // Save AppData to the file
        }

        // Save debts data
        public void SaveDebts(List<DebtModel> debts)
        {
            var appData = LoadAppData(); // Load existing data
            appData.Debts = debts;  // Update the Debts list
            SaveAppData(appData);  // Save updated AppData to file
        }

        // Get all debts for a specific user
        public List<DebtModel> GetUserDebts(int userId)
        {
            var debts = LoadAppData().Debts;

            // Filter debts where the UserId matches
            return debts.Where(d => d.UserId == userId).ToList();
        }

        // Get all debts (no user filter)
        public List<DebtModel> GetAllDebts()
        {
            return LoadAppData().Debts;  // Return all debts in the data
        }

        public List<TransactionModel> FilterTransactions(int userId, string? type = null, List<string>? tags = null, DateTime? date = null)
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

            // Filter by specific date
            if (date.HasValue)
            {
                transactions = transactions.Where(t => t.Date.Date == date.Value.Date);  // Compare only the date part
            }

            return transactions.ToList();
        }

    }


