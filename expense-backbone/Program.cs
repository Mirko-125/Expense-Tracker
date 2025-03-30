namespace ExpenseTracker
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Expense expense = new Expense();
            expense.Date = DateTime.UtcNow;
            Console.WriteLine(expense.Date);
            return 1;
        }
    }
}