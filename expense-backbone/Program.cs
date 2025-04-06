using System.Text;
using System.Text.Json;

namespace ExpenseTracker
{
    // left TODO
    // Users can view a summary of all expenses.
    // Users can view a summary of expenses for a specific month (of current year).
    // will be a plus
    // Add expense categories and allow users to filter expenses by category.
    // Allow users to set a budget for each month and show a warning when the user exceeds the budget.
    public enum Month
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
    public class Program
    {
        private const string path = "data.json";
        public static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                ShowUsage();
                return 0;
            }
            string command = args[0].ToLower();
            var expenses = LoadData();
            try
            {
                switch (command)
                {
                    case "add":
                        if (args.Length != 5)
                        {
                            Console.WriteLine("Mistake is made when giving arguments, check the example underneath:");
                            ShowUsage();
                        }
                        AddExpense(expenses, args[2], args[4]);
                        break;
                    case "update":
                        if (args.Length != 3)
                        {
                            Console.WriteLine("Mistake is made when giving arguments, check the example underneath:");
                            ShowUsage();
                        }
                        UpdateExpense(expenses, args[2]);
                        break;
                    case "delete":
                        if (args.Length != 3)
                        {
                            Console.WriteLine("Mistake is made when giving arguments, check the example underneath:");
                            ShowUsage();
                        }
                        DeleteExpense(expenses, args[2]);
                        break;
                    case "list":
                        ListExpenses(expenses);
                        break;
                    case "export":
                        ExportToCSV(expenses);
                        break;
                    //Console.WriteLine("$ expense-tracker summary --month 8");
                    case "summary":
                        if (args.Length != 3)
                        {
                            Console.WriteLine("Mistake is made when giving arguments, check the example underneath:");
                            ShowUsage();
                        }
                        CalculateByMonth(expenses, args[2]);
                        break;
                    default:
                        Console.WriteLine("Unknown command, check the example underneath:");
                        ShowUsage();
                        return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: An exception has occured: {ex}");
            }

            SaveData(expenses);
            return 0;
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

        static void AddExpense(List<Expense> expenses, string description, string amount)
        {
            try
            {
                int newId = expenses.Count > 0 ? expenses[^1].ID + 1 : 1;
                int cash = Int32.Parse(amount);
                var newExpense = new Expense
                {
                    ID = newId,
                    Description = description,
                    Amount = cash,
                    Date = DateTime.UtcNow
                };

                expenses.Add(newExpense);
                Console.WriteLine($"Expense added successfully (ID: {newExpense.ID}), use list for details");
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter a number for the cash amount");
            }
        }
        static void UpdateExpense(List<Expense> expenses, string stringId)
        {
            try
            {
                int id = Int32.Parse(stringId);
                var expense = expenses.Find(e => e.ID == id);
                if (expense == null)
                {
                    Console.WriteLine("Expense with ID " + id + " not found.");
                    return;
                }
                Console.WriteLine("Editing: " + expense + ", which part do you want to edit?");
                Console.WriteLine("'d' for description");
                Console.WriteLine("'a' for amount");
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "d":
                        Console.WriteLine("Write the new description for the expense");
                        expense.Description = Console.ReadLine();
                        break;
                    case "a":
                        Console.WriteLine("Write the new amount of cash for the expense");
                        string? cash = Console.ReadLine();
                        if (null != cash)
                        {
                            expense.Amount = Int32.Parse(cash);
                        }
                        break;
                    default:
                        Console.WriteLine($"Error: {choice} is out of bounds");
                        return;
                }
                Console.WriteLine("Successfully updated expense: " + expense);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter a valid expense ID");
            }
        }
        static void DeleteExpense(List<Expense> expenses, string stringId)
        {
            try
            {
                int id = Int32.Parse(stringId);
                var expense = expenses.Find(e => e.ID == id);
                if (expense == null)
                {
                    Console.WriteLine("Expense with ID " + id + " not found.");
                    return;
                }
                expenses.Remove(expense);
                Console.WriteLine("Successfully deleted expense, expense remains: " + expense);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter a valid expense ID");
            }
        }
        static void ListExpenses(List<Expense> expenses) // string[] args
        {
            // filtering checking
            foreach (var expense in expenses)
            {
                // filtering
                Console.WriteLine(expense);
            }
        }

        #region Feature: CSV Formating
        static void ExportToCSV(List<Expense> expenses)
        {
            var csv = new StringBuilder();
            csv.AppendLine("ID,Description,Amount,Date");
            foreach (var expense in expenses)
            {
                string escapedDescription;
                if (null != expense.Description)
                {
                    escapedDescription = EscapeCsvField(expense.Description);
                }
                else
                {
                    escapedDescription = "N/A";
                }
                string formattedDate = expense.Date.ToString("yyyy-MM-dd HH:mm:ss");
                csv.AppendLine($"{expense.ID},{escapedDescription},{expense.Amount},{formattedDate}");
            }
            File.WriteAllText("expenses.csv", csv.ToString());
            Console.WriteLine("Successfully exported to CSV");
        }

        static string EscapeCsvField(string field)
        {
            if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
            {
                field = field.Replace("\"", "\"\"");
                return $"\"{field}\"";
            }
            return field;
        }
        #endregion

        #region experimenting
        static void CalculateByMonth(List<Expense> expenses, string monthInput)
        {
            int monthValue = Int32.Parse(monthInput);

            var filteredExpenses = expenses.Where(expense =>
            {
                return expense.Date.Month == monthValue;
            }).ToList();

            int sum = 0;

            foreach (var expense in filteredExpenses)
            {
                Console.WriteLine(expense);
                sum += expense.Amount;
            }
            double average = (double)sum / filteredExpenses.Count;
            Console.WriteLine($"Total sum of expenses done on the {monthValue}{(monthValue == 1 ? "st" : monthValue == 2 ? "nd" : monthValue == 3 ? "rd" : "th")} month is {sum}");
            Console.WriteLine($"While the average spent on the {monthValue}{(monthValue == 1 ? "st" : monthValue == 2 ? "nd" : monthValue == 3 ? "rd" : "th")} month is {average}");
        }
        #endregion
    }
}