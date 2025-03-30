namespace ExpenseTracker
{
    public enum Category
    {
        Luxury,
        Health,
        Friends,
        Resource
    }

    public class Expense
    {
        public int ID { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        // public Category Value { get; set; }
        public override string ToString()
        {
            return $"ID: {ID}, Description: {Description}, Amount: {Amount}, Spent on {Date} (UTC)";
        }
    }
}