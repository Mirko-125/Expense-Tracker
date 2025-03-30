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
        public string Description { get; set; } = "Unknown";
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        // public Category Value { get; set; }
    }
}