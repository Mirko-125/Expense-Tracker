using System.Text.Json;

namespace ExpenseTracker
{
    public class Program
    {
        private const string path = "data.json";
        public static int Main(string[] args)
        {
            var expenses = LoadData();
            ShowUsage();
            SaveData(expenses);
            return 1;
        }

        static void ShowUsage()
        {
            Console.WriteLine("Turorial:");
            Console.WriteLine("$ expense-tracker add --description \"Lunch\" --amount 20");
            Console.WriteLine("$ expense-tracker add --description \"Dinner\" --amount 10");
            Console.WriteLine("$ expense-tracker list");
            Console.WriteLine("$ expense-tracker summary");
            Console.WriteLine("$ expense-tracker delete --id 2");
            Console.WriteLine("$ expense-tracker summary");
            Console.WriteLine("$ expense-tracker summary --month 8");
        }

        static List<Expense> LoadData()
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
                return new List<Expense>();
            }
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
        }

        static void SaveData(List<Expense> expenses)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(expenses, options);
            File.WriteAllText(path, json);
        }

        static void AddExpense(List<Expense> expenses)
        {

        }
        static void UpdateExpense(List<Expense> expenses)
        {

        }
        static void DeleteExpense(List<Expense> expenses)
        {

        }
        static void ListExpenses(List<Expense> expenses)
        {

        }
    }
}