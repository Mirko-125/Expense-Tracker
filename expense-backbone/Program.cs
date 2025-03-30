﻿using System.Text.Json;

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
                Console.WriteLine("Expense added successfully (ID: 1), use list for details");
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
        static void ListExpenses(List<Expense> expenses)
        {

        }
    }
}