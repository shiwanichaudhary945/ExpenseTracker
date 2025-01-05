using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using ExpenseTracker.Models;
using System.Security.Cryptography;



    public class UserService
    {
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
        private static readonly string FilePath = Path.Combine(FolderPath, "data.json");  // Using appdata.json instead of users.json

        // Load AppData from json to obj
        public AppData LoadData()
        {
            if (!File.Exists(FilePath))
                return new AppData();  // Return a new AppData object if the file doesn't exist

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<AppData>(json) ?? new AppData();  // Deserialize the data or return a new AppData
        }

        // Save AppData (users, debts, transactions) to JSON
        public void SaveData(AppData appData)
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);  // Ensure the folder exists
            }

            var json = JsonSerializer.Serialize(appData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);  // Save the AppData to file
        }

        // Hash password using SHA256
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);  // Return the hashed password
        }

        // Validate password by comparing the hashed version
        public bool ValidatePassword(string inputPassword, string storedPassword)
        {
            var hashedInputPassword = HashPassword(inputPassword);
            return hashedInputPassword == storedPassword;  // Check if hashed input matches stored password
        }

        // Load Users from the file
        public List<Usermodel> LoadUsers()
        {
            var appData = LoadData();
            return appData.Users;
        }

        // Save Users to the file
        public void SaveUsers(List<Usermodel> users)
        {
            var appData = LoadData();
            appData.Users = users;
            SaveData(appData);
        }
    }


